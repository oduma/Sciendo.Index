using System;
using System.IO;

namespace Sciendo.Music.Web.Files.Generator
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 2)
                return -1;

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Source directory does not exist.");
                return -1;
            }
            if (!Directory.Exists(args[1]))
            {
                Console.WriteLine("Target directory {0} does not exist",args[1]);
                return -1;
            }
            var targetFile = Path.Combine(args[1], "Sciendo.Music.Web.Files.wxs");
            if (!File.Exists(targetFile))
            {
                Console.WriteLine("Target file does not exist");
                return -1;
            }
            OutsideParameters.SourceDirectory = args[0];
            
            FragmentTemplate template= new FragmentTemplate();
            var text = template.TransformText();
            using(var fs = File.CreateText(targetFile))
            {
                fs.Write(text);
                fs.Close();
            }
            return 0;
        }
    }
}
