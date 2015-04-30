using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Web.Files.Generator
{
    public static class OutsideParameters
    {
        public static string SourceDirectory { get; set; }

        public static IEnumerable<string> AvoidDirectories = new String[]
        {"App_Start", "Controllers", "Hubs", "obj", "Properties", "Service References"};

        public static IEnumerable<string> AvoidExtensions = new String[] { ".config",".log",".cs",".csproj",".user",".orig" }; 

        public static List<string> ComponentIds = new List<string>(); 
        private static void Get()
        {
            var s = "";
            s.Split(new char[] {Path.DirectorySeparatorChar}).Last();
        }
    }
}
