namespace Cloud_Transport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ASL_SchedularCalendar",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        USERID = c.Long(nullable: false),
                        Title = c.String(),
                        Text = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.COMPID, t.USERID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ASL_SchedularCalendar");
        }
    }
}
