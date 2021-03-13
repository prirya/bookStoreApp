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
    /// Interaction logic for CustomersManager.xaml
    /// </summary>
    public partial class CustomersManager : Page
    {
        public CustomersManager()
        {
            InitializeComponent();
            datePicker.DisplayDateEnd = DateTime.Now;
            Loaded += Loader;
            phonetxtBox.Focus();
            saveBtn.Visibility = Visibility.Collapsed;
        }
        List<CustomerModel> people = new List<CustomerModel>();
        private void Loader(object sender, RoutedEventArgs e)
        {
            people = DataAccess.GetDataUser();
            dataGrid.ItemsSource = people;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        public void Clear()
        {
            CustomersIDtxtBox.Text = "";
            datePicker.SelectedDate = null;
            emailtxtBox.Text = "";
            addresstxtBox.Text = "";
            maleRadio.IsChecked = false;
            femaleRadio.IsChecked = false;
            firstNametxtBox.Text = "";
            lastNametxtBox.Text = "";
            phonetxtBox.Text = "";
        }
        public void refreshData()
        {
            dataGrid.ItemsSource = null;
            LoadCustomerList();
            dataGrid.ItemsSource = people;
        }
        private void LoadCustomerList()
        {
            people = DataAccess.GetDataUser();
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedCells == null || dataGrid.SelectedCells.Count < 1)
            {
                addBtn.Visibility = Visibility.Visible;
                saveBtn.Visibility = Visibility.Collapsed;
                return;
            }
            var selectPerson = dataGrid.SelectedCells[0].Item as CustomerModel;
            if (selectPerson == null)
            {
                addBtn.Visibility = Visibility.Visible;
                saveBtn.Visibility = Visibility.Collapsed;
                return;
            }
            else if (selectPerson != null)
            {
                addBtn.Visibility = Visibility.Collapsed;
                saveBtn.Visibility = Visibility.Visible;
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
            if (selectPerson.Sex == true)
            {
                maleRadio.IsChecked = true;
            }
            else if (selectPerson.Sex == false)
            {
                femaleRadio.IsChecked = true;
            }
            datePicker.SelectedDate = selectPerson.Birthday;
            emailtxtBox.Text = selectPerson.Email;
            addresstxtBox.Text = selectPerson.Address;
            phonetxtBox.Text = selectPerson.Phone;
        }
        private bool CheckData()
        {
            if (phonetxtBox.Text == "")
            {
                MessageBox.Show("please enter Phone number");
                return false;
            }
            if (firstNametxtBox.Text == "")
            {
                MessageBox.Show("please enter First name");
                return false;
            }
            if (lastNametxtBox.Text == "")
            {
                MessageBox.Show("please enter Last name");
                return false;
            }
            if (maleRadio.IsChecked == false && femaleRadio.IsChecked == false)
            {
                MessageBox.Show("please select your gender");
                return false;
            }
            if (datePicker.SelectedDate == null)
            {
                MessageBox.Show("please select your Birth Day");
                return false;
            }
            return true;
        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData() == false) { return; }
            bool sex = true;
            if (maleRadio.IsChecked == true) { sex = true; }
            if (femaleRadio.IsChecked == true) { sex = false; }
            DataAccess.AddDataCustomerTable(new CustomerModel(0,
                $"{firstNametxtBox.Text} {lastNametxtBox.Text}", 
                addresstxtBox.Text,
                emailtxtBox.Text, 
                datePicker.SelectedDate.Value,
                sex,
                phonetxtBox.Text));
            Clear();
            refreshData();
        }
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData() == false) { return; }
            bool sex = true;
            if (maleRadio.IsChecked == true) { sex = true; }
            if (femaleRadio.IsChecked == true) { sex = false; }
            if (dataGrid.SelectedCells.Count >= 1)
            {
                var selectuser = dataGrid.SelectedCells[0].Item as CustomerModel;
                if (selectuser != null)
                {
                    //selectuser.CustomerId = int.Parse(CustomersIDtxtBox.Text);
                    selectuser.Name = $"{firstNametxtBox.Text} {lastNametxtBox.Text}";
                    selectuser.Address = addresstxtBox.Text;
                    selectuser.Email = emailtxtBox.Text;
                    selectuser.Birthday = datePicker.SelectedDate.Value;
                    selectuser.Sex = sex;
                    DataAccess.UpdateCustomer(selectuser);
                    refreshData();
                    Clear();
                    return;
                }
            }
        }
        private void clearBTn_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            refreshData();
        }
        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells == null || dataGrid.SelectedCells.Count < 1) { return; }
            var selectPerson = dataGrid.SelectedCells[0].Item as CustomerModel; //TODO fix bug if null
            var Result = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButton.YesNo);
            if (Result == MessageBoxResult.Yes)
            {
                DataAccess.RemoveCustomers(selectPerson);
                Clear();
                refreshData();
                return;
            }
            else if (Result == MessageBoxResult.No)
            {
                refreshData();
            }
            
        }
        private void SerachBtn_Click(object sender, RoutedEventArgs e)
        {
            if (searchtxtBox.Text == "") { refreshData(); return; }
            people = DataAccess.SearchCustomers(searchtxtBox.Text);
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = people;
        }
        private void SearchtxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { SerachBtn_Click(null, null); }
        }
    }
}
