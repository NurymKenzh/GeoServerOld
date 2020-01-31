namespace GeoServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fields1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.fields", "croprotation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.fields", "croprotation");
        }
    }
}
