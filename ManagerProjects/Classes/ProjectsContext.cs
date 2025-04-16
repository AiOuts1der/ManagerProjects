using System.Data.Entity;
using ManagerProjects.Migrations;

namespace ManagerProjects.Classes
{
    public class ProjectsContext : DbContext
    {
        public ProjectsContext() : base("DBConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProjectsContext, Configuration>());
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}