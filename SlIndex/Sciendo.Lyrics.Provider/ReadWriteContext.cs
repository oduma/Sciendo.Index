using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using Id3;
using Sciendo.Lyrics.Common;

namespace Sciendo.Lyrics.Provider
{
    public class ReadWriteContext:ArtistSong
    {
        [XmlIgnore]
        public Action<Status, string, string> Progress { get; set; }
        public string ReadLocation { get; set; }

        public Uri Url { get
        {
            return new Uri(string.Format("http://lyrics.wikia.com/api.php?func=getSong&artist={0}&song={1}&fmt=xml", artist,
                    song));
        }}

        public Status Status { get; set; }

        public static IMp3Stream DefaultMp3FileLoader(string filePath)
        {
            return new Mp3File(filePath);
        }

        public virtual ReadWriteContext ProcessFile(Func<string,IMp3Stream> Mp3FileLoader)
        {
            if (Status == Status.ArtistSongRetrievedFromFile)
            {
                if(Progress!=null)
                    Progress(Status, ReadLocation, "");
                return this;
            }
            if (!File.Exists(ReadLocation))
            {
                Status = Status.FileNotFound;
                if (Progress != null)
                    Progress(Status, ReadLocation, "");
                return this;
            }
            IMp3Stream mp3File = Mp3FileLoader(ReadLocation);
            if (!mp3File.HasTags || mp3File.AvailableTagVersions==null)
            {
                Status = Status.FileNotTagged;
                if (Progress != null)
                    Progress(Status, ReadLocation, "");
                return this;
            }
            var version = mp3File.AvailableTagVersions.FirstOrDefault();
            if (version == null)
            {
                Status = Status.UnknownTagVersion;
                if (Progress != null)
                    Progress(Status, ReadLocation, "");
                return this;
            }
            var id3Tag = mp3File.GetTag(version.Major, version.Minor);
            if(id3Tag== null || id3Tag.Artists==null || string.IsNullOrEmpty(id3Tag.Title))
            {
                Status = Status.FileNotTagged;
                if (Progress != null)
                    Progress(Status, ReadLocation, "");
                return this;
            }
            Status = Status.ArtistSongRetrievedFromFile;
            artist = string.Join("",id3Tag.Artists.TextValue.Replace(" ", "_").ToCharArray().Where(c=>(int)c>=32));
            song = id3Tag.Title.TextValue.Replace(" ", "_");
            return this;
        }

        public ReadWriteContext TakeFromWeb(WebClient webClient, string sourceRootDirectory, string targetRootDirectory)
        {
            if (Status == Status.FileNotFound || Status == Status.FileNotTagged || Status == Status.UnknownTagVersion)
                return this;
            if (Status != Status.ArtistSongRetrievedFromFile && Status != Status.LyricsUrlUnreachable)
            {
                if (Progress != null)
                    Progress(Status, ReadLocation, "");
                return this;
            }
            try
            {
                var downloadedFromApi = webClient.DownloadString(Url);
                if(downloadedFromApi.IndexOf(@"<lyrics>Not found</lyrics>")>0)
                {
                    Status = Status.LyricsDownloadedOk;
                    return this;
                }
                downloadedFromApi.Replace("\0","");
                var fullTargetPath = Path.ChangeExtension(ReadLocation.Replace(sourceRootDirectory, targetRootDirectory), "lrc");
                var directoryPath = fullTargetPath.Replace(Path.GetFileName(fullTargetPath), "");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                using (StreamWriter fs = File.CreateText(fullTargetPath))
                {
                    fs.Write(downloadedFromApi);
                    Status = Status.LyricsDownloadedOk;
                }
            }
            catch
            {
                Status = Status.LyricsUrlUnreachable;
            }
            if (Progress != null)
                Progress(Status, ReadLocation, string.Format("artist: {0}; song: {1}", artist, song));
            return this;
        }
    }
}
