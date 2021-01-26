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

namespace bookStoreApp
{
    /// <summary>
    /// Interaction logic for SelectMenu.xaml
    /// </summary>
    public partial class SelectMenu : Page
    {
        public SelectMenu()
        {
            InitializeComponent();
        }
        private void CustomersBtm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CustomersManager());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void LogBtm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LogSelledList());
        }

        private void booksManagerBtm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BooksManager());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TransactionsLog());
        }
    }
}
