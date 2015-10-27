using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Cell
    {
        public const double Size = 20.0;

        public static SolidColorBrush BrushLive = Brushes.Crimson;
        public static SolidColorBrush BrushDead = Brushes.MintCream;
        public static SolidColorBrush BrushHighlight = Brushes.Orange;

        public static DisplayMode DisplayMode { get; set; } = DisplayMode.Normal;

        public Rectangle Rectangle { get; set; } = new Rectangle()
        {
            Tag = CellState.Dead,
            Height = Cell.Size,
            Width = Cell.Size,
            Stroke = Brushes.DarkGray,
            StrokeThickness = 2,
            Margin = new Thickness(1.0, 1.0, 1.0, 1.0)
        };

        public CellState State
        {
            get { return (CellState)Rectangle.Tag; }
            set { Rectangle.Tag = value; }
        }

        public CellState NextState { get; set; } = CellState.Dead;
        public CellState PreviousState { get; set; } = CellState.Dead;

        public Cell()
        {
            Rectangle.MouseDown += (sender, args) =>
            {
                Rectangle r = sender as Rectangle;
                r.Tag = (CellState)r.Tag == CellState.Live ? CellState.Dead : CellState.Live;
                Draw();
            };
        }

        public void Draw()
        {
            Rectangle.Fill = (State == CellState.Live) ? Cell.BrushLive : Cell.BrushDead;

            if (Cell.DisplayMode == DisplayMode.HighlightNewborn)
            {
                if (IsNewborn())
                {
                    Rectangle.Fill = Cell.BrushHighlight;
                }
            }
            else if (Cell.DisplayMode == DisplayMode.HighlightDying)
            {
                if (IsDying())
                {
                    Rectangle.Fill = Cell.BrushHighlight;
                }
            }

        }

        private bool IsNewborn()
        {
            return PreviousState == CellState.Dead && State == CellState.Live;
        }

        private bool IsDying()
        {
            return State == CellState.Live && NextState == CellState.Dead;
        }
    }
}