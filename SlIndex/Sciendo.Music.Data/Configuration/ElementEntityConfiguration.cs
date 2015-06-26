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
    public class ElementEntityConfiguration:EntityTypeConfiguration<Element>
    {
        public ElementEntityConfiguration()
        {
            this.Property<int>(c => c.ElementId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property<LyricsFileFlag>(c => c.LyricsFileFlag).HasColumnType("int");
            this.Property<MusicFileFlag>(c => c.MusicFileFlag).HasColumnType("int");
            this.Property<bool>(c => c.IsIndexed).HasColumnType("bit");

            this.HasKey<int>(c => c.ElementId).ToTable("Element");

        }
    }
}
