using System.CodeDom;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife.UserControls
{
    /// <summary>
    ///     Interaction logic for GameBoard11.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(GameBoard));

        public GameBoard()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void Cell_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle r = sender as Rectangle;

            if (Equals(r.Fill, Cell.BrushLive) || Equals(r.Fill, Cell.BrushNewborn) || Equals(r.Fill, Cell.BrushDying) || Equals(r.Fill, Cell.BrushNewbornDying))
            {
                r.Fill = Cell.BrushDead;
            }
            else if (Equals(r.Fill, Cell.BrushDead))
            {
                r.Fill = Cell.BrushLive;
            }
        }
    }
}