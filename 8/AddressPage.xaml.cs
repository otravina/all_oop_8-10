using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
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

namespace _8
{
    /// <summary>
    /// Логика взаимодействия для AddressPage.xaml
    /// </summary>
    public partial class AddressPage : Page
    {
        Book _book;

        public AddressPage(int id, Book book, string AFIO)
        {
            InitializeComponent();
            
            tbAuthorID.Text = id.ToString();
            tbAFIO.Text = AFIO;
            _book = book;            
        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbCity.Text))
            {
                MessageBox.Show("Введите City", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!Regex.IsMatch(tbCity.Text, @"^[\p{L}-]+$"))
            {
                MessageBox.Show("Город может содержать только буквы и дефис", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(tbAFIO.Text))
            {
                MessageBox.Show("Введите FIO", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Author author = new Author
            {
                AuthorID = int.Parse(tbAuthorID.Text),
                City = tbCity.Text,
                Country = tbCountry.Text,
                FIO = tbAFIO.Text
            };

            var validationContext = new ValidationContext(_book);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (!Validator.TryValidateObject(_book, validationContext, results, true))
            {
                MessageBox.Show(results.First().ErrorMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _book.Author = author;
            _book.UpdatedAt = DateTime.Now;

            AppData.db.Book.AddOrUpdate(_book);
            AppData.db.SaveChanges();
            MessageBox.Show("Студент был добавлен","Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            Save_btn.IsEnabled = false;
        }
    }
}