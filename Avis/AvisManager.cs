using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using VAtlas;
using NAudio.Wave;

namespace Avis
{
    class AvisManager
    {
        public readonly Ust Ust;
        public readonly Atlas Atlas;
        public readonly Singer Singer;

        public AvisManager (Ust ust, Atlas atlas, Singer singer)
        {
            Ust = ust;
            Singer = singer;
            Atlas = atlas;
        }

        public void Start()
        {
            Errors.Log("Avis started...");

            Errors.Log("Convert started...");
            var convertingPassed = Convert();
            if (!convertingPassed)
                return;
            Errors.Log("Convert finished.");

            Errors.Log("Render started...");
            var rendered = false;

            var avisFolder = PathResolver.GetTempFolder("Avis");
            var tempFolder = Path.Combine(avisFolder, "Temp");
            var resampler = Path.Combine(avisFolder, "resampler.exe");
            var appendtool = Path.Combine(avisFolder, "wavtool.exe");
            var output = "output.wav";
            Program.Try(() =>
            {
                var renderer = new Renderer();
                rendered = renderer.Render(Ust, Singer, tempFolder, output, resampler, appendtool);
            }, "Render failed");
            if (!rendered)
                return;

            HandleRendered(Path.Combine(tempFolder, output));

            Errors.Log("Avis finished.");
        }

        private void HandleRendered(string output)
        {
            Errors.Log("Render finished.");

            if (!File.Exists(output))
            {
                Errors.Log("Output file doesn't exist.");
                return;
            }
            Errors.Log("Play output.");

            var wavReader = new WaveFileReader(output);
            var waveChannel = new WaveChannel32(wavReader);
            var player = new WaveOutEvent();
            player.Init(waveChannel);
            player.PlaybackStopped += delegate { playFinished = true; };
            player.Play();
            while (!playFinished)
            {
                Thread.Sleep(1);
            }
        }

        private bool playFinished;

        private bool Convert()
        {
            var convertingPassed = false;
            Program.Try(() =>
            {
                var minLength = 110 * (int)Ust.Tempo / 120;
                var atlasSettings = new AtlasSettings()
                {
                    LengthByOto = true,
                    IsParsed = true,
                    MinLengthDefault = minLength,
                    MinLength = minLength,
                    MakeFade = true,
                    CompressionRatio = 1.1,
                    LastChildCompressionRatio = 1.4
                };
                var parser = new Parser(Atlas, Ust);
                parser.Split();
                parser.AtlasConverting(atlasSettings);
                Ust.SetLength(atlasSettings);
                convertingPassed = true;
            }, "Ust converting failed");
            return convertingPassed;
        }


    }
}
