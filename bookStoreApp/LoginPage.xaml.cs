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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void loginBottom_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> getUserid = DataAccess.GetUsernames();
            string userid = IDtxtBox.Text;
            string password = passWordtxtBox.Text;
            if (getUserid.ContainsKey(userid)) 
            {
                if (getUserid[userid] == password)
				{
                    IDtxtBox.Text = passWordtxtBox.Text = "";
                    NavigationService.Navigate(new SelectMenu());
                    return;
                }
            }
			else
			{
                MessageBox.Show("Wrong ID, Password or both");
			}
        }

        private void passWordtxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            {
                loginBottom_Click(null,null);
            }
        }

        private void IDtxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                passWordtxtBox.Focus();
            }
#if DEBUG
            if (e.Key == Key.F12)
            {
                IDtxtBox.Text = "root";
                passWordtxtBox.Text = "7777";
                loginBottom_Click(null, null);
            }
#endif
        }
    }
}
