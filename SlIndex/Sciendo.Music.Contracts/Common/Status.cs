using System.Runtime.Serialization;
namespace Sciendo.Music.Contracts.Common
{
    [DataContract]
    public enum Status
    {
        [EnumMember]
        None,
        [EnumMember]
        Error,
        [EnumMember]
        LyricsDownloadedOk,
        [EnumMember]
        Done
    }
}
