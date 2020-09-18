using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAtlas;

namespace Avis
{
    class Renderer
    {
        //public delegate void RenderHandler(string output);
        //public event RenderHandler OnRendered = (output) => { };

        public bool Render(Ust ust, Singer singer, string tempFolder, string output, string resampler = "resampler.exe", string appendtool = "wavtool.exe")
        {
            var rendered = false;
            Program.Try(() =>
            {
                var bat = Path.Combine(tempFolder, @"render.bat");
                var request = BuildRequest(ust, singer, output, resampler, appendtool, tempFolder);

                var deleteCommand = $"del \"{tempFolder}\\*.wav\"\r\n";
                if (!File.Exists(bat))
                    File.Create(bat).Close();
                File.WriteAllText(bat, deleteCommand);
                File.AppendAllText(bat, request);

                File.AppendAllText(bat,
                    $"@if not exist \"{output}.whd\" goto E \r\n" +
                    $"@if not exist \"{output}.dat\" goto E \r\n" +
                    $"copy /Y \"{output}.whd\" /B + \"{output}.dat\" /B \"{output}\" \r\n" +
                    $"del \"{output}.whd\" \r\n" + $"del \"{output}.dat\" \r\n" + ":E");
                Execute(bat, tempFolder, output);
                rendered = true;
            });

            return rendered;
        }

        private void Execute(string filename, string folder, string output)
        {
            var bat = new Process();
            bat.StartInfo.FileName = filename;
            bat.StartInfo.WorkingDirectory = folder;
            bat.Start();
            bat.WaitForExit();
        }

        private string BuildRequest(Ust ust, Singer singer, string output, string resampler, string appendtool, string tempFolder)
        {
            stringBuilder = new StringBuilder();

            var notes = BuildRenderNotes(ust, singer);

            for (int i = 0; i < notes.Length; i++)
            {
                RenderNote note = notes[i];
                RenderNote next = notes.ElementAtOrDefault(i + 1);
                var tempFile = Path.Combine(tempFolder, $"_#{i.ToString().PadLeft(3, '0')}_[{note.ParsedLyric}]lyrics_[{note.NoteNum}]num_[{note.Length}]length.wav");
                if (note.IsRest)
                {
                    SendRestToAppendTool(note.FinalLength, output, tempFile, ust.Tempo, appendtool);
                }
                else
                {
                    SendToResampler(note, next, tempFile, ust.VoiceDir, resampler);
                    SendToAppendTool(note, next, tempFile, appendtool, ust.Tempo, output);
                }
            }

            var result = stringBuilder.ToString();
            stringBuilder.Clear();
            stringBuilder = null;
            return result;
        }

        private StringBuilder stringBuilder;

        private RenderNote[] BuildRenderNotes(Ust ust, Singer singer)
        {
            var renderNotes = new List<RenderNote>();
            for (int i = 0; i < ust.Notes.Length; i++)
            {
                var renderNote = RenderNote.CreateFromNote(ust.Notes[i]);
                renderNote.Oto = singer.FindOto(renderNote) ?? Oto.CreateDefault();
                renderNotes.Add(renderNote);
            }

            for (int i = 0; i < renderNotes.Count; i++)
            {
                var note = renderNotes[i];
                var next = renderNotes.ElementAtOrDefault(i + 1);
                note.Envelope = new Envelope(note, next);
            }

            var pitchController = new PitchController(ust.Tempo);
            for (int i = 0; i < renderNotes.Count; i++)
            {
                var prev = renderNotes.ElementAtOrDefault(i - 1);
                var curr = renderNotes.ElementAtOrDefault(i);
                var next = renderNotes.ElementAtOrDefault(i + 1);

                pitchController.BuildPitchData(curr, prev, next);
            }

            return renderNotes.ToArray();
        }

        private void SendToResampler(RenderNote note, RenderNote next, string tempFilename, string voiceDir, string resampler)
        {
            var pitchBase64 = Base64.Current.Base64EncodeInt12(note.PitchBend.Array);
            var oto = note.Oto;
            var otoFile = Path.Combine(voiceDir, oto.File);
            var stringNum = MusicMath.Current.NoteNum2String(note.NoteNum - 12);
            var requiredLength = GetRequiredLength(note, next);
            string request = string.Format(
                "\"{0}\" \"{1}\" \"{2}\" {3} {4} \"{5}\" {6} {7} {8} {9} {10} {11} !{12} {13}\r\n\r\n",
                resampler,
                otoFile,
                tempFilename,
                stringNum,
                note.Velocity * 100,
                note.Flags, //Part.Flags + note.Flags,
                oto.Offset,
                requiredLength,
                oto.Consonant,
                oto.Cutoff,
                note.Intensity,
                0.0, //note.Modulation,
                note.NoteNum,
                pitchBase64);
            stringBuilder.AppendLine(request);
        }


        /// <summary>
        ///     Send Note to AppendTool
        /// </summary>
        private void SendToAppendTool(RenderNote note, RenderNote next, string filename, string appendtool, double tempo, string output)
        {
            var offset = note.Oto.Preutterance;
            if (next != null)
                offset -= next.Oto.StraightPreutterance;

            var envelope = note.Envelope;
            var sign = offset >= 0 ? "+" : "-";
            var length = $"{note.FinalLength}@{tempo}{sign}{Math.Abs(offset).ToString("f0")}";
            string ops = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}",
                0, //note.Stp, // STP,
                length, //note.RequiredLength, 
                envelope.p1,
                envelope.p2,
                envelope.p3,
                envelope.v1,
                envelope.v2,
                envelope.v3,
                envelope.v4,
                note.Oto.Overlap,
                envelope.p4,
                envelope.p5,
                envelope.v5);
            var request = $"\"{appendtool}\" \"{output}\" \"{filename}\" {ops} \r\n";
            stringBuilder.AppendLine(request);
        }

        private int GetRequiredLength(RenderNote note, RenderNote next)
        {
            var len = note.FinalLength;
            double requiredLength = len + note.Oto.Preutterance;
            //if (next != null)
            //    requiredLength -= next.StraightPre;

            var stp = 0;
            requiredLength = Math.Ceiling((requiredLength + stp + 25) / 50) * 50;
            return (int)requiredLength;
        }
        public void SendRestToAppendTool(long duration, string output, string tempName, double tempo, string appendtool)
        {
            //var Part = Project.Current.Tracks[0].Parts[0];
            var length = $"{duration}@{tempo}+0";
            var ops = $"0 {length} 0 0";
            var request = $"\"{appendtool}\" \"{output}\" \"{tempName}\" {ops}\r\n";
            stringBuilder.AppendLine(request);
        }
    }
}
