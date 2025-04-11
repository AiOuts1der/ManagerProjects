using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace ManagerProjects.Classes
{
    public class ProjectContext : DbContext
    {
        public ProjectContext() : base("DBconnection")
        { }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Developer> Developers { get; set; }

        public DbSet<Department> Departments { get; set; }
    }

}
