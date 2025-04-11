using System.Data.Entity;

namespace ManagerProjects.Classes
{
    public class ProjectsContext : DbContext
    {
        public ProjectsContext() : base("DBConnection")
        { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}