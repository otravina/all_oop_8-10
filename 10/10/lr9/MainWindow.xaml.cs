using lr9.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace lr9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IUnitOfWork _unitOfWork;

        private ObservableCollection<Course> courses = new ObservableCollection<Course>();

        Student student = new Student();

        public MainWindow(IUnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            DisableAllFields();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void View_btn_Click(object sender, RoutedEventArgs e)
        {
            SaveEdit.Visibility = Visibility.Collapsed;
            SaveAdd.Visibility = Visibility.Collapsed;
            Search.Visibility = Visibility.Collapsed;

            lastNameTextBlock.IsReadOnly = true;
            firstNameTextBlock.IsReadOnly = true;
            ageTextBlock.IsReadOnly = true;

            Student selectedStudent = studentsDataGrid.SelectedItem as Student;
            List<CheckBox> coursesCheckBoxes = new List<CheckBox>();

            foreach (var checkBox in infoGrid.Children.OfType<CheckBox>())
            {
                coursesCheckBoxes.Add(checkBox);
                checkBox.IsEnabled = false;
            }

            if (selectedStudent != null)
            {
                firstNameTextBlock.Text = selectedStudent.FirstName;
                lastNameTextBlock.Text = selectedStudent.LastName;
                ageTextBlock.Text = selectedStudent.Age.ToString();

                var selectedCourses = _unitOfWork.Course.Find(c => c.StudentId == selectedStudent.Id).ToList();

                if (selectedCourses != null)
                {
                    foreach (var checkBox in infoGrid.Children.OfType<CheckBox>())
                    {
                        string courseName = checkBox.Content.ToString();

                        bool isCourseSelected = selectedCourses.Any(c => c.Name == courseName);

                        checkBox.IsChecked = isCourseSelected;
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            Student selectedStudent = studentsDataGrid.SelectedItem as Student;

            if (selectedStudent != null)
            {
                var selectedCourses = _unitOfWork.Course.Find(c => c.StudentId == selectedStudent.Id).ToList();
                
                _unitOfWork.Course.RemoveRange(selectedCourses);
                _unitOfWork.Student.Remove(selectedStudent);

                _unitOfWork.Complete();

                RefreshData();
            }
            else
            {
                MessageBox.Show("Выберите студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {
            SaveEdit.Visibility = Visibility.Visible;
            SaveAdd.Visibility = Visibility.Collapsed;
            Search.Visibility = Visibility.Collapsed;

            lastNameTextBlock.IsReadOnly = false;
            firstNameTextBlock.IsReadOnly = false;
            ageTextBlock.IsReadOnly = false;

            Student selectedStudent = studentsDataGrid.SelectedItem as Student;
            List<CheckBox> coursesCheckBoxes = new List<CheckBox>();

            foreach (var checkBox in infoGrid.Children.OfType<CheckBox>())
            {
                coursesCheckBoxes.Add(checkBox);
                checkBox.IsEnabled = true;
            }

            if (selectedStudent != null)
            {
                firstNameTextBlock.Text = selectedStudent.FirstName;
                lastNameTextBlock.Text = selectedStudent.LastName;
                ageTextBlock.Text = selectedStudent.Age.ToString();

                var selectedCourses = _unitOfWork.Course.Find(c => c.StudentId == selectedStudent.Id).ToList();

                if (selectedCourses != null)
                {
                    foreach (var checkBox in infoGrid.Children.OfType<CheckBox>())
                    {
                        string courseName = checkBox.Content.ToString();

                        bool isCourseSelected = selectedCourses.Any(c => c.Name == courseName);

                        checkBox.IsChecked = isCourseSelected;
                    }
                }
            }
        }

        private void Save_Edit_btn_Click(object sender, RoutedEventArgs e)
        {

            Student selectedStudent = studentsDataGrid.SelectedItem as Student;

            if (string.IsNullOrEmpty(firstNameTextBlock.Text))
            {
                MessageBox.Show("Поле 'Имя' пустое. Введите имя студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(firstNameTextBlock.Text, @"\d"))
            {
                MessageBox.Show("Поле 'Имя' должно содеражать только буквы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                selectedStudent.FirstName = firstNameTextBlock.Text;

            }
            if (string.IsNullOrEmpty(lastNameTextBlock.Text))
            {
                MessageBox.Show("Поле 'Фамилия' пустое. Введите фамилию студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(lastNameTextBlock.Text, @"\d"))
            {
                MessageBox.Show("Поле 'Фамилия' должно содеражать только буквы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                selectedStudent.LastName = lastNameTextBlock.Text;
            }

            if (string.IsNullOrEmpty(ageTextBlock.Text))
            {
                MessageBox.Show("Поле 'Возраст' пустое. Введите возраст студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(ageTextBlock.Text, @"\D"))
            {
                MessageBox.Show("Поле 'Возраст' должно содеражать только цифры!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (int.Parse(ageTextBlock.Text) < 18 || int.Parse(ageTextBlock.Text) > 100)
            {
                MessageBox.Show("Введите возраст от 18 до 100!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                selectedStudent.Age = int.Parse(ageTextBlock.Text);
            }

            var selectedCourses = _unitOfWork.Course.Find(c => c.StudentId == selectedStudent.Id).ToList();

            _unitOfWork.Course.RemoveRange(selectedCourses);

            foreach (var checkBox in infoGrid.Children.OfType<CheckBox>())
            {
                if (checkBox.IsChecked == true)
                {
                    string courseName = checkBox.Content.ToString();
                    Course newCourse = new Course { Name = courseName, StudentId = selectedStudent.Id };
                    _unitOfWork.Course.Add(newCourse);
                }
            }

            bool isAtLeastOneSelected = false;

            foreach (var checkBox in infoGrid.Children.OfType<CheckBox>())
            {
                if (checkBox.IsChecked == true)
                {
                    isAtLeastOneSelected = true;
                    break;
                }
            }

            if (!isAtLeastOneSelected)
            {
                MessageBox.Show("Выберите хотя бы одну посещаемую дисциплину для студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _unitOfWork.Student.Update(selectedStudent);
            _unitOfWork.Complete();

            MessageBox.Show("Данные студента обновлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshData();
            ClearAllFields();
            DisableAllFields();
            SaveEdit.Visibility = Visibility.Collapsed;
        }

        private void Save_Add_btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(firstNameTextBlock.Text))
            {
                MessageBox.Show("Поле 'Имя' пустое. Введите имя студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(firstNameTextBlock.Text, @"\d"))
            {
                MessageBox.Show("Поле 'Имя' должно содеражать только буквы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                student.FirstName = firstNameTextBlock.Text;
            }

            if (string.IsNullOrEmpty(lastNameTextBlock.Text))
            {
                MessageBox.Show("Поле 'Фамилия' пустое. Введите фамилию студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(lastNameTextBlock.Text, @"\d"))
            {
                MessageBox.Show("Поле 'Фамилия' должно содеражать только буквы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                student.LastName = lastNameTextBlock.Text;
            }

            if (string.IsNullOrEmpty(ageTextBlock.Text))
            {
                MessageBox.Show("Поле 'Возраст' пустое. Введите возраст студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(ageTextBlock.Text, @"\D"))
            {
                MessageBox.Show("Поле 'Возраст' должно содеражать только цифры!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (int.Parse(ageTextBlock.Text) < 18 || int.Parse(ageTextBlock.Text) > 100)
            {
                MessageBox.Show("Введите возраст от 18 до 100!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                student.Age = int.Parse(ageTextBlock.Text);
            }

            if (Математика.IsChecked.Value
                || Английский.IsChecked.Value
                || Физика.IsChecked.Value
                || Химия.IsChecked.Value
                || Инженерия.IsChecked.Value
                || Биологоия.IsChecked.Value
                || Менеджмент.IsChecked.Value
                || История.IsChecked.Value
                || Русский_язык.IsChecked.Value)
            {
                foreach (var checkBox in infoGrid.Children.OfType<CheckBox>())
                {
                    string courseName = checkBox.Content.ToString();

                    if (checkBox.IsChecked == true)
                    {
                        var course = new Course
                        {
                            Name = courseName,
                            StudentId = student.Id
                        };
                        courses.Add(course);
                    }
                }

                student.Courses = courses.ToList();

                _unitOfWork.Student.Add(student);
                _unitOfWork.Complete();


                MessageBox.Show("Студент добавлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshData();
                ClearAllFields();
                DisableAllFields();
                SaveAdd.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Выберите хотя бы одну посещаемую дисциплину для студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            EnableAllFields();
            SaveAdd.Visibility = Visibility.Visible;
            Search.Visibility = Visibility.Collapsed;
        }

        private void RefreshData()
        {
            studentsDataGrid.ItemsSource = GetStudents();

            student = new Student();
            courses = new ObservableCollection<Course>();
        }

        private void GoBack_btn_Click(object sender, RoutedEventArgs e)
        {
            SaveEdit.Visibility = Visibility.Collapsed;
            Search.Visibility = Visibility.Collapsed;

            firstNameTextBlock.Text = null;
            lastNameTextBlock.Text = null;
            ageTextBlock.Text = null;

            foreach (var checkBox in infoGrid.Children.OfType<CheckBox>())
            {
                checkBox.IsChecked = false;
                checkBox.IsEnabled = true;
            }

            firstNameTextBlock.IsReadOnly = false;
            lastNameTextBlock.IsReadOnly = false;
            ageTextBlock.IsReadOnly = false;

            student = new Student();
            courses = new ObservableCollection<Course>();

            RefreshData();
            DisableAllFields();
        }

        private void Sort_lastname_asc_Click(object sender, RoutedEventArgs e)
        {
            var sortedStudents = _unitOfWork.Student.GetAll().OrderBy(s => s.LastName).ToList();
            studentsDataGrid.ItemsSource = sortedStudents;

        }
        private void Sort_lastname_desc_Click(object sender, RoutedEventArgs e)
        {
            var sortedStudents = _unitOfWork.Student.GetAll().OrderByDescending(s => s.LastName).ToList();
            studentsDataGrid.ItemsSource = sortedStudents;
        }
        private void Sort_age_asc_Click(object sender, RoutedEventArgs e)
        {
            var sortedStudents = _unitOfWork.Student.GetAll().OrderBy(s => s.Age).ToList();
            studentsDataGrid.ItemsSource = sortedStudents;
        }
        private void Sort_age_desc_Click(object sender, RoutedEventArgs e)
        {
            var sortedStudents = _unitOfWork.Student.GetAll().OrderByDescending(s => s.Age).ToList();
            studentsDataGrid.ItemsSource = sortedStudents;
        }

        private void menu_search_Click(object sender, RoutedEventArgs e)
        {
            EnableAllFields();
            Search.Visibility = Visibility.Visible;

            Математика.IsEnabled = false;
            Физика.IsEnabled = false;
            Английский.IsEnabled = false;
            Биологоия.IsEnabled = false;
            Инженерия.IsEnabled = false;
            Химия.IsEnabled = false;
            История.IsEnabled = false;
            Менеджмент.IsEnabled = false;
            Русский_язык.IsEnabled = false;
        }

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ageTextBlock.Text) && string.IsNullOrEmpty(firstNameTextBlock.Text) && string.IsNullOrEmpty(lastNameTextBlock.Text))
            {
                MessageBox.Show("Введите хотя бы один параметр для поиска (имя, фамилия, возраст)!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                string searchAge = null;

                if (!string.IsNullOrEmpty(ageTextBlock.Text))
                {
                    if (Regex.IsMatch(ageTextBlock.Text, @"\D"))
                    {
                        MessageBox.Show("Поле 'Возраст' должно содеражать только цифры!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        searchAge = ageTextBlock.Text;
                    }

                }
                string searchName = firstNameTextBlock.Text;
                string searchLastName = lastNameTextBlock.Text;

                var searchResults = _unitOfWork.Student.Find(s => (string.IsNullOrEmpty(searchAge) || s.Age == int.Parse(searchAge))
                    && (string.IsNullOrEmpty(searchName) || s.FirstName.Contains(searchName))
                    && (string.IsNullOrEmpty(searchLastName) || s.LastName.Contains(searchLastName)))
                    .ToList();

                studentsDataGrid.ItemsSource = searchResults;
            }
        }

        private List<Student> GetStudents()
        {
            return _unitOfWork.Student.GetAll().ToList();
        }

        private void DisableAllFields()
        {
            firstNameTextBlock.IsReadOnly = true;
            lastNameTextBlock.IsReadOnly = true;
            ageTextBlock.IsReadOnly = true;
            Математика.IsEnabled = false;
            Физика.IsEnabled = false;
            Английский.IsEnabled = false;
            Биологоия.IsEnabled = false;
            Инженерия.IsEnabled = false;
            Химия.IsEnabled = false;
            История.IsEnabled = false;
            Менеджмент.IsEnabled = false;
            Русский_язык.IsEnabled = false;
        }

        private void EnableAllFields()
        {
            firstNameTextBlock.IsReadOnly = false;
            lastNameTextBlock.IsReadOnly = false;
            ageTextBlock.IsReadOnly = false;
            Математика.IsEnabled = true;
            Физика.IsEnabled = true;
            Английский.IsEnabled = true;
            Биологоия.IsEnabled = true;
            Инженерия.IsEnabled = true;
            Химия.IsEnabled = true;
            История.IsEnabled = true;
            Менеджмент.IsEnabled = true;
            Русский_язык.IsEnabled = true;
        }

        private void ClearAllFields()
        {
            firstNameTextBlock.Text = string.Empty;
            lastNameTextBlock.Text = string.Empty;
            ageTextBlock.Text = string.Empty;
            Математика.IsChecked = false;
            Физика.IsChecked = false;
            Английский.IsChecked = false;
            Биологоия.IsChecked = false;
            Инженерия.IsChecked = false;
            Химия.IsChecked = false;
            История.IsChecked = false;
            Менеджмент.IsChecked = false;
            Русский_язык.IsChecked = false;
        }
    }
}