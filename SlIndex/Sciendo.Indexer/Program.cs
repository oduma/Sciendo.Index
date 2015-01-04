using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Indexer
{
    class Program
    {
        static void Main(string[] args)
        {
            MusicFilesProcessor _fileProcessor = new MusicFilesProcessor();

            Reader reader = new Reader();
            reader.ProcessFiles = _fileProcessor.ProcessFilesBatch;
            reader.ParseDirectory(args[0], args[1]);
            Console.WriteLine(_fileProcessor.Counter);
            Console.ReadLine();

        }
    }
}
