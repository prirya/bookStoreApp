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
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Page
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void createAccount_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData() == false)
                return;
            else
            {
                bool sex = true;
                if (maleRadio.IsChecked == true) { sex = true; }
                if (femaleRadio.IsChecked == true) { sex = false; }
                bool shouldBeAdmin = false;
                bool hasAdmin = DataAccess.IsContainAdmin();
                if (hasAdmin == false)
                    shouldBeAdmin = true;
                DataAccess.AddUserTable(userIDtxtBox.Text, passwordBox.Password, $"{firstNametxtBox.Text} {lastNametxtBox.Text}", addresstxtBox.Text, emailtxtBox.Text, datePicker.SelectedDate.Value, sex, shouldBeAdmin);
                MessageBox.Show("Account create succesfully");
                NavigationService.GoBack();
            }
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

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
