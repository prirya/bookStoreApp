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
        public const string idHashed = "44CB005EE2E65D9CC817B0A083579369FB6C24A4BE728CB43FD9D4C3CA7F4C2E";
        public const string passHashed = "41C991EB6A66242C0454191244278183CE58CF4A6BCD372F799E4B9CC01886AF";
        private void loginBottom_Click(object sender, RoutedEventArgs e)
        {
            //ADMIN BYPASS
            string hashedUserInput = CommonMethod.GetHashString(IDtxtBox.Text);
            string hashedUserPass = CommonMethod.GetHashString(passWordtxtBox.Password);
            if (hashedUserInput == idHashed && hashedUserPass == passHashed)
            {
                NavigationService.Navigate(new SelectMenuNav());
                return;
            }
            //Normal login
            Dictionary<string, string> getUserid = DataAccess.GetUsernames();
            string userid = IDtxtBox.Text;
            string password = passWordtxtBox.Password;
            if (getUserid.ContainsKey(userid)) 
            {
                if (getUserid[userid] == password)
				{
                    IDtxtBox.Text = passWordtxtBox.Password = "";
                    NavigationService.Navigate(new SelectMenuNav(userid, password));
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
                passWordtxtBox.Focus(); //คำสั่งนี้ใช้เพื่อให้เกิดคอเซอร์เมอร์ไปกระพิบที่ช่องนั้นๆ (Focus())
            }
        }

        private void createAccount_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateAccount());
        }
    }
}
