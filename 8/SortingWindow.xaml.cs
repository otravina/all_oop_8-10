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

namespace _8
{
    /// <summary>
    /// Логика взаимодействия для SortingWindow.xaml
    /// </summary>
    public partial class SortingWindow : Window
    {
        public SortingWindow()
        {
            InitializeComponent();
        }

        public bool PublishYearAscending { get; set; }
        public bool PublishYearDescending { get; set; }
        public bool FileSizeAscending { get; set; }
        public bool FileSizeDescending { get; set; }
        public bool PageCountAscending { get; set; }
        public bool PageCountDescending { get; set; }
        public bool UDCAscending { get; set; }
        public bool UDCDescending { get; set; }

        private void Sorting_btn_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)rbAsSize.IsChecked)
            {
                FileSizeAscending = true;
            }
            else if ((bool)rbDeSize.IsChecked)
            {
                FileSizeDescending = true;
            }

            if ((bool)rbAsYear.IsChecked)
            {
                PublishYearAscending = true;
            }
            else if ((bool)rbDeYear.IsChecked)
            {
                PublishYearDescending = true;
            }

            if ((bool)rbAsCount.IsChecked)
            {
                PageCountAscending = true;
            }
            else if((bool)rbDeCount.IsChecked)
            {
                PageCountDescending = true;
            }

            if ((bool)rbAsUDC.IsChecked)
            {
                UDCAscending = true;
            }
            else if ((bool)rbDeUDC.IsChecked)
            {
                UDCDescending = true;
            }
            Close();
        }
    }
}
