using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ManagerProjects.Classes;

namespace ManagerProjects
{
    // Страница со статистикой
    public partial class DataPage : Page
    {
        public DataPage()
        {
            InitializeComponent();
            // Загрузка статистики при создании страницы
            LoadStatistics();
        }

        // Метод загрузки статистических данных
        private void LoadStatistics()
        {
            try
            {
                using (var db = new ProjectsContext())
                {
                    // Статистика по проектам
                    var projects = db.Projects.ToList();
                    txtTotalProjects.Text = projects.Count.ToString();
                    txtInDevelopment.Text = projects.Count(p => p.Status == Status.InDevelopment).ToString();
                    txtInClose.Text = projects.Count(p => p.Status == Status.InClose).ToString();
                    txtInProduction.Text = projects.Count(p => p.Status == Status.InProduction).ToString();
                    txtArchived.Text = projects.Count(p => p.Status == Status.InArchive).ToString();

                    // Статистика по подразделениям
                    var departments = db.Departments.ToList();
                    txtTotalDepartments.Text = departments.Count.ToString();
                    txtDepartmentsWithProjects.Text = departments.Count(d => d.Projects.Any()).ToString();

                    // Статистика по разработчикам
                    var developers = db.Developers.ToList();
                    txtTotalDevelopers.Text = developers.Count.ToString();
                    txtDevelopersInProjects.Text = developers.Count(d => db.Projects.Any(p => p.ResponsibleDeveloper.Id == d.Id)).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статистики: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}