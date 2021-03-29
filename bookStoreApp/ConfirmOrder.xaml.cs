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
    /// Interaction logic for ConfrimOrder.xaml
    /// </summary>
    public partial class ConfirmOrder : Page
    {
        List<CustomerModel> people = new List<CustomerModel>();
        List<GetdataTransactions> BookList = new List<GetdataTransactions>();
        Bill Order = new Bill();
        List<BillDetail> bill = new List<BillDetail>();
        List<UserModel> UserList = new List<UserModel>();
        int useridsent;
        public ConfirmOrder()
        {
            InitializeComponent();
            Loaded += Loader;
            phonetxtBox.Focus();


        }
        decimal Totalprice;
        public ConfirmOrder(List<GetdataTransactions> bookList, string totalPrice, string Dot , string totalQuantity ,string userid,decimal totalprice)
        {
            InitializeComponent();
            Loaded += Loader;
            phonetxtBox.Focus();
            dataGrid2.ItemsSource = null;
            BookList = bookList;
            dataGrid2.ItemsSource = BookList;
            totalPricetxtBox.Text = totalPrice;
            DottxtBox.Text = Dot;
            totalQuantitytxtBox.Text = totalQuantity;
            useridsent = DataAccess.GetUser(userid).Number;
            Totalprice = totalprice;
        }
        private void Loader(object sender, RoutedEventArgs e)
        {
            people = DataAccess.GetDataUser();
            dataGrid1.ItemsSource = people;
        }
        private void LoadCustomerList()
        {
            people = DataAccess.GetDataUser();
        }
        private void ClearAll()
        {
            totalQuantitytxtBox.Text = "";
            totalPricetxtBox.Text = "";
            phonetxtBox.Text = "";
            DottxtBox.Text = "";
            CustomersIDtxtBox.Text = "";
            firstNametxtBox.Text = "";
            lastNametxtBox.Text = "";
            BookList.Clear();
            people.Clear();
            dataGrid1.ItemsSource = null;
            dataGrid2.ItemsSource = null;
        }
        private void Clearpeople()
        {
            dataGrid1.ItemsSource = null;

            phonetxtBox.Text = "";
            CustomersIDtxtBox.Text = "";
            firstNametxtBox.Text = "";
            lastNametxtBox.Text = "";
        }
        public void refreshData()
        {
            dataGrid1.ItemsSource = null;
            LoadCustomerList();
            dataGrid1.ItemsSource = people;
        }

        private void SerachBtn_Click(object sender, RoutedEventArgs e)
        {
            if (phonetxtBox.Text == "") { refreshData(); return; }
            if (NumberOnly(phonetxtBox.Text) == false)
            {
                MessageBox.Show("please put the number only");
                return;
            }
            people = DataAccess.SearchCustomers(phonetxtBox.Text);
            dataGrid1.ItemsSource = null;
            dataGrid1.ItemsSource = people;
            if (people.Count == 1)
            {
                dataGrid1.SelectAll();
                SellBtn.Focus();
            }
        } 
        private void CustomersBtm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CustomersManager());
        }

        private void backBtm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectPerson = dataGrid1.SelectedCells[0].Item as CustomerModel;
            if (selectPerson == null)
            {
                return;
            }
            CustomersIDtxtBox.Text = selectPerson.CustomerId.ToString();
            var name = selectPerson.Name.Split(' ');
            if (name.Length > 1)
            {
                firstNametxtBox.Text = name[0];
                lastNametxtBox.Text = name[1];
            }
            else if (name.Length == 1)
            {
                firstNametxtBox.Text = name[0];
                lastNametxtBox.Text = "";
            }
            phonetxtBox.Text = selectPerson.Phone;
            Order.CustomerID = selectPerson.CustomerId;
        }
        private void SellBtn_Click(object sender, RoutedEventArgs e)
        {
            string textshow = "";
            int count = 1;
            decimal totalprice = Transactions.CalculateTotalBookPrice(BookList);
            if (CustomersIDtxtBox.Text == "" ) { Order.CustomerID = 0; }
            else if (CustomersIDtxtBox.Text != "") { Order.CustomerID = int.Parse(CustomersIDtxtBox.Text); }
            Order.TimeSold = DateTime.Now;
            Order.User = useridsent;
            Order.TotalPrice = totalprice;

            foreach (var order in BookList)
            {
                textshow += $"{count}.{order.TitleBook}  Quantity :{order.QuantitySold} Price :{order.Price * order.QuantitySold}\r \n";
                count++;
            }
            var Answer = MessageBox.Show($"{textshow} \r\n Total Price :{totalprice} Baht", "Are you sure to sell this order?", MessageBoxButton.YesNo);
            if (Answer == MessageBoxResult.Yes)
            {
                DataAccess.SellBook(BookList);
                int billNumber = DataAccess.AddDataTransactions(Order);
                bill = new List<BillDetail>();
                foreach (var ISBNall in BookList)
                {
                    var detail = new BillDetail();
                    detail.NumberBill = billNumber;
                    detail.ISBN = ISBNall.ISBN;
                    detail.Quantity = ISBNall.QuantitySold;
                    bill.Add(detail);
                }
                DataAccess.AddBill(billNumber, bill);
                ClearAll();
                NavigationService.Navigate(new Transactions());
            }
        }

        private void phonetxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { SerachBtn_Click(null, null); }
        }

        private void customerBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CustomersManager());
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            Clearpeople();
            refreshData();
        }
        public static bool NumberOnly(string word)
        {
            foreach (var c in word)
            {
                if (char.IsDigit(c) == true)
                {
                    continue;
                }
                else if (char.IsDigit(c) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
