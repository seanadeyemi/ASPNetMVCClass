namespace SampleApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_Category_Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "Description");
        }
    }
}
