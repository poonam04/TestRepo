namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageData",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        ImageName = c.String(),
                        DataResponse = c.String(),
                        Image = c.Binary(),
                        DateAdded = c.DateTime(nullable: false),
                        IsLive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ImageId);
            
            CreateTable(
                "dbo.TagData",
                c => new
                    {
                        ImageTagId = c.Int(nullable: false, identity: true),
                        ImageId = c.Int(nullable: false),
                        Name = c.String(),
                        DateAdded = c.DateTime(nullable: false),
                        IsLive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ImageTagId);
            
            CreateTable(
                "dbo.UsersData",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        EmailID = c.String(maxLength: 100),
                        DateCreated = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UsersData");
            DropTable("dbo.TagData");
            DropTable("dbo.ImageData");
        }
    }
}
