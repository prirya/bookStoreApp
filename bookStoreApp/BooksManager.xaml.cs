﻿using System;
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
    /// Interaction logic for BooksManager.xaml
    /// </summary>
    public partial class BooksManager : Page
    {
        public BooksManager()
        {
            InitializeComponent();
            Loaded += Loader;
            saveBtn.Visibility = Visibility.Collapsed;
        }
        List<BookModel> book = new List<BookModel>();
        private void Loader(object sender, RoutedEventArgs e)
        {
            book = DataAccess.GetBookData();
            dataGrid.ItemsSource = book;
        }
        public void Clear()
        {
            ISBNtxtBox.Text = "";
            typetxtBox.Text = "";
            pricetxtBox.Text = "";
            price2txtBox.Text = "";
            quantitytxtBox.Text = "";
            titletxtBox.Text = "";
            descriptiontxtBox.Text = "";
        }
        public void RefreshData()
        {
            dataGrid.ItemsSource = null;
            LoadBookList();
            dataGrid.ItemsSource = book;
        }
        private void LoadBookList()
        {
            book = DataAccess.GetBookData();
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedCells == null || dataGrid.SelectedCells.Count < 1)
            {
                addBtn.Visibility = Visibility.Visible;
                saveBtn.Visibility = Visibility.Collapsed;
                return;
            }
            var selectBook = dataGrid.SelectedCells[0].Item as BookModel;
            if (selectBook == null)
            {
                addBtn.Visibility = Visibility.Visible;
                saveBtn.Visibility = Visibility.Collapsed;
                return;
            }
            else if (selectBook != null)
            {
                addBtn.Visibility = Visibility.Collapsed;
                saveBtn.Visibility = Visibility.Visible;
            }
            ISBNtxtBox.Text = selectBook.ISBN;
            typetxtBox.Text = selectBook.Type;
            string priceString = selectBook.Price.ToString();
            var price = priceString.Split('.');
            if (price.Length > 1)
            {
                pricetxtBox.Text = price[0];
                price2txtBox.Text = price[1];
            }
            else if (price.Length == 1)
            {
                pricetxtBox.Text = price[0];
                price2txtBox.Text = "";
            }
            quantitytxtBox.Text = selectBook.Quantity.ToString();
            titletxtBox.Text = selectBook.Title;
            descriptiontxtBox.Text = selectBook.Description;
        }
        private bool CheckData()
        {
            if (ISBNtxtBox.Text == "")
            {
                MessageBox.Show("please enter ISBN");
                return false;
            }
            if (typetxtBox.Text == "")
            {
                MessageBox.Show("please enter type");
                return false;
            }
            if (pricetxtBox.Text == "")
            {
                MessageBox.Show("please enter price");
                return false;
            }
            if (quantitytxtBox.Text == "")
            {
                MessageBox.Show("please enter quantity");
                return false;
            }
            if (titletxtBox.Text == "")
            {
                MessageBox.Show("please enter title");
                return false;
            }
            return true;
        }
        private void backBtm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void HideEditBtm_Click(object sender, RoutedEventArgs e)
        {
            
            if (editColumn.Width.IsStar == true) { showallOff(); HideShowBtm.Content = "Hide Edit"; }
            else if (editColumn.Width.IsStar == false) { showallOn(); HideShowBtm.Content = "Show Edit"; }
        }
        private void showallOn() 
        {
            editColumn.Width = new GridLength(1,GridUnitType.Star);
        }
        private void showallOff()
        {
            editColumn.Width = new GridLength(0, GridUnitType.Pixel);
        }
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData() == false) { return; }
            string type = typetxtBox.Text;
            string isbn = ISBNtxtBox.Text;
            string title = titletxtBox.Text;
            string description = descriptiontxtBox.Text;
            bool decimaltxt = CommonMethod.NumberOnly(pricetxtBox.Text);
            bool decimaltxt2 = CommonMethod.NumberOnly(price2txtBox.Text);
            if (decimaltxt == false || decimaltxt2 == false)
            {
                MessageBox.Show("Number Only in Price or Decimal");
                return;
            }
            string monny = pricetxtBox.Text + "." + price2txtBox.Text;
            decimal price = decimal.Parse(monny);
            bool quantitytxt = CommonMethod.NumberOnly(quantitytxtBox.Text);
            if (quantitytxt == false)
            {
                MessageBox.Show("Put number only in quantity");
                return;
            }
            int quantity = int.Parse(quantitytxtBox.Text);
            DataAccess.AddDataBookTable(isbn, title, type, description, price, quantity);
            Clear();
            RefreshData();
        }
        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells == null || dataGrid.SelectedCells.Count < 1) { return; }
            var book = dataGrid.SelectedCells[0].Item as BookModel;
            var Result = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButton.YesNo);
            if (Result == MessageBoxResult.Yes)
            {
                DataAccess.RemoveBookTable(book);
                Clear();
                RefreshData();
                return;
            }
            else if (Result == MessageBoxResult.No)
            {
                RefreshData();
            }
        }
        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            RefreshData();
        }
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData() == false) { return; }
            if (dataGrid.SelectedCells.Count >= 1)
            {
                var book = dataGrid.SelectedCells[0].Item as BookModel;
                if (book != null)
                {
                    book.ISBN = ISBNtxtBox.Text;
                    book.Title = titletxtBox.Text;
                    book.Type = typetxtBox.Text;
                    book.Description = descriptiontxtBox.Text;
                    bool decimaltxt = CommonMethod.NumberOnly(pricetxtBox.Text);
                    bool decimaltxt2 = CommonMethod.NumberOnly(price2txtBox.Text);
                    if (decimaltxt == false || decimaltxt2 == false)
                    {
                        MessageBox.Show("Number Only in Price or Decimal");
                        return;
                    }
                    string monny = pricetxtBox.Text + "." + price2txtBox.Text;
                    decimal price = decimal.Parse(monny);
                    bool quantitytxt = CommonMethod.NumberOnly(quantitytxtBox.Text);
                    if (quantitytxt == false)
                    {
                        MessageBox.Show("Put number only in quantity");
                        return;
                    }
                    book.Price = price;
                    book.Quantity = int.Parse(quantitytxtBox.Text);
                    DataAccess.SaveBook(book);
                    RefreshData();
                    Clear();
                    return;
                }
            }
            
        }
        private void SerachBtn_Click(object sender, RoutedEventArgs e)
        {
            if (searchtxtBox.Text == "") { RefreshData(); return; }
            book = DataAccess.SearchBooks(searchtxtBox.Text);
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = book;
        }
        private void SearchtxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { SerachBtn_Click(null, null); }
        }

        private void pricetxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod) 
            {
                e.Handled = true;
                price2txtBox.Focus();
            }
        }
    }
}
