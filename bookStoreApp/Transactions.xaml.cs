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
        string useridsent = "";
        decimal TotalPrice = 0m;
        List<GetdataTransactions> BookList = new List<GetdataTransactions>();
        List<BookModel> AllBooks = new List<BookModel>();
        public Transactions()
        {
            InitializeComponent();
            iSBNtxtBox.Focus();
            AllBooks = DataAccess.GetBookData();
            iSBNtxtBox.ItemsSource = AllBooks;
        }
        public Transactions(string userid)
        {
            InitializeComponent();
            iSBNtxtBox.Focus();
            useridsent = userid;
        }
        private void backBtm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        /// <summary
        /// คำนวณค่าหนังสือทั้งหมดจากรายการหนังสือที่อยู่ใน BookList
        /// </summary>
        public static decimal CalculateTotalBookPrice(List<GetdataTransactions> bookList)
		{
            decimal totalcost = 0m; //สร้างค่าเก็บไว้เพื่อคำนวณและรอส่งกลับ
            foreach (var totalQuantity in bookList) //วนซ้ำรายการ "BookList" ทีละอัน โดยระหว่างวน เรียกหนังสือทีละเล่มในรายการว่า "totalQuantity"
            {
                //คำนวณค่าหนังสือทีละเล่ม ด้วยการเอาค่า total บวกเข้ากับค่า หนังสือปัจจุบัน คูณด้วยค่าหนังสือปัจจุบัน
                totalcost = totalcost + (totalQuantity.Price * totalQuantity.QuantitySold);
            }
            return totalcost; //ส่งค่าที่คำนวณเสร็จแล้วกลับไป
        }
        public static int CalculateTotalBookCount(List<GetdataTransactions> bookList)
        {
            int bookcount = 0;
            foreach (var totalQuantity in bookList) 
            {
                bookcount = bookcount + totalQuantity.QuantitySold;
            }
            return bookcount;
        }
        private void Clearinput()
        {
            quantitytxtBox.Text = "1";
            iSBNtxtBox.Text = "";
        }
        private void ClearBookList()
        {
            totalQuantitytxtBox.Text = "";
            totalPricetxtBox.Text = "";
            DottxtBox.Text = "";
            BookList.Clear();
            dataGrid.ItemsSource = null;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (iSBNtxtBox.Text == "") { return; }
            if (CommonMethod.NumberOnly(quantitytxtBox.Text) == false) { return; }
            var databook = DataAccess.GetDataBook(iSBNtxtBox.Text, int.Parse(quantitytxtBox.Text));
            if (databook.Count == 0)
            {
                return;
            }
            else if (databook.Count > 0)  //Count คือการนับจำนวนวัสดุที่อยู่ใน List ว่ามีกี่ตัว
            {
                foreach (var book in databook)
                {
                    bool found = false;

                    foreach (var book2 in BookList)
                    {
                        if (book.ISBN == book2.ISBN)
                        {
                            book2.QuantitySold += book.QuantitySold;
                            found = true;
                        }
                    }
                    if (found == false) 
                    {
                        BookList.Add(book);
                    }
                }
            }
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = BookList;
            int bookcount = CalculateTotalBookCount(BookList);
            TotalPrice = CalculateTotalBookPrice(BookList);
            totalQuantitytxtBox.Text = bookcount.ToString();
            string stringtotalPrice = TotalPrice.ToString();
            var cha = stringtotalPrice.Split('.');
            if (cha.Length > 1)
            {
                totalPricetxtBox.Text = cha[0];
                DottxtBox.Text = $".{cha[1]}";
            }
            else if (cha.Length == 1)
            {
                totalPricetxtBox.Text = cha[0];
                DottxtBox.Text = "";
            }
            Clearinput();
        }

        private void SellBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BookList.Count < 1)
            {
                return;
            }
            NavigationService.Navigate(new ConfirmOrder(BookList, totalPricetxtBox.Text, DottxtBox.Text, totalQuantitytxtBox.Text, useridsent, TotalPrice));
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells == null || dataGrid.SelectedCells.Count < 1) { return; }
            var book = dataGrid.SelectedCells[0].Item as GetdataTransactions;
            if (book == null || dataGrid.SelectedCells == null) 
            {
                return; 
            }
            else if (book != null)
            {
                var Result = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButton.YesNo);
                if (Result == MessageBoxResult.Yes)
                {

                    BookList.Remove(book);
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = BookList;
                    int bookcount = CalculateTotalBookCount(BookList);
                    TotalPrice = CalculateTotalBookPrice(BookList);
                    totalQuantitytxtBox.Text = bookcount.ToString();
                    string stringtotalPrice = TotalPrice.ToString();
                    var cha = stringtotalPrice.Split('.');
                    if (cha.Length > 1)
                    {
                        totalPricetxtBox.Text = cha[0];
                        DottxtBox.Text = $".{cha[1]}";
                    }
                    else if (cha.Length == 1)
                    {
                        totalPricetxtBox.Text = cha[0];
                        DottxtBox.Text = "";
                    }
                    return;
                }
                else if (Result == MessageBoxResult.No)
                {
                    return;
                }
            }
            
            

        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearBookList();
        }

        private void iSBNtxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { AddBtn_Click(null, null); }
        }
    }
}