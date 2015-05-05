using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Lyrics.Provider
{
    public class Options
    {
        [Value(0)]
        public string SourceDirectory { get; set; }

        [Value(1)]
        public string TargetDirectory { get; set; }

        [Value(2)]
        public string TraceFile { get; set; }


    }
}
