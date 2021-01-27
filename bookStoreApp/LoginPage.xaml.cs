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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void adminTest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SelectMenu());
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
                    NavigationService.Navigate(new SelectMenu());
                }
            }
        }
    }
}
