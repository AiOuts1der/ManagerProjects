using ManagerProjects.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace ManagerProjects
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

            //SaveToDB(GetTestProjects());

            projectGrid.ItemsSource = LoadFromDB();
        }

        public List<Project> LoadFromDB()
        {
            List<Project> projects;

            using (ProjectsContext db = new ProjectsContext())
            {
                projects = db.Projects.Include("ResponsibleDeveloper").Include("Departments").ToList();
            }
            return projects;
        }

        private void SaveToDB(List<Project> projects)
        {
            using (ProjectsContext db = new ProjectsContext())
            {
                db.Projects.AddRange(projects);
                db.SaveChanges();
            }
        }

        private List<Project> GetTestProjects()
        {
            Directory.CreateDirectory(@"ProjectReleases\Employees");
            File.Create(@"ProjectReleases\Employees\Employees Version 1.0.0.0.zip");

            Directory.CreateDirectory(@"ProjectReleases\Sca"); //пропускной режим
            File.Create(@"ProjectReleases\Sca\Sca Version 1.1.0.0.zip");

            Directory.CreateDirectory(@"ProjectReleases\Canteen");
            File.Create(@"ProjectReleases\Canteen\Canteen Version 1.3.0.0.zip");

            Developer dev1 = new Developer { LastName = "Петров", FirstName = "Алексей", Patronymic = "Сергеевич" };
            Developer dev2 = new Developer { LastName = "Князев", FirstName = "Дмитрий", Patronymic = "Михайлович" };
            Developer dev3 = new Developer { LastName = "Машинский", FirstName = "Сергей", Patronymic = "Григорьевич" };

            Department depart1 = new Department { Title = "Отдел кадров" };
            Department depart2 = new Department { Title = "Охрана" };

            Project proj1 = new Project();
            proj1.Title = "Учёт сотрудников";
            proj1.StartDevelopementDate = new DateTime(2024, 01, 10);
            proj1.StartProductionDate = new DateTime(2024, 04, 15);
            proj1.CurrentVersion = "1.2.0.0";
            proj1.Status = Status.InProduction;
            proj1.ReleasesUrl = @"ProjectReleases\Employees";
            proj1.GitUrl = @"http://github.com/AiOuts1der/Employee";
            proj1.ResponsibleDeveloper = dev1;
            proj1.Departments = new List<Department> { depart1 };
            proj1.AmountWorkplaces = 5;
            proj1.Comment = "123";

            Project proj2 = new Project();
            proj2.Title = "Пропускной режим";
            proj2.StartDevelopementDate = new DateTime(2024, 08, 12);
            proj2.StartProductionDate = new DateTime(2024, 12, 16);
            proj2.CurrentVersion = "1.1.0.0";
            proj2.Status = Status.InProduction;
            proj2.ReleasesUrl = @"ProjectReleases\Sca";
            proj2.GitUrl = @"https://github.com/AiOuts1der/Sca";
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
                Status = Status.InProduction,
                ReleasesUrl = @"ProjectReleases\Canteen",
                GitUrl = @"https://github.com/AiOuts1der/Canteen",
                ResponsibleDeveloper = dev3,
                Departments = new List<Department> { depart1, depart2 },
                AmountWorkplaces = 15,
                Comment = "zxc"
            };

            return new List<Project>() { proj1, proj2, proj3 };
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void HyperlinkDir_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            string targetDirectory = e.Uri.ToString(); 
            Process.Start("explorer.exe", targetDirectory); 
            e.Handled = true;
        }


    }
}
