using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace bookStoreApp
{
    class CommonMethor
    {
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public static bool NumberOnly(string word)
        {
            foreach (var c in word)
            {
                if (char.IsDigit(c) == true)
                {
                    continue;
                }
                else if (char.IsDigit(c) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
