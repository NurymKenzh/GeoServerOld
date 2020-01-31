using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    public class NpgsqlContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public NpgsqlContext() : base("name=NpgsqlContext")
        {
        }

        public System.Data.Entity.DbSet<GeoServer.Models.admpol1> Admpol1 { get; set; }

        public System.Data.Entity.DbSet<GeoServer.Models.admpol2> Admpol2 { get; set; }

        public System.Data.Entity.DbSet<GeoServer.Models.CATO> CATOes { get; set; }

        public System.Data.Entity.DbSet<GeoServer.Models.field> fields { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<admpol1>().HasKey(a => a.gid);
            modelBuilder.Entity<admpol2>().HasKey(a => a.gid);

            modelBuilder.Entity<field>().HasKey(f => f.gid);
            //modelBuilder.Entity<field>().HasKey(f => f.idfrommap);
            modelBuilder.Entity<field>().Property(f => f.area).HasPrecision(29, 19);

            //modelBuilder.Entity<NDVI>().Property(n => n.Area).HasPrecision(29, 19);
            modelBuilder.Entity<NDVI>().Property(n => n.Min).HasPrecision(29, 19);
            modelBuilder.Entity<NDVI>().Property(n => n.Max).HasPrecision(29, 19);
            modelBuilder.Entity<NDVI>().Property(n => n.Range).HasPrecision(29, 19);
            modelBuilder.Entity<NDVI>().Property(n => n.Mean).HasPrecision(29, 19);
            modelBuilder.Entity<NDVI>().Property(n => n.STD).HasPrecision(29, 19);
            modelBuilder.Entity<NDVI>().Property(n => n.Sum).HasPrecision(29, 19);

            modelBuilder.Entity<DoneNDVI>().Property(d => d.MeanAverage).HasPrecision(29, 19);

            modelBuilder.Entity<CropConditionType>().Property(c => c.Max).HasPrecision(29, 19);

            modelBuilder.Entity<CropConditionNDVI>().Property(c => c.Value).HasPrecision(29, 19);

            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<GeoServer.Models.NDVI> NDVIs { get; set; }

        public System.Data.Entity.DbSet<GeoServer.Models.CropRotationType> CropRotationTypes { get; set; }

        public System.Data.Entity.DbSet<GeoServer.Models.CropRotation> CropRotations { get; set; }

        public System.Data.Entity.DbSet<GeoServer.Models.DoneNDVI> DoneNDVIs { get; set; }

        public System.Data.Entity.DbSet<GeoServer.Models.CropConditionType> CropConditionTypes { get; set; }

        public System.Data.Entity.DbSet<GeoServer.Models.CropConditionNDVI> CropConditionNDVIs { get; set; }

        public System.Data.Entity.DbSet<GeoServer.Models.FieldNDVIJson> FieldNDVIJsons { get; set; }
    }
}
