namespace Cloud_Transport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ASL_PCalendarImage",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Year = c.Long(nullable: false),
                        Month = c.String(nullable: false, maxLength: 128),
                        FilePath = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.Year, t.Month });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ASL_PCalendarImage");
        }
    }
}
