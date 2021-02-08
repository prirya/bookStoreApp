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
            Loaded += winLoader;
        }

        private void winLoader(object sender, RoutedEventArgs e)
        {
            people = SqliteDataAccess.LoadPeople();
            dataGrid.ItemsSource = people;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        List<UserModel> people = new List<UserModel>();
        private void LoadpeopleList()
        {
            people = SqliteDataAccess.LoadPeople();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectPerson = dataGrid.SelectedCells[0].Item as UserModel;
            if (selectPerson == null) { return; }
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
            emailtxtBox.Text = selectPerson.email;
            addresstxtBox.Text = selectPerson.address;
        }
    }

}
