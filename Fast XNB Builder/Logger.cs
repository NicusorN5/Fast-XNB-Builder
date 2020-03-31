using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Framework.Content.Pipeline.Builder;
using MonoGame.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Fast_XNB_Builder
{
    class Logger : ContentBuildLogger
    {
        public Logger()
        {

        }
        public override void LogImportantMessage(string message, params object[] messageArgs)
        {
            string a = "";
            for (int i = 0; i < messageArgs.Length; i++) a += messageArgs[i] + " ";
            Console.WriteLine("XNB Converter > [IMPORTANT]" + message+" "+a);
        }

        public override void LogMessage(string message, params object[] messageArgs)
        {
            string a = "";
            for (int i = 0; i < messageArgs.Length; i++) a += messageArgs[i] + " ";
            Console.WriteLine("XNB Converter > [INFO]" + message + " " + a);
        }

        public override void LogWarning(string helpLink, ContentIdentity contentIdentity, string message, params object[] messageArgs)
        {
            string a = "";
            for (int i = 0; i < messageArgs.Length; i++) a += messageArgs[i] + " ";
            Console.WriteLine("XNB Converter > [WARNING]" + message + " " + a);
        }
    }
}
