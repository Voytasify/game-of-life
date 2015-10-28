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
using System.Windows.Shapes;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for GenLeapDialogBox.xaml
    /// </summary>
    public partial class GenLeapDialogBox : Window
    {
        public int GenerationLeap => Convert.ToInt32(TextBoxGenLeap.Text);

        public GenLeapDialogBox(int genLeap)
        {
            InitializeComponent();

            TextBoxGenLeap.Text = Convert.ToString(genLeap);
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
