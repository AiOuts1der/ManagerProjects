using ManagerProjects.Classes;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace ManagerProjects
{
    public partial class HomePage : Page
    {   
        public HomePage() 
        {
            InitializeComponent();

            List<Project> loadedList;

            using (ProjectContext db = new ProjectContext())
            {
                //db.Projects.AddRange(GetTestProjects());
                //db.SaveChanges();
               loadedList = db.Projects.ToList();
            }

            listBox.ItemsSource = loadedList;
        }

        private List<Project> GetTestProjects()
        {
            Developer dev1 = new Developer("Петров", "Алексей", "Сергеевич");
            Developer dev2 = new Developer("Князев", "Дмитрий", "Михайлович");
            Developer dev3 = new Developer("Машинский", "Сергей", "Григорьевич");

            Department depart1 = new Department("Отдел кадров");
            Department depart2 = new Department("Охрана");

            Project proj1 = new Project();
            proj1.Title = "Учёт сотрудников";
            proj1.StartDevelopementDate = new DateTime(2024, 01, 10);
            proj1.StartProductionDate = new DateTime(2024, 04, 15);
            proj1.CurrentVersion = "1.2.0.0";
            proj1.Status = Status.Production;
            proj1.ReleasesUrl = @"C:\Releases";
            proj1.GitUrl = @"\\GitServer\Employees";
            proj1.ResponsibleDeveloper = dev1;
            proj1.Departments = new List<Department> { depart1 };
            proj1.AmountWorkplaces = 5;
            proj1.Comment = "123";

            Project proj2 = new Project();
            proj2.Title = "Пропускной режим";
            proj2.StartDevelopementDate = new DateTime(2024, 08, 12);
            proj2.StartProductionDate = new DateTime(2024, 12, 16);
            proj2.CurrentVersion = "1.1.0.0";
            proj2.Status = Status.Production;
            proj2.ReleasesUrl = @"C:\Releases";
            proj2.GitUrl = @"\\GitServer\Sca";
            proj2.ResponsibleDeveloper = dev2;
            proj2.Departments = new List<Department> { depart1, depart2 };
            proj2.AmountWorkplaces = 10;
            proj2.Comment = "zxc";

            Project proj3 = new Project()
            {
                Title = "Столовая",
                StartDevelopementDate = new DateTime(2024, 05, 10),
                StartProductionDate = new DateTime(2024, 07, 20),
                CurrentVersion = "1.0.2.0",
                Status = Status.Production,
                ReleasesUrl = @"C:\Releases",
                GitUrl = @"\\GitServer\Canteen",
                ResponsibleDeveloper = dev3,
                Departments = new List<Department> { depart1, depart2 },
                AmountWorkplaces = 15,
                Comment = "zxc"
            };

            return new List<Project>() { proj1, proj2, proj3 };
        }
    }
}
