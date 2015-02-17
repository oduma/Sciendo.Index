using System;
using System.Collections.Generic;
using System.Linq;
using Id3;
using Id3.Id3;
using Sciendo.Index.Solr;

namespace Sciendo.Indexer.Agent
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
                    var version = Enumerable.FirstOrDefault<Version>(mp3File.AvailableTagVersions);
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
                            artists = Enumerable.Select<string, string>(id3Tag.Artists.Value, a=>string.Join("",Enumerable.Where<char>(a.ToCharArray(), c=>((int)c)>=32))).ToArray();
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
