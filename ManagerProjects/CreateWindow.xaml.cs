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
using System.Windows.Shapes;
using ManagerProjects.Classes;

namespace ManagerProjects
{
    // Окно для создания новых проектов
    public partial class CreateWindow : Window
    {
        // Новый проект
        private readonly Project _newProject;
        // Список всех подразделений
        private List<Department> _allDepartments;

        public CreateWindow()
        {
            InitializeComponent();

            // Инициализация нового проекта с пустыми значениями
            _newProject = new Project
            {
                ResponsibleDeveloper = new Developer(),
                Departments = new List<Department>()
            };

            // Загрузка подразделений
            LoadDepartments();
        }

        // Загрузка подразделений из БД
        private void LoadDepartments()
        {
            try
            {
                using (ProjectsContext db = new ProjectsContext())
                {
                    // Получение подразделений без дубликатов
                    _allDepartments = db.Departments
                        .AsEnumerable()
                        .GroupBy(d => d.Title, StringComparer.OrdinalIgnoreCase)
                        .Select(g => g.First())
                        .ToList();
                }

                // Привязка данных к ListBox
                lbDepartments.ItemsSource = _allDepartments
                    .Select(d => new DepartmentViewModel
                    {
                        Id = d.Id,
                        Title = d.Title,
                        IsSelected = false // Все подразделения изначально не выбраны
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке подразделений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        // Обработчик создания проекта
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация обязательных полей
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Введите название проекта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (dpStartDev.SelectedDate == null)
                {
                    MessageBox.Show("Укажите дату начала разработки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Заполнение данных проекта из формы
                _newProject.Title = txtTitle.Text;
                _newProject.StartDevelopmentDate = dpStartDev.SelectedDate.Value;
                _newProject.StartProductionDate = (DateTime)dpStartProd.SelectedDate;
                _newProject.CurrentVersion = txtVersion.Text;
                _newProject.Status = cbStatus.SelectedIndex != -1 ? (Status)cbStatus.SelectedIndex : Status.InDevelopment;
                _newProject.ReleasesUrl = txtReleasesUrl.Text;
                _newProject.GitUrl = txtGitUrl.Text;

                // Обработка числового поля
                if (int.TryParse(txtWorkplaces.Text, out int workplaces))
                {
                    _newProject.AmountWorkplaces = workplaces;
                }
                else
                {
                    _newProject.AmountWorkplaces = 0;
                }

                _newProject.Comment = txtComment.Text;

                // Данные разработчика
                _newProject.ResponsibleDeveloper.LastName = txtLastName.Text;
                _newProject.ResponsibleDeveloper.FirstName = txtFirstName.Text;
                _newProject.ResponsibleDeveloper.Patronymic = txtPatronymic.Text;

                // Получение выбранных подразделений
                var selectedDepartments = lbDepartments.ItemsSource
                    .Cast<DepartmentViewModel>()
                    .Where(d => d.IsSelected)
                    .Select(d => _allDepartments.First(ad => ad.Id == d.Id))
                    .ToList();

                _newProject.Departments = selectedDepartments;

                // Сохранение в БД
                using (ProjectsContext db = new ProjectsContext())
                {
                    db.Developers.Add(_newProject.ResponsibleDeveloper);
                    db.Projects.Add(_newProject);
                    db.SaveChanges();
                }

                MessageBox.Show("Проект успешно создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании проекта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик отмены создания
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Обработчик добавления подразделения
        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            // Диалог для ввода названия нового подразделения
            var inputDialog = new InputDialog("Добавить подразделение", "Введите название подразделения:");
            if (inputDialog.ShowDialog() == true)
            {
                // Создание и сохранение подразделения
                var newDepartment = new Department { Title = inputDialog.Answer };

                using (ProjectsContext db = new ProjectsContext())
                {
                    db.Departments.Add(newDepartment);
                    db.SaveChanges();
                }

                // Обновление списка
                _allDepartments.Add(newDepartment);
                lbDepartments.ItemsSource = _allDepartments.Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    Title = d.Title,
                    IsSelected = false
                }).ToList();
            }
        }
    }
}
