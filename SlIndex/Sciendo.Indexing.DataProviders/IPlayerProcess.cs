namespace Sciendo.Music.DataProviders
{
    public interface IPlayerProcess
    {
        bool AddSongToQueue(string filePath, string withProcess);
    }
}
