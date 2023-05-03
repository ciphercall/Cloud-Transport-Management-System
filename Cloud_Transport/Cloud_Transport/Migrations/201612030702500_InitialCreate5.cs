namespace Cloud_Transport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GL_MTRANS", "TRIPNO", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GL_MTRANS", "TRIPNO");
        }
    }
}
