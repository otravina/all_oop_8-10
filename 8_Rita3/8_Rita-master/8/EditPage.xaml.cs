using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Globalization;
using System.Data.Entity.Migrations;

namespace _8
{
    /// <summary>
    /// Логика взаимодействия для EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        Book book = new Book();

        string img;
        public EditPage(Book book, bool statement)
        {
            InitializeComponent();

            img = book.BookCover;

            if (statement == false)
            {
                tbUDC.IsReadOnly = true;
                tbTitle.IsReadOnly = true;
                tbPageCount.IsEnabled = false;
                cbFormat.IsEnabled = false;
                cbPublisher.IsEnabled = false;
                tbFileSize.IsReadOnly = true;
                tbPublishYear.IsReadOnly = true;
                dpUploadDate.IsEnabled = false;
                tbAuthorID.IsReadOnly = true;
                tbCity.IsReadOnly = true;
                tbAFIO.IsReadOnly = true;
                tbListOfAuthors.IsReadOnly = true;
                Save_btn.IsEnabled = false;
                LoadImage_btn.IsEnabled = false;              
            }

            tbUDC.Text = book.UDC.ToString();
            tbTitle.Text = book.Title;
            tbPageCount.Text = book.PageCount.ToString();
            cbFormat.Text = book.Format;
            cbPublisher.Text = book.Publisher;
            tbPublishYear.Text = book.PublishYear.ToString();
            tbListOfAuthors.Text = book.Author.FIO.ToString();
            tbFileSize.Text = book.FileSize.ToString().Replace(',', '.');
            dpUploadDate.SelectedDate = book.UploadDate;

            tbAuthorID.Text = book.Author.AuthorID.ToString();
            tbCity.Text = book.Author.City;
            tbCountry.Text = book.Author.Country;
            tbAFIO.Text = book.Author.FIO;

            tbAuthorID.Text = tbUDC.Text;

            if (!string.IsNullOrEmpty(book.BookCover))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(book.BookCover);
                bitmap.EndInit();

                image.Source = bitmap;
            }
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

        private void Save_btn_Click(object sender, RoutedEventArgs e)
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
                tbAuthorID.Text = tbUDC.Text;
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
                MessageBox.Show("Поле 'Group' пустое. Выберите Group", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (int.Parse(tbPublishYear.Text) >2024 || int.Parse(tbPublishYear.Text) < 1500)
            {
                MessageBox.Show("Введите PublishYear от 1500 до 2024", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Поле 'FileSize' должно содеражать только цифры и точку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if(decimal.TryParse(tbFileSize.Text, NumberStyles.Number, culture, out fileSize))
                book.FileSize = fileSize;
            }

            string bDay = dpUploadDate.SelectedDate.ToString();
            DateTime currentDateTime = DateTime.Now;
            if (DateTime.Parse(bDay) > currentDateTime)
            {
                MessageBox.Show("Дата рождения не может быть в будущем!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }
           
            if (!string.IsNullOrEmpty(book.BookCover))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(book.BookCover);
                bitmap.EndInit();

                image.Source = bitmap;
                book.BookCover = bitmap.UriSource.ToString();
            }
            else
            {
                book.BookCover = img;
            }

            if (image.Source == null)
            {
                MessageBox.Show("Добавьте фото студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(tbCity.Text))
            {
                MessageBox.Show("Введите City", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!Regex.IsMatch(tbCity.Text, @"^[\p{L}-]+$"))
            {
                MessageBox.Show("Город может содержать только буквы и дефис", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Author author = new Author
            {
                AuthorID = int.Parse(tbUDC.Text),
                City = tbCity.Text,
                Country = tbCountry.Text,
                FIO = tbAFIO.Text,
            };

            book.Author = author;

            var validationContext = new ValidationContext(book);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (!Validator.TryValidateObject(book, validationContext, results, true))
            {
                MessageBox.Show(results.First().ErrorMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string connectionString = "Data Source=DESKTOP-KS0KMCI\\SQLEXPRESS;Initial Catalog=Library;Integrated Security=True;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UpdateBook", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UDC", tbUDC.Text);

                    command.Parameters.AddWithValue("@Title", tbTitle.Text);
                    command.Parameters.AddWithValue("@BookCover", book.BookCover);
                    command.Parameters.AddWithValue("@PublishYear", tbPublishYear.Text);
                    command.Parameters.AddWithValue("@Publisher", cbPublisher.SelectedItem);
                    command.Parameters.AddWithValue("@UploadDate", dpUploadDate.SelectedDate);
                    command.Parameters.AddWithValue("@PageCount", tbPageCount.Text);
                    command.Parameters.AddWithValue("@Format", cbFormat.SelectedItem);
                    command.Parameters.AddWithValue("@FileSize", tbFileSize.Text);
                    command.Parameters.AddWithValue("@ListOfAuthors", tbAFIO.Text);

                    command.Parameters.AddWithValue("@City", tbCity.Text);
                    command.Parameters.AddWithValue("@Country", tbCountry.Text);
                    command.Parameters.AddWithValue("@FIO", tbAFIO.Text);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Информация о студенте была изменена", "Уведомление",
                MessageBoxButton.OK, MessageBoxImage.Information);           
        }
    }
}
