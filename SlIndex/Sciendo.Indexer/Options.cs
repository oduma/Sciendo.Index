using CommandLine;

namespace Sciendo.Indexer
{
    class Options
    {
        [Value(1)]
        public string Path { get; set; }

        [Option('t', "type", Required = false,
          HelpText = "Type of indexing")]
        public IndexingType IndexMusicPath { get; set; }

    }
}
