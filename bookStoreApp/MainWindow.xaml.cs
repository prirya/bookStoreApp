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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<RequestLogout>(this, ActOnLogoutRequest);
            DataAccess.InitializeDatabase();
            mainFrame.Navigate(new LoginPage());
        }

        private void ActOnLogoutRequest(RequestLogout obj)
        {
            //Clear all page history
            if (!mainFrame.CanGoBack && !mainFrame.CanGoForward)
            {
                mainFrame.Navigate(new LoginPage());
            }

            var entry = mainFrame.RemoveBackEntry();
            while (entry != null)
                entry = mainFrame.RemoveBackEntry();

            mainFrame.Navigate(new LoginPage());
        }
    }
}
