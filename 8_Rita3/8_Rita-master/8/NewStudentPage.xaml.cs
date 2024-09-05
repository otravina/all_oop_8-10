using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Imaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Globalization;

namespace _8
{
    /// <summary>
    /// Логика взаимодействия для NewStudentPage.xaml
    /// </summary>
    public partial class NewStudentPage : Page
    {
        public NewStudentPage()
        {
            InitializeComponent();
        }

        Book book = new Book();
        string AFIO;

        private void To_Address_btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbUDC.Text))
            {
                MessageBox.Show("Поле 'ID' не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(tbUDC.Text, @"\D"))
            {
                MessageBox.Show("Поле 'ID' должно содеражать только цифры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                book.UDC = int.Parse(tbUDC.Text);
            }

            if (string.IsNullOrEmpty(tbTitle.Text))
            {
                MessageBox.Show("Поле 'FIO' пустое. Введите FIO", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(tbTitle.Text, @"\d"))
            {
                MessageBox.Show("Поле 'FIO' должно содеражать только буквы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                book.Title = tbTitle.Text;
            }

            if (string.IsNullOrEmpty(cbPublisher.Text))
            {
                MessageBox.Show("Поле 'Publisher' пустое. Выберите Publisher", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                book.Publisher = cbPublisher.Text;
            }

            if (string.IsNullOrEmpty(tbPageCount.Text))
            {
                MessageBox.Show("Поле 'PageCount' пустое. Выберите PageCount", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                book.PageCount = int.Parse(tbPageCount.Text);
            }

            if (string.IsNullOrEmpty(cbFormat.Text))
            {
                MessageBox.Show("Поле 'Формат' пустое. Выберите Group", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                book.Format = cbFormat.Text;
            }

            if (string.IsNullOrEmpty(tbPublishYear.Text))
            {
                MessageBox.Show("Поле 'PublishYear' пустое.  Введите PublishYear", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Regex.IsMatch(tbPublishYear.Text, @"\D"))
            {
                MessageBox.Show("Поле 'PublishYear' должно содеражать только цифры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } 
            else
            {
                book.PublishYear = int.Parse(tbPublishYear.Text);
            }

            CultureInfo culture = CultureInfo.InvariantCulture;
            decimal fileSize;
            if (string.IsNullOrEmpty(tbFileSize.Text))
            {
                MessageBox.Show("Поле 'FileSize' пустое.  Введите FileSize", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(tbFileSize.Text, @"^[0-9]+(?:\.[0-9]+)?$"))
            {
                MessageBox.Show("Поле 'FileSize' должно содеражать только цифры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (decimal.TryParse(tbFileSize.Text, NumberStyles.Number, culture, out fileSize))
                    book.FileSize = fileSize;
                else
                {
                    MessageBox.Show("Ошибка при парсинге значения FileSize", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (dpUploadDate.SelectedDate is null)
            {
                MessageBox.Show("Дата публикации не может быть пустой!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); 
                return;
            }
            string bDay = dpUploadDate.SelectedDate.ToString();
            DateTime currentDateTime = DateTime.Now;

            if (DateTime.Parse(bDay) > currentDateTime)
            {
                MessageBox.Show("Дата публикации не может быть в будущем!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); 
                return;
            }
            else
            {
                book.UploadDate = dpUploadDate.SelectedDate;
            }

            if (tbListOfAuthors.Text == null)
            {
                MessageBox.Show("Не может быть пустым автор!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                book.ListOfAuthors = tbListOfAuthors.Text;
                AFIO = tbListOfAuthors.Text;
            }

            if (image.Source == null)
            {
                MessageBox.Show("Добавьте фото книги!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var validationContext = new ValidationContext(book);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (!Validator.TryValidateObject(book, validationContext, results, true))
            {
                MessageBox.Show(results.First().ErrorMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var id = int.Parse(tbUDC.Text);

            NewStudent.NavigationService.Navigate(new AddressPage(id, book, AFIO));
        }

        private void LoadImage_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(imagePath);
                bitmapImage.EndInit();

                image.Source = bitmapImage;
                
                book.BookCover = imagePath;
            }
        }
    }
}
