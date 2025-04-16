namespace ManagerProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255),
                        StartDevelopementDate = c.DateTime(nullable: false),
                        StartProductionDate = c.DateTime(nullable: false),
                        CurrentVersion = c.String(maxLength: 255),
                        Status = c.Int(nullable: false),
                        ReleasesUrl = c.String(maxLength: 255),
                        GitUrl = c.String(maxLength: 255),
                        AmountWorkplaces = c.Int(nullable: false),
                        Comment = c.String(maxLength: 255),
                        ResponsibleDeveloper_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Developers", t => t.ResponsibleDeveloper_Id)
                .Index(t => t.ResponsibleDeveloper_Id);
            
            CreateTable(
                "dbo.Developers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 255),
                        LastName = c.String(maxLength: 255),
                        Patronymic = c.String(maxLength: 255), //отчество
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectDepartments",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Department_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.Department_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ResponsibleDeveloper_Id", "dbo.Developers");
            DropForeignKey("dbo.ProjectDepartments", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.ProjectDepartments", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ProjectDepartments", new[] { "Department_Id" });
            DropIndex("dbo.ProjectDepartments", new[] { "Project_Id" });
            DropIndex("dbo.Projects", new[] { "ResponsibleDeveloper_Id" });
            DropTable("dbo.ProjectDepartments");
            DropTable("dbo.Developers");
            DropTable("dbo.Projects");
            DropTable("dbo.Departments");
        }
    }
}
