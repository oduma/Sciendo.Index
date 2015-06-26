using Sciendo.Music.Contracts.Analysis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Data.Configuration
{
    public class SnapshotEntityConfiguration:EntityTypeConfiguration<Snapshot>
    {
        public SnapshotEntityConfiguration()
        {
            this.HasKey<int>(c => c.SnapshotId);
            this.Property<int>(c => c.SnapshotId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property<DateTime>(c => c.TakenAt).HasColumnType("datetime2").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.ToTable("Snapshot");

        }
    }
}
