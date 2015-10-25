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
        private const double CellSize = 20.0;
        private const int BoardSize = 40;

        Rectangle[,] Cells = new Rectangle[BoardSize, BoardSize];

        public MainWindow()
        {
            InitializeComponent();

            this.GameBoard.DataContext = this;

            // set board dimensions
            this.GameBoard.Width = BoardSize * CellSize;
            this.GameBoard.Height = BoardSize * CellSize;

            for (int i = 0; i < BoardSize; i++)
            {
                // add columns
                this.GameBoard.ColumnDefinitions.Add(new ColumnDefinition());
                this.GameBoard.ColumnDefinitions[i].Width = new GridLength(CellSize);

                // add rows
                this.GameBoard.RowDefinitions.Add(new RowDefinition());
                this.GameBoard.RowDefinitions[i].Height = new GridLength(CellSize);
            }

            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < CellSize; j++)
                {
                    Cells[i, j] = new Rectangle() { Fill = Brushes.LightSkyBlue, Height = CellSize, Width = CellSize, Stroke = Brushes.Black, StrokeThickness = 1 };
                    Cells[i, j].SetBinding(Rectangle.FillProperty, new Binding(){ Source = Cells[i, j].Fill });
                    Cells[i, j].MouseDown += OnMouseDown;

                    this.GameBoard.Children.Add(Cells[i, j]);
                    Grid.SetColumn(Cells[i, j], i);
                    Grid.SetRow(Cells[i, j], j);
                }
            }

        }

        private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Rectangle r = sender as Rectangle;
            r.Fill = Equals(r.Fill, Brushes.LightSkyBlue) ? Brushes.Crimson : Brushes.LightSkyBlue;
        }
    }
}
