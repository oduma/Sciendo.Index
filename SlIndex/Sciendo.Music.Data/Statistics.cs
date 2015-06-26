namespace Sciendo.Music.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Sciendo.Music.Contracts.Analysis;
    using Sciendo.Music.Data.Configuration;

    public partial class Statistics : DbContext
    {
        public Statistics()
            : base("name=Statistics")
        {
        }

        public virtual DbSet<Element> Elements { get; set; }
        public virtual DbSet<Snapshot> Snapshots { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations
                .Add<Snapshot>(new SnapshotEntityConfiguration())
                .Add<Element>(new ElementEntityConfiguration());
        }
        
    }
}
