using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using System.Windows.Navigation;
using ManagerProjects.Classes;

namespace ManagerProjects
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            InitializeDatabase(); // Создаём структуру БД
            projectGrid.ItemsSource = LoadFromDB();
        }

        public List<Project> LoadFromDB()
        {
            using (ProjectsContext db = new ProjectsContext())
            {
                return db.Projects
                    .Include(p => p.ResponsibleDeveloper)
                    .Include(p => p.Departments)
                    .ToList();
            }
        }

        private void InitializeDatabase()
        {
            using (ProjectsContext db = new ProjectsContext())
            {
                // Проверяем, существует ли база, и создаём её при необходимости
                if (!db.Database.Exists())
                {
                    db.Database.Create();
                }
            }
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