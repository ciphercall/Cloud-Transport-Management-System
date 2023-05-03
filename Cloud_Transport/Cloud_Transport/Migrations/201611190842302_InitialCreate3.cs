namespace Cloud_Transport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TMS_EXPENSE",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        TRANSMY = c.String(nullable: false, maxLength: 6),
                        TRANSNO = c.Long(nullable: false),
                        TRANSSL = c.Long(nullable: false),
                        TRANSDT = c.DateTime(),
                        TRIPNO = c.Long(),
                        COSTPID = c.Long(),
                        DEBITCD = c.Long(),
                        AMOUNT = c.Decimal(precision: 18, scale: 2),
                        REMARKS = c.String(maxLength: 100),
                        USERPC = c.String(),
                        INSUSERID = c.Long(nullable: false),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(nullable: false),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.TRANSMY, t.TRANSNO, t.TRANSSL });
            
            CreateTable(
                "dbo.TMS_TRIP",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        COSTPID = c.Long(nullable: false),
                        TRIPMY = c.String(nullable: false, maxLength: 6),
                        RTRIPNO = c.Long(nullable: false),
                        TRIPNO = c.Long(nullable: false),
                        TRIPDT = c.DateTime(),
                        TRIPFR = c.String(maxLength: 50),
                        TRIPTO = c.String(maxLength: 50),
                        TRIPTP = c.String(maxLength: 10),
                        PARTYID = c.Long(),
                        CDESTN = c.String(maxLength: 50),
                        AMTFARE = c.Decimal(precision: 18, scale: 2),
                        AMTDEMI = c.Decimal(precision: 18, scale: 2),
                        AMTOTC = c.Decimal(precision: 18, scale: 2),
                        AMTTOT = c.Decimal(precision: 18, scale: 2),
                        REMARKS = c.String(maxLength: 100),
                        USERPC = c.String(),
                        INSUSERID = c.Long(nullable: false),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(nullable: false),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.COSTPID, t.TRIPMY, t.RTRIPNO, t.TRIPNO });
            
            CreateTable(
                "dbo.TMS_TRIPMST",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        COSTPID = c.Long(nullable: false),
                        TRIPMY = c.String(nullable: false, maxLength: 6),
                        RTRIPNO = c.Long(nullable: false),
                        TRIPDT = c.DateTime(),
                        QTYFUEL = c.Decimal(precision: 18, scale: 2),
                        QTYMOBIL = c.Decimal(precision: 18, scale: 2),
                        REMARKS = c.String(maxLength: 100),
                        USERPC = c.String(),
                        INSUSERID = c.Long(nullable: false),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(nullable: false),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.COMPID, t.COSTPID, t.TRIPMY, t.RTRIPNO });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TMS_TRIPMST");
            DropTable("dbo.TMS_TRIP");
            DropTable("dbo.TMS_EXPENSE");
        }
    }
}
