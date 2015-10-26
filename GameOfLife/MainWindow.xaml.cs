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

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameBoard gameBoard;

        public MainWindow()
        {
            InitializeComponent();
            this.gameBoard = new GameBoard(this.BoardGrid);
        }

        private void MenuItem_ChangeBoardDim_OnClick(object sender, RoutedEventArgs e)
        {

           this.BoardGrid.Children.Clear();
           this.gameBoard = new GameBoard(this.BoardGrid, 10, 10);
        }

        private void ButtonIterate_OnClick(object sender, RoutedEventArgs e)
        {
            this.gameBoard.Iterate();
        }
    }
}
