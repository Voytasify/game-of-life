using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Cell
    {
        public const double Size = 20.0;
        public static SolidColorBrush BrushLive = Brushes.DarkViolet;
        public static SolidColorBrush BrushDead = Brushes.Beige;

        public Rectangle Rectangle { get; set; }

        public CellState State
        {
            get { return Equals(this.Rectangle.Fill, Cell.BrushLive) ? CellState.Live : CellState.Dead; }
            set { this.Rectangle.Fill = Equals(value, CellState.Live) ? Cell.BrushLive : Cell.BrushDead; }
        }
    }
}
