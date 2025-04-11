using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ManagerProjects.Classes
{
    public class Developer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; } //отчество

        [NotMapped]
        public string Name { get { return $"{LastName} {FirstName.First()}.{Patronymic.First()}."; } }
    }
}