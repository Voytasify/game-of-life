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
    /// Interaction logic for BoardDimDialogBox.xaml
    /// </summary>
    public partial class BoardDimDialogBox : Window
    {
        public int BoardWidth => Convert.ToInt32(TextBoxWidth.Text);
        public int BoardHeight => Convert.ToInt32(TextBoxHeight.Text);

        public BoardDimDialogBox(int width, int height)
        {
            InitializeComponent();
            
            TextBoxWidth.Text = Convert.ToString(width);
            TextBoxHeight.Text = Convert.ToString(height);
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
