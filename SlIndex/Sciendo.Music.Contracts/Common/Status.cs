using System.Runtime.Serialization;
namespace Sciendo.Music.Contracts.Common
{
    [DataContract]
    public enum Status
    {
        None,
        Error,
        LyricsDownloadedOk,
        Done
    }
}
