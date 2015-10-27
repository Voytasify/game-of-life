using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using GameOfLife.Annotations;

namespace GameOfLife
{
    class GameBoard
    {
        private Cell[,] Cells;
        private int width;
        private int height;

        public GameBoard(Grid grid, int w, int h)
        {
            width = w;
            height = h;

            SetupGrid(grid);
            SetupCells(grid);
            DrawCells();
        }

        public GameBoard(Grid grid, int w, int h, string[] gameState)
            : this(grid, w, h)
        {
            SetState(gameState);
            DrawCells();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sb.Append((int)Cells[y, x].State);
                }
                sb.Append(System.Environment.NewLine);
            }

            return sb.ToString();
        }

        public void Iterate(int numberOfIterations)
        {
            for (int i = 0; i < numberOfIterations; i++)
            {
                Iterate();
            }
        }

        public void Iterate()
        {
            Cell[,] tmpBoard = GetBoardCopy();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int liveNeighboursCount = GetNumberOfLiveNeighbours(tmpBoard, x, y);

                    Cells[y, x].PreviousState = Cells[y, x].State;

                    if (tmpBoard[y, x].State == CellState.Live)
                    {
                        if (liveNeighboursCount < 2 || liveNeighboursCount > 3)
                        {
                            Cells[y, x].State = CellState.Dead;
                        }
                        else
                        {
                            Cells[y, x].State = CellState.Live;
                        }
                    }
                    else
                    {
                        if (liveNeighboursCount == 3)
                        {
                            Cells[y, x].State = CellState.Live;
                        }
                        else
                        {
                            Cells[y, x].State = CellState.Dead;
                        }
                    }
                }
            }

            DrawCells();
        }

        public void HighlightNewbornCells()
        {
            SetDisplayMode(DisplayMode.HighlightNewborn);
            DrawCells();
            SetDisplayMode(DisplayMode.Normal);
        }

        public void HighlightDyingCells()
        {
            CalculateNextCellStates();
            SetDisplayMode(DisplayMode.HighlightDying);
            DrawCells();
            SetDisplayMode(DisplayMode.Normal);
        }

        private void CalculateNextCellStates()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int liveNeighboursCount = GetNumberOfLiveNeighbours(Cells, x, y);

                    if (Cells[y, x].State == CellState.Live)
                    {
                        if (liveNeighboursCount < 2 || liveNeighboursCount > 3)
                        {
                            Cells[y, x].NextState = CellState.Dead;
                        }
                        else
                        {
                            Cells[y, x].NextState = CellState.Live;
                        }
                    }
                    else
                    {
                        if (liveNeighboursCount == 3)
                        {
                            Cells[y, x].NextState = CellState.Live;
                        }
                        else
                        {
                            Cells[y, x].NextState = CellState.Dead;
                        }
                    }
                }
            }
        }

        private void SetDisplayMode(DisplayMode displayMode)
        {
            Cell.DisplayMode = displayMode;
        }

        private void DrawCells()
        {
            foreach (Cell cell in Cells)
            {
                cell.Draw();
            }
        }

        private void SetupGrid(Grid grid)
        {
            grid.Width = width * Cell.Size;
            grid.Height = height * Cell.Size;

            // add columns
            for (int i = 0; i < width; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions[i].Width = new GridLength(Cell.Size);
            }

            // add rows
            for (int i = 0; i < height; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions[i].Height = new GridLength(Cell.Size);
            }
        }

        private void SetupCells(Grid grid)
        {
            Cells = new Cell[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Cells[y, x] = new Cell();

                    grid.Children.Add(Cells[y, x].Rectangle);

                    Grid.SetColumn(Cells[y, x].Rectangle, x);
                    Grid.SetRow(Cells[y, x].Rectangle, y);
                }
            }
        }

        private void SetState(string[] gameState)
        {
            for (int y = 0; y < gameState.Length; y++)
            {
                for (int x = 0; x < gameState[0].Length; x++)
                {
                    Cells[y, x].State = gameState[y][x] == '0' ? CellState.Dead : CellState.Live;
                }
            }
        }

        private Cell[,] GetBoardCopy()
        {
            Cell[,] boardCopy = new Cell[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    boardCopy[y, x] = new Cell()
                    {
                        Rectangle = new Rectangle() { Tag = Cells[y, x].Rectangle.Tag }
                    };
                }
            }

            return boardCopy;
        }

        private int GetNumberOfLiveNeighbours(Cell[,] board, int cellX, int cellY)
        {
            return GetNeighboursCoords(cellX, cellY)
                .Where(p => p.X >= 0 && p.Y >= 0 && p.X < width && p.Y < height)
                .Count(p => board[(int)p.Y, (int)p.X].State == CellState.Live);
        }

        private IEnumerable<Point> GetNeighboursCoords(int x, int y)
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
    }
}