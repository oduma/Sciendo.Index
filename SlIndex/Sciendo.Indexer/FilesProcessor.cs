using Id3;
using Id3.Id3;
using Newtonsoft.Json;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Indexer
{
    public class FilesProcessor
    {
        private int _counter;

        public FilesProcessor(string rootFolder)
        {
            _counter = 0;
            RootFolder = rootFolder;
            _packageTraces = new List<PackageTrace>();
        }

        private List<PackageTrace> _packageTraces;
        public int Counter { get { return _counter; } }

        public IEnumerable<Document> PrepareDocuments(IEnumerable<string> files, string rootFolder)
        {
            string[] artists = null;
            string title=string.Empty;
            string album=string.Empty;

            foreach (var file in files)
            {
                IMp3Stream mp3File = new Mp3File(file);
                if (mp3File.HasTags && mp3File.AvailableTagVersions != null)
                {
                    var version = mp3File.AvailableTagVersions.FirstOrDefault();
                    if (version != null)
                    {
                        IId3Tag id3Tag=null;
                        try
                        {
                            id3Tag= mp3File.GetTag(version.Major, version.Minor);
                        }
                        catch { }
                        if (id3Tag != null)
                        {
                            artists = id3Tag.Artists.Value.ToArray();
                            title = id3Tag.Title.TextValue;
                            album = id3Tag.Album.TextValue;
                        }
                    }
                }
        
                yield return new Document(file, rootFolder,artists,title,album);
            }
        }

        public void ProcessFilesBatch(IEnumerable<string> files)
        {
            var package = PrepareDocuments(files, RootFolder).ToArray();
            //_packageTraces.Add(new PackageTrace { Package = package, Response = new TrySendResponse { Status = Status.NotIndexed } });
            var response = SolrSender.TrySend("http://localhost:8080/solr/medialib/update/json?commitWithin=1000", package);
            if (response.Status != Status.Done)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("error indexing: {0}",JsonConvert.SerializeObject(package));
                Console.ResetColor();
            }
            _counter += package.Length;
        }



        public string RootFolder { get; private set; }
    }
}
