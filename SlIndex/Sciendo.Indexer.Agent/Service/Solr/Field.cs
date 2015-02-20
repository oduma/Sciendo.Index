namespace Sciendo.Indexer.Agent.Service.Solr
{
    public class Field<T>
    {
        public Field()
        {
            boost = 1d;
        }
        public T set { get; set; }

        public double boost { get; set; }
    }
}
