using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagerProjects.Classes
{
    public class Department
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public Department(string title)
        {
            Title = title;
        }

        
    }
}
