namespace Cloud_Transport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GL_COSTPOOL", "BUYDT", c => c.DateTime());
            AddColumn("dbo.GL_COSTPOOL", "REGDT", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GL_COSTPOOL", "REGDT");
            DropColumn("dbo.GL_COSTPOOL", "BUYDT");
        }
    }
}
