using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace bookStoreApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() { Startup += App_Startup; }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            System.Globalization.CultureInfo Timelockzone = new System.Globalization.CultureInfo("en");
            System.Globalization.DateTimeFormatInfo timeformat = new System.Globalization.DateTimeFormatInfo();
            timeformat.ShortDatePattern = "dd-MM-yyyy";
            timeformat.DateSeparator = "-";
            Timelockzone.DateTimeFormat = timeformat;
            System.Threading.Thread.CurrentThread.CurrentCulture = Timelockzone;
        }
    }
}
