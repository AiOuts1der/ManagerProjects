using System.Data.Entity;

namespace ManagerProjects.Classes
{
    public class ProjectsContext : DbContext
    {
        public ProjectsContext() : base("DBConnection")
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Уникальность названия проекта
            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Title)
                .IsUnique();

            // Уникальность названия подразделения
            modelBuilder.Entity<Department>()
                .HasIndex(d => d.Title)
                .IsUnique();

            modelBuilder.Entity<Developer>()
                .HasIndex(d => new { d.LastName, d.FirstName, d.Patronymic })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}