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
    /// Interaction logic for LogSelledList.xaml
    /// </summary>
    public partial class LogSelledList : Page
    {
        List<Bill> Bill = new List<Bill>();
        List<BillDetail> BillDetail = new List<BillDetail>();
        List<BookModel> BookList = new List<BookModel>();
        List<LogShow> Logbill = new List<LogShow>();

        public LogSelledList()
        {
            InitializeComponent();
            Loaded += Loader;
        }
        private void Loader(object sender, RoutedEventArgs e)
        {
            Bill = DataAccess.GetBill();
            foreach (var bill in Bill)
            {
                var log = new LogShow();
                log.NumberBill = bill.NumberBill;
                log.CustomerName = DataAccess.SearchCustomersID(bill.CustomerID);
                log.SellerUser = DataAccess.SearchUserID(bill.User);
                log.TimeSold = bill.TimeSold;
                log.TotalPrice = bill.TotalPrice;
                Logbill.Add(log);
            }
            dataGrid1.ItemsSource = Logbill;
            
        }
        private void Refresh()
        {
            dataGrid1.ItemsSource = null;
            dataGrid2.ItemsSource = null;
            Logbill.Clear();
            Loader(null,null);
        }
        

        private void backBtm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectPerson = dataGrid1.SelectedCells[0].Item as LogShow;
            if (selectPerson == null)
            {
                return;
            }
            BillDetail = DataAccess.GetBillDetaill(selectPerson.NumberBill);
            BookList = new List<BookModel>();
            foreach (var bill in BillDetail)
            {
                var book = DataAccess.SearchBooks(bill.ISBN)[0];
                book.Quantity = bill.Quantity;
                BookList.Add(book);
            }            
            dataGrid2.ItemsSource = null;
            dataGrid2.ItemsSource = BookList;
           
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
