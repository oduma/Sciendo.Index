namespace Sciendo.Indexer.Agent
{
    public interface IIndexerAgent
    {
        int IndexLyricsOnDemand(string fromPath, string forMusicPath, string searchPattern);
        int IndexMusicOnDemand(string fromPath, string searchPattern);

    }
}
