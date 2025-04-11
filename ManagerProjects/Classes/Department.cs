using System.Collections.Generic;

namespace ManagerProjects.Classes
{
    public class Department
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}