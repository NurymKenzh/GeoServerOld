namespace GeoServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fields : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "public.admpol1",
            //    c => new
            //        {
            //            gid = c.Int(nullable: false, identity: true),
            //            catoid = c.Int(),
            //        })
            //    .PrimaryKey(t => t.gid)
            //    .ForeignKey("public.catoes", t => t.catoid)
            //    .Index(t => t.catoid);
            
            //CreateTable(
            //    "public.catoes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            AB = c.String(),
            //            CD = c.String(),
            //            EF = c.String(),
            //            HIJ = c.String(),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "public.admpol2",
            //    c => new
            //        {
            //            gid = c.Int(nullable: false, identity: true),
            //            catoid = c.Int(),
            //        })
            //    .PrimaryKey(t => t.gid)
            //    .ForeignKey("public.catoes", t => t.catoid)
            //    .Index(t => t.catoid);
            
            //CreateTable(
            //    "public.CropConditionNDVIs",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Date = c.DateTime(nullable: false),
            //            catoid = c.Int(),
            //            CropConditionTypeId = c.Int(),
            //            Value = c.Decimal(nullable: false, precision: 29, scale: 19),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("public.catoes", t => t.catoid)
            //    .ForeignKey("public.CropConditionTypes", t => t.CropConditionTypeId)
            //    .Index(t => t.catoid)
            //    .Index(t => t.CropConditionTypeId);
            
            //CreateTable(
            //    "public.CropConditionTypes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Max = c.Decimal(nullable: false, precision: 29, scale: 19),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "public.CropRotations",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CropRotationTypeId = c.Int(),
            //            fieldgid = c.Int(nullable: false),
            //            Year = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("public.CropRotationTypes", t => t.CropRotationTypeId)
            //    .ForeignKey("public.fields", t => t.fieldgid, cascadeDelete: true)
            //    .Index(t => t.CropRotationTypeId)
            //    .Index(t => t.fieldgid);
            
            //CreateTable(
            //    "public.CropRotationTypes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            NameRU = c.String(),
            //            NameKK = c.String(),
            //            NameEN = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "public.fields",
            //    c => new
            //        {
            //            gid = c.Int(nullable: false, identity: true),
            //            catoid = c.Int(),
            //            idfrommap = c.Int(nullable: false),
            //            area = c.Decimal(nullable: false, precision: 29, scale: 19),
            //        })
            //    .PrimaryKey(t => t.gid)
            //    .ForeignKey("public.catoes", t => t.catoid)
            //    .Index(t => t.catoid);
            
            //CreateTable(
            //    "public.DoneNDVIs",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Date = c.DateTime(nullable: false),
            //            catoid = c.Int(),
            //            MeanAverage = c.Decimal(nullable: false, precision: 29, scale: 19),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("public.catoes", t => t.catoid)
            //    .Index(t => t.catoid);
            
            //CreateTable(
            //    "public.FieldNDVIJsons",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Date = c.DateTime(nullable: false),
            //            catoid = c.Int(),
            //            Json = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("public.catoes", t => t.catoid)
            //    .Index(t => t.catoid);
            
            //CreateTable(
            //    "public.NDVIs",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            fieldgid = c.Int(nullable: false),
            //            Date = c.DateTime(nullable: false),
            //            Count = c.Int(nullable: false),
            //            Min = c.Decimal(nullable: false, precision: 29, scale: 19),
            //            Max = c.Decimal(nullable: false, precision: 29, scale: 19),
            //            Range = c.Decimal(nullable: false, precision: 29, scale: 19),
            //            Mean = c.Decimal(nullable: false, precision: 29, scale: 19),
            //            STD = c.Decimal(nullable: false, precision: 29, scale: 19),
            //            Sum = c.Decimal(nullable: false, precision: 29, scale: 19),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("public.fields", t => t.fieldgid, cascadeDelete: true)
            //    .Index(t => t.fieldgid);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("public.NDVIs", "fieldgid", "public.fields");
            //DropForeignKey("public.FieldNDVIJsons", "catoid", "public.catoes");
            //DropForeignKey("public.DoneNDVIs", "catoid", "public.catoes");
            //DropForeignKey("public.CropRotations", "fieldgid", "public.fields");
            //DropForeignKey("public.fields", "catoid", "public.catoes");
            //DropForeignKey("public.CropRotations", "CropRotationTypeId", "public.CropRotationTypes");
            //DropForeignKey("public.CropConditionNDVIs", "CropConditionTypeId", "public.CropConditionTypes");
            //DropForeignKey("public.CropConditionNDVIs", "catoid", "public.catoes");
            //DropForeignKey("public.admpol2", "catoid", "public.catoes");
            //DropForeignKey("public.admpol1", "catoid", "public.catoes");
            //DropIndex("public.NDVIs", new[] { "fieldgid" });
            //DropIndex("public.FieldNDVIJsons", new[] { "catoid" });
            //DropIndex("public.DoneNDVIs", new[] { "catoid" });
            //DropIndex("public.fields", new[] { "catoid" });
            //DropIndex("public.CropRotations", new[] { "fieldgid" });
            //DropIndex("public.CropRotations", new[] { "CropRotationTypeId" });
            //DropIndex("public.CropConditionNDVIs", new[] { "CropConditionTypeId" });
            //DropIndex("public.CropConditionNDVIs", new[] { "catoid" });
            //DropIndex("public.admpol2", new[] { "catoid" });
            //DropIndex("public.admpol1", new[] { "catoid" });
            //DropTable("public.NDVIs");
            //DropTable("public.FieldNDVIJsons");
            //DropTable("public.DoneNDVIs");
            //DropTable("public.fields");
            //DropTable("public.CropRotationTypes");
            //DropTable("public.CropRotations");
            //DropTable("public.CropConditionTypes");
            //DropTable("public.CropConditionNDVIs");
            //DropTable("public.admpol2");
            //DropTable("public.catoes");
            //DropTable("public.admpol1");
        }
    }
}
