using GalaSoft.MvvmLight.Messaging;
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
    /// Interaction logic for SelectMenuNav.xaml
    /// </summary>
    public partial class SelectMenuNav : Page
    {
        static string useridsent = "";
        public SelectMenuNav()
        {
            InitializeComponent(); 
            welcomeTextUserName.Text = "BACKDOOR BREACHED";
            welcomeTextUserID.Text = "Is everything alright?";
            mainMenu.SelectedItem = transactions;
        }

        public SelectMenuNav(string username, string password)
        {
            InitializeComponent();
            bool IsAdmin = DataAccess.Typeuser(username, password);
            string hashedID = CommonMethod.GetHashString(username);
            string hashedPassword = CommonMethod.GetHashString(password);
            if (hashedID == LoginPage.idHashed && hashedPassword == LoginPage.passHashed)
            {
                IsAdmin = true;
            }
            adminManager.Visibility = IsAdmin ? Visibility.Visible : Visibility.Collapsed;
            useridsent = username;
            var adminInfo = DataAccess.GetUser(username);
            welcomeTextUserName.Text = $"Welcome {adminInfo.name}";
            welcomeTextUserID.Text = $"({adminInfo.Number}) {adminInfo.userId}";
            mainMenu.SelectedItem = transactions;
        }

        private void NavigationView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            switch ((args.SelectedItem as ModernWpf.Controls.NavigationViewItemBase).Tag)
            {
                case "home":
                    mainFrame.Navigate(new Transactions(useridsent));
                    break;
                case "people":
                    mainFrame.Navigate(new CustomersManager());
                    break;
                case "book":
                    mainFrame.Navigate(new BooksManager());
                    break;
                case "sold":
                    mainFrame.Navigate(new LogSelledList());
                    break;
                case "admin":
                    mainFrame.Navigate(new AdminPage());
                    break;
                case "leave":
                    Messenger.Default.Send(new RequestLogout());
                    break;
            }
        }
    }
}
