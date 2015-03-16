using System;
using System.Linq;
using Id3;
using Id3.Id3;

namespace Sciendo.Music.Agent.Common
{
    public class SongInfo
    {
        public  SongInfo(string file)
        {
            IMp3Stream mp3File = new Mp3File(file);
            if (mp3File.HasTags && mp3File.AvailableTagVersions != null)
            {
                var version = Enumerable.FirstOrDefault<Version>(mp3File.AvailableTagVersions);
                if (version != null)
                {
                    IId3Tag id3Tag = null;
                    try
                    {
                        id3Tag = mp3File.GetTag(version.Major, version.Minor);
                    }
                    catch
                    {

                    }
                    if (id3Tag != null)
                    {
                        Artists =
                            Enumerable.Select<string, string>(id3Tag.Artists.Value,
                                a => string.Join("", Enumerable.Where<char>(a.ToCharArray(), c => ((int)c) >= 32)))
                                .ToArray();
                        Title = id3Tag.Title.TextValue;
                        Album = id3Tag.Album.TextValue;
                    }
                }
            }
        }


        public SongInfo()
        {
            Artists = null;
            Title = string.Empty;
            Album = string.Empty;
        }

        public string[] Artists { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
    }
}
