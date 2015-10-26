using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace GameOfLife
{
    class GameBoard
    {
        public Cell[,] Cells { get; set; }

        private int width;
        private int height;

        public GameBoard(int width, int height)
        {
            Cells = new Cell[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cells[i, j] = new Cell() { State = CellState.Dead };
                }
            }

        }

        public void Iterate(int numberOfIterations)
        {
            Cell[,] tmpBoard = GetBoardCopy();

            for (int i = 0; i < numberOfIterations; i++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    for (int y = 0; y < this.height; y++)
                    {
                        int liveNeighboursCount = GetNumberOfLiveNeighbours(tmpBoard, x, y);

                        if (tmpBoard[i, x].State == CellState.Live)
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
        }

        private Cell[,] GetBoardCopy()
        {
            Cell[,] boardCopy = new Cell[this.width, this.height];

            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    boardCopy[i, j] = new Cell() { State = this.Cells[i, j].State };
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
