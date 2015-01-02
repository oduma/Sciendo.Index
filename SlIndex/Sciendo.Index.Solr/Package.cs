namespace Sciendo.Index.Solr
{
    public class Package
    {
        public PackageContent add { get; set; }
    }

    public class PackageContent
    {
        public Document doc { get; set; }

        public PackageContent()
        {
            overwrite = true;
            boost = 1d;
            commitWithin = 1000;
        }

        public double boost { get; set; }
        public bool overwrite { get; set; }
        public long commitWithin { get; set; }
    }
}
