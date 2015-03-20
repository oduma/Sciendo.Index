namespace Sciendo.Music.Contracts.Processing
{
    public interface ILyricsDeserializer
    {
        T DeserializeFromFile<T>(string fileName) where T : class;
        T Deserialize<T>(string xmlString) where T : class;
    }
}
