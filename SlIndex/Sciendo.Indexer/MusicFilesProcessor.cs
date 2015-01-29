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
    public class MusicFilesProcessor:FilesProcessor
    {

        protected override IEnumerable<Document> PrepareDocuments(IEnumerable<string> files, string rootFolder)
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
                            artists = id3Tag.Artists.Value.Select(a=>string.Join("",a.ToCharArray().Where(c=>((int)c)>=32))).ToArray();
                            title = id3Tag.Title.TextValue;
                            album = id3Tag.Album.TextValue;
                        }
                    }
                }
        
                yield return new FullDocument(file, rootFolder,artists,title,album);
            }
        }

    }
}
