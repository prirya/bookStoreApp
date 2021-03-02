using System;
using System.Collections;
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
    /// Interaction logic for TransactionsLog.xaml
    /// </summary>
    public partial class Transactions : Page
    {
        List<GetdataTransactions> BookList = new List<GetdataTransactions>();
        public Transactions()
        {
            InitializeComponent();
        }
        private void backBtm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            BookList.AddRange(DataAccess.GetDataBook(iSBNtxtBox.Text, int.Parse(quantitytxtBox.Text)));
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = BookList;
            int bookcount = 0;
            decimal totalprice = 0m;
            foreach (var totalQuantity in BookList)
            {
                bookcount += totalQuantity.QuantitySold;
                totalprice += totalQuantity.Price * totalQuantity.QuantitySold;
            }
            totalQuantitytxtBox.Text = bookcount.ToString();
            string stringtotalPrice = totalprice.ToString();
            var cha = stringtotalPrice.Split('.');
            if (cha.Length > 1)
            {
                totalPricetxtBox.Text = cha[0];
                DottxtBox.Text = cha[1];
            }
            else if (cha.Length == 1)
            {
                totalPricetxtBox.Text = cha[0];
                DottxtBox.Text = "";
            }
        }
    }
}