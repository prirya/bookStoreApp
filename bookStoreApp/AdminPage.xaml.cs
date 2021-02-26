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
using bookStoreApp;
using System.Data;
using System.Data.SqlClient;

namespace bookStoreApp
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            datePicker.DisplayDateEnd = DateTime.Now;
            Loaded += winLoader;
        }

        private void winLoader(object sender, RoutedEventArgs e)
        {
            people = DataAccess.LoadAdmin();
            dataGrid.ItemsSource = people;
        }
        public void Clear()
        {
            datePicker.DisplayDateEnd = DateTime.Now;
            datePicker.SelectedDate = DateTime.Now;
            userIDtxtBox.Text = "";
            passwordBox.Password = "";
            passwordConfirmBox.Password = "";
            datePicker.SelectedDate = null;
            emailtxtBox.Text = "";
            addresstxtBox.Text = "";
            maleRadio.IsChecked = false;
            femaleRadio.IsChecked = false;
            firstNametxtBox.Text = "";
            lastNametxtBox.Text = "";
            adminCBox.IsChecked = false;
        }
        public void refreshDATA()
        {
            dataGrid.ItemsSource = null;
            LoadpeopleList();
            dataGrid.ItemsSource = people;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        List<UserModel> people = new List<UserModel>();
        private void LoadpeopleList()
        {
            people = DataAccess.LoadAdmin();
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedCells == null || dataGrid.SelectedCells.Count < 1)
            {
                addBtn.Visibility = Visibility.Visible;
                saveBtn.Visibility = Visibility.Collapsed;
                return;
            }
            var selectPerson = dataGrid.SelectedCells[0].Item as UserModel;
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
            userIDtxtBox.Text = selectPerson.userId;
            passwordBox.Password = selectPerson.password;
            passwordConfirmBox.Password = passwordBox.Password;
            var name = selectPerson.name.Split(' ');
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
            if (selectPerson.sex == true)
            {
                maleRadio.IsChecked = true;
            }
            else if (selectPerson.sex == false)
            {
                femaleRadio.IsChecked = true;
            }
            datePicker.SelectedDate = selectPerson.birthday;
            adminCBox.IsChecked = selectPerson.typeAdmin;
            emailtxtBox.Text = selectPerson.email;
            addresstxtBox.Text = selectPerson.address;
        }
        private bool CheckData()
        {
            if (userIDtxtBox.Text == "")
            {
                MessageBox.Show("please enter user ID");
                return false;
            }
            if (passwordBox.Password == "")
            {
                MessageBox.Show("please enter Password");
                return false;
            }
            if (passwordConfirmBox.Password == "")
            {
                MessageBox.Show("please confirm password");
                return false;
            }
            if (passwordBox.Password != passwordConfirmBox.Password)
            {
                MessageBox.Show("Password and Confirm password do not match");
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
            DataAccess.AddUserTable(userIDtxtBox.Text, passwordBox.Password, $"{firstNametxtBox.Text} {lastNametxtBox.Text}", addresstxtBox.Text, emailtxtBox.Text, datePicker.SelectedDate.Value, sex, adminCBox.IsChecked.Value);
            Clear();
            refreshDATA();
        }
        private void ClearBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Clear();
            addBtn.Visibility = Visibility.Visible;
            dataGrid.SelectedIndex = -1;
            refreshDATA();
        }
        private void saveBtn_Click(object sender, RoutedEventArgs e) //DOTO : มีบางอย่างผิดปกติกับฟังชั่นนี้
        {
            if (CheckData() == false) { return; }
            bool sex = true;
            if (maleRadio.IsChecked == true) { sex = true; }
            if (femaleRadio.IsChecked == true) { sex = false; }
            if (dataGrid.SelectedCells.Count >= 1)
            {
                var selectuser = dataGrid.SelectedCells[0].Item as UserModel;
                if (selectuser != null)
                {
                    selectuser.userId = userIDtxtBox.Text;
                    selectuser.password = passwordBox.Password;
                    selectuser.name = $"{firstNametxtBox.Text} {lastNametxtBox.Text}";
                    selectuser.address = addresstxtBox.Text;
                    selectuser.email = emailtxtBox.Text;
                    selectuser.birthday = datePicker.SelectedDate.Value;
                    selectuser.sex = sex;
                    selectuser.typeAdmin = adminCBox.IsChecked.Value;
                    DataAccess.UpdateUser(selectuser);
                    refreshDATA();
                    Clear();
                    return;
                }
            }
        }
        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectPerson = dataGrid.SelectedCells[0].Item as UserModel;
            var Result = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButton.YesNo);
                if (Result == MessageBoxResult.Yes)
                    {
                        DataAccess.RemoveUser(selectPerson);
                        Clear();
                        refreshDATA();
                        return;
                    }
                else if (Result == MessageBoxResult.No)
                    {
                        refreshDATA();
                    }
        }
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SearchtxtBox.Text == "") { refreshDATA(); return; }
            people = DataAccess.SearchPeople(SearchtxtBox.Text);
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = people;
        }
        private void SearchtxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter){ SearchBtn_Click(null , null); }
        }
    }
}
