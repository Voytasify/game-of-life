using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{
    class GameBoard
    {
        public int IterationCounter { get; set; }

        private Cell[,] Cells;
        private Grid grid;
        private int width;
        private int height;

        public GameBoard(Grid g, int w = 20, int h = 20)
        {
            this.IterationCounter = 0;

            this.grid = g;
            this.width = w;
            this.height = h;

            this.grid.Width = this.width * Cell.Size;
            this.grid.Height = this.height * Cell.Size;

            // add columns
            for (int i = 0; i < this.width; i++)
            {
                this.grid.ColumnDefinitions.Add(new ColumnDefinition());
                this.grid.ColumnDefinitions[i].Width = new GridLength(Cell.Size);
            }

            // add rows
            for (int i = 0; i < this.height; i++)
            {
                this.grid.RowDefinitions.Add(new RowDefinition());
                this.grid.RowDefinitions[i].Height = new GridLength(Cell.Size);
            }

            this.Cells = new Cell[this.width, this.height];

            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    this.Cells[x, y] = new Cell()
                    {
                        Rectangle = new Rectangle() { Height = Cell.Size, Width = Cell.Size, Stroke = Brushes.Black, StrokeThickness = 1 },
                        State = CellState.Dead
                    };
                    this.Cells[x, y].Rectangle.MouseDown += OnMouseDown_Cell;

                    this.grid.Children.Add(this.Cells[x, y].Rectangle);
                    Grid.SetColumn(Cells[x, y].Rectangle, x);
                    Grid.SetRow(Cells[x, y].Rectangle, y);
                }
            }

        }

        ~GameBoard()
        {
         //   grid.Dispatcher.Invoke((Action)(() =>
        //       grid.Children.Clear()));
        }

        private void OnMouseDown_Cell(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Rectangle r = sender as Rectangle;
            r.Fill = Equals(r.Fill, Cell.BrushLive) ? Cell.BrushDead : Cell.BrushLive;
        }

        public void Iterate(int numberOfIterations = 1)
        {
            Cell[,] tmpBoard = GetBoardCopy();

            for (int i = 0; i < numberOfIterations; i++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    for (int y = 0; y < this.height; y++)
                    {
                        int liveNeighboursCount = GetNumberOfLiveNeighbours(tmpBoard, x, y);

                        if (tmpBoard[x, y].State == CellState.Live)
                        {
                            if (liveNeighboursCount < 2 || liveNeighboursCount > 3)
                            {
                                this.Cells[x, y].State = CellState.Dead;
                            }
                        }
                        else
                        {
                            if (liveNeighboursCount == 3)
                            {
                                this.Cells[x, y].State = CellState.Live;
                            }
                        }
                    }
                }
            }

            this.IterationCounter += numberOfIterations;
        }

        private Cell[,] GetBoardCopy()
        {
            Cell[,] boardCopy = new Cell[this.width, this.height];

            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    boardCopy[x, y] = new Cell()
                    {
                        Rectangle = new Rectangle() { Fill = this.Cells[x, y].Rectangle.Fill }
                    };
                }
            }

            return boardCopy;
        }

        private int GetNumberOfLiveNeighbours(Cell[,] board, int cellX, int cellY)
        {
            int counter = 0;

            foreach (Point p in GetNeighboursCoords(cellX, cellY))
            {
                if (IsPointEligible(p))
                {
                    if (board[(int)p.X, (int)p.Y].State == CellState.Live)
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }

        private List<Point> GetNeighboursCoords(int x, int y)
        {
            List<Point> points = new List<Point>();

            points.Add(new Point(x - 1, y - 1));
            points.Add(new Point(x - 1, y));
            points.Add(new Point(x - 1, y + 1));
            points.Add(new Point(x, y + 1));
            points.Add(new Point(x + 1, y + 1));
            points.Add(new Point(x + 1, y));
            points.Add(new Point(x + 1, y - 1));
            points.Add(new Point(x, y - 1));

            return points;
        }

        private bool IsPointEligible(Point p)
        {
            return p.X >= 0 && p.Y >= 0 && p.X < width && p.Y < height;
        }
    }
}
