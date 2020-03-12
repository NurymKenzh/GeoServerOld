namespace GeoServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CropConditionType : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.CropConditionTypes", "NameRU", c => c.String());
            AddColumn("public.CropConditionTypes", "NameKK", c => c.String());
            AddColumn("public.CropConditionTypes", "NameEN", c => c.String());
            DropColumn("public.CropConditionTypes", "Name");
        }
        
        public override void Down()
        {
            AddColumn("public.CropConditionTypes", "Name", c => c.String());
            DropColumn("public.CropConditionTypes", "NameEN");
            DropColumn("public.CropConditionTypes", "NameKK");
            DropColumn("public.CropConditionTypes", "NameRU");
        }
    }
}
