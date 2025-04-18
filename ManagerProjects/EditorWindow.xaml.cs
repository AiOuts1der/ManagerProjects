﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ManagerProjects.Classes;

namespace ManagerProjects
{
    /// <summary>
    /// Логика взаимодействия для EditorWindow.xaml
    /// </summary>
    // Окно для редактирования существующих и создания новых проектов
    public partial class EditorWindow : Window
    {
        // Текущий редактируемый проект
        private Project _currentProject;
        // Список всех подразделений из БД
        private List<Department> _allDepartments;

        // Конструктор принимает проект для редактирования или создает новый
        public EditorWindow(Project project = null)
        {
            InitializeComponent();

            // Инициализация проекта (нового или переданного)
            _currentProject = project ?? new Project
            {
                StartDevelopmentDate = DateTime.Now,
                StartProductionDate = DateTime.Now
            };
            // Инициализация связанных объектов, если они null
            _currentProject.ResponsibleDeveloper = _currentProject.ResponsibleDeveloper ?? new Developer();
            _currentProject.Departments = _currentProject.Departments ?? new List<Department>();

            // Загрузка данных
            LoadDepartments();
            LoadProjectData();
        }

        // Загрузка списка подразделений из БД
        private void LoadDepartments()
        {
            try
            {
                using (ProjectsContext db = new ProjectsContext())
                {
                    // Получаем подразделения, исключая дубликаты по названию
                    _allDepartments = db.Departments
                        .AsEnumerable()
                        .GroupBy(d => d.Title, StringComparer.OrdinalIgnoreCase)
                        .Select(g => g.First())
                        .ToList();
                }

                // Получаем ID подразделений текущего проекта
                var projectDepartmentIds = _currentProject.Departments?
                    .Select(d => d.Id)
                    .Distinct()
                    .ToList() ?? new List<int>();

                // Создаем модели для отображения с флажками выбора
                lbDepartments.ItemsSource = _allDepartments
                    .Select(d => new DepartmentViewModel
                    {
                        Id = d.Id,
                        Title = d.Title,
                        IsSelected = projectDepartmentIds.Contains(d.Id)
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке подразделений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        // Загрузка данных проекта в форму
        private void LoadProjectData()
        {
            // Заполнение полей формы данными из проекта
            txtTitle.Text = _currentProject.Title;
            if (_currentProject.StartDevelopmentDate > DateTime.MinValue)
                dpStartDev.SelectedDate = _currentProject.StartDevelopmentDate;

            if (_currentProject.StartProductionDate > DateTime.MinValue)
                dpStartProd.SelectedDate = _currentProject.StartProductionDate;

            // Остальные поля формы
            txtVersion.Text = _currentProject.CurrentVersion;
            cbStatus.SelectedIndex = (int)_currentProject.Status;
            txtReleasesUrl.Text = _currentProject.ReleasesUrl;
            txtGitUrl.Text = _currentProject.GitUrl;
            txtWorkplaces.Text = _currentProject.AmountWorkplaces.ToString();
            txtComment.Text = _currentProject.Comment;

            // Данные ответственного разработчика
            if (_currentProject.ResponsibleDeveloper != null)
            {
                txtLastName.Text = _currentProject.ResponsibleDeveloper.LastName;
                txtFirstName.Text = _currentProject.ResponsibleDeveloper.FirstName;
                txtPatronymic.Text = _currentProject.ResponsibleDeveloper.Patronymic;
            }
        }

        // Обработчик сохранения проекта
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация обязательных полей
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Введите название проекта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка ФИО разработчика
                if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                    string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                    string.IsNullOrWhiteSpace(txtPatronymic.Text))
                {
                    MessageBox.Show("Введите полное ФИО ответственного разработчика", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка выбранных подразделений
                var selectedDepartments = lbDepartments.ItemsSource
                    .Cast<DepartmentViewModel>()
                    .Where(d => d.IsSelected)
                    .ToList();

                if (selectedDepartments.Count == 0)
                {
                    MessageBox.Show("Выберите хотя бы одно подразделение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // Сохранение данных из формы в объект проекта
                _currentProject.Title = txtTitle.Text;
                _currentProject.StartDevelopmentDate = dpStartDev.SelectedDate ?? DateTime.Now;
                _currentProject.StartProductionDate = dpStartProd.SelectedDate ?? DateTime.Now;
                _currentProject.CurrentVersion = txtVersion.Text;
                _currentProject.Status = (Status)cbStatus.SelectedIndex;
                _currentProject.ReleasesUrl = txtReleasesUrl.Text;
                _currentProject.GitUrl = txtGitUrl.Text;

                // Парсинг числовых полей
                if (int.TryParse(txtWorkplaces.Text, out int workplaces))
                {
                    _currentProject.AmountWorkplaces = workplaces;
                }

                _currentProject.Comment = txtComment.Text;

                // Сохранение данных разработчика
                _currentProject.ResponsibleDeveloper.LastName = txtLastName.Text;
                _currentProject.ResponsibleDeveloper.FirstName = txtFirstName.Text;
                _currentProject.ResponsibleDeveloper.Patronymic = txtPatronymic.Text;

                // Работа с БД
                using (ProjectsContext db = new ProjectsContext())
                {
                    if (db.Projects.Any(p => p.Id != _currentProject.Id && p.Title == txtTitle.Text))
                    {
                        MessageBox.Show("Проект с таким названием уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Проверка на уникальность разработчика
                    var existingDeveloper = db.Developers.FirstOrDefault(d =>
                d.LastName == txtLastName.Text &&
                d.FirstName == txtFirstName.Text &&
                d.Patronymic == txtPatronymic.Text);

                    if (existingDeveloper != null)
                    {
                        // Используем существующего разработчика
                        _currentProject.ResponsibleDeveloper = existingDeveloper;
                    }
                    else
                    {
                        // Создаем нового разработчика
                        _currentProject.ResponsibleDeveloper = new Developer
                        {
                            LastName = txtLastName.Text,
                            FirstName = txtFirstName.Text,
                            Patronymic = txtPatronymic.Text
                        };
                        db.Developers.Add(_currentProject.ResponsibleDeveloper);
                    }

                    if (_currentProject.Id == 0)
                    {
                        db.Projects.Add(_currentProject);
                    }
                    else
                    {
                        var existingProject = (from p in db.Projects.Include("Departments")
                                               where p.Id == _currentProject.Id
                                               select p).FirstOrDefault();

                        if (existingProject != null)
                        {
                            db.Entry(existingProject).CurrentValues.SetValues(_currentProject);
                            existingProject.Departments.Clear();
                            foreach (var department in _currentProject.Departments)
                            {
                                var existingDepartment = db.Departments.Find(department.Id);
                                if (existingDepartment != null)
                                {
                                    existingProject.Departments.Add(existingDepartment);
                                }
                            }
                        }
                    }
                    db.SaveChanges();
                }
                MessageBox.Show("Данные успешно сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик отмены редактирования
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Обработчик добавления нового подразделения
        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            // Открытие диалога для ввода названия подразделения
            var inputDialog = new InputDialog("Добавить подразделение", "Введите название подразделения:");
            if (inputDialog.ShowDialog() == true)
            {
                // Создание и сохранение нового подразделения
                var newDepartment = new Department { Title = inputDialog.Answer };

                using (ProjectsContext db = new ProjectsContext())
                {
                    db.Departments.Add(newDepartment);
                    db.SaveChanges();
                }

                // Обновление списка подразделений
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

    // Модель представления для отображения подразделений с флажками
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }
    }
}
