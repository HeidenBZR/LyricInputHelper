using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Classes
{
    class Oto
    {
        public static string[] Dir;

        public Oto(string VoiceDir)
        {
            Dir = new string[] { VoiceDir};
            // а также посмотреть ото во вложенных папках
            Read();
        }

        public static void Read()
        {

        }
    }
}
