namespace Cloud_Transport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GL_MASTER", "TRIPNO", c => c.Long());
            AddColumn("dbo.GL_STRANS", "TRIPNO", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GL_STRANS", "TRIPNO");
            DropColumn("dbo.GL_MASTER", "TRIPNO");
        }
    }
}
