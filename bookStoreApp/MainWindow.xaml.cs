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
            DataAccess.InitializeDatabase();
        }

        private void adddatabottom_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.AddDataCustomerTable(nameTextbox.Text, addressTextbox.Text, emailTextbox.Text);
            nameTextbox.Text = addressTextbox.Text = emailTextbox.Text = "";
        }

        private void showBottom_Click(object sender, RoutedEventArgs e)
        {
                string textResult = ""; //ข้อความว่างสำหรับเก็บ Entry อื่นๆ
                List<string> getData = DataAccess.GetData(); //เอาข้อมูลจาก database มาเก็บเป็นตัวแปล getdata
                for (int i = 0; i < getData.Count; i++) //วนซ้ำตามเงื่อนไข
                {
                    textResult += getData[i] + "\n"; //ขึ้นบรรทัดใหม่ // += คือ ต่อเติมค่าที่อยู่ข้างหน้าเข้าไปหาค่าที่อยู่ข้างหลัง ยกตัวอย่าง A += B ก็จะได้ AB
                }
                MessageBox.Show(textResult);
        }
    }
}
