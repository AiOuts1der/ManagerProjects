using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ManagerProjects.Classes;

namespace ManagerProjects
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    // Главное окно приложения
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Начальная загрузка домашней страницы
            MainFrame.Navigate(new HomePage());
        }

        // Обработчик перехода на домашнюю страницу
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomePage());
        }

        // Обработчик создания проекта
        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            // Открытие окна создания проекта
            CreateWindow createWindow = new CreateWindow();
            if (createWindow.ShowDialog() == true && MainFrame.Content is HomePage homePage)
            {
                // Обновление списка проектов после создания
                homePage.projectGrid.ItemsSource = homePage.LoadFromDB();
            }
        }

        // Обработчик редактирования проекта
        private void Editor_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, что выбран проект для редактирования
            if (MainFrame.Content is HomePage homePage && homePage.projectGrid.SelectedItem is Project selectedProject)
            {
                // Открытие окна редактирования
                EditorWindow editorWindow = new EditorWindow(selectedProject);
                if (editorWindow.ShowDialog() == true)
                {
                    // Обновление списка проектов после редактирования
                    homePage.projectGrid.ItemsSource = homePage.LoadFromDB();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите проект для редактирования",
                              "Предупреждение",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
            }
        }

        // Обработчик удаления проекта
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, что выбран проект для удаления
            if (MainFrame.Content is HomePage homePage && homePage.projectGrid.SelectedItem is Project selectedProject)
            {
                // Подтверждение удаления
                var result = MessageBox.Show($"Вы действительно хотите удалить проект '{selectedProject.Title}'?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (ProjectsContext db = new ProjectsContext())
                        {
                            // Загрузка проекта со связанными данными
                            var projectToDelete = db.Projects
                                .Include("ResponsibleDeveloper")
                                .Include("Departments")
                                .FirstOrDefault(p => p.Id == selectedProject.Id);

                            if (projectToDelete != null)
                            {
                                // Удаление проекта
                                db.Projects.Remove(projectToDelete);
                                db.SaveChanges();

                                // Обновление списка проектов
                                homePage.projectGrid.ItemsSource = homePage.LoadFromDB();

                                MessageBox.Show("Проект успешно удален", "Успех",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении проекта: {ex.Message}",
                                      "Ошибка",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите проект для удаления",
                              "Предупреждение",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
            }
        }

        // Обработчик перехода на страницу данных
        private void Data_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DataPage());
        }

        // Обработчик открытия окна "О программе"
        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }
    }
}
