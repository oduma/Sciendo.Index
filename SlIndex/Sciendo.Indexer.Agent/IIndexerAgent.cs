namespace Sciendo.Indexer.Agent
{
    public interface IIndexerAgent
    {
        int IndexLyricsOnDemand(string fromPath, string searchPattern);
        int IndexMusicOnDemand(string fromPath, string searchPattern );

    }
}
