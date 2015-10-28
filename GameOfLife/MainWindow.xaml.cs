using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using GameOfLife.Annotations;
using GameOfLife.UserControls;
using Microsoft.Win32;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int MaxBoardWidth = 60;
        private const int MaxBoardHeight = 26;

        public int BoardWidth => Cells.FirstOrDefault().Count;
        public int BoardHeight => Cells.Count;

        private int _generationLeap = 1;
        private int _iterationCounter = 0;

        public int GenerationLeap
        {
            get { return _generationLeap; }
            set
            {
                if (value != _generationLeap)
                {
                    _generationLeap = value;
                    OnPropertyChanged("GenerationLeap");
                }
            }
        }

        public int IterationCounter
        {
            get { return _iterationCounter; }
            set
            {
                if (value != _iterationCounter)
                {
                    _iterationCounter = value;
                    OnPropertyChanged("IterationCounter");
                }
            }
        }
        
        public ObservableCollection<ObservableCollection<Cell>> Cells { get; set; } = new ObservableCollection<ObservableCollection<Cell>>();

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            for (int y = 0; y < MaxBoardHeight; y++)
            {
                Cells.Add(new ObservableCollection<Cell>());

                for (int x = 0; x < MaxBoardWidth; x++)
                {
                    Cells[y].Add(new Cell());
                }
            } 

            InitializeComponent();
            DataContext = this;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MenuItem_ChangeBoardDimensions_OnClick(object sender, RoutedEventArgs e)
        {
            BoardDimDialogBox dlg = new BoardDimDialogBox(BoardWidth, BoardHeight);

            if (dlg.ShowDialog() != true) return;

            if (dlg.BoardHeight < 1 || dlg.BoardWidth < 1 || dlg.BoardHeight > MaxBoardHeight || dlg.BoardWidth > MaxBoardWidth)
            {
                MessageBox.Show(
                    String.Format(
                        "Wymiary planszy muszą mieścić się w zakresie: {0} [0 - {1}] (szerokość) x [1 - {2}] (wysokość).",
                        Environment.NewLine, MaxBoardWidth, MaxBoardHeight), "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (dlg.BoardHeight > BoardHeight)
            {
                int diff = dlg.BoardHeight - BoardHeight;
                for (int i = 0; i < diff; i++)
                {
                    Cells.Add(new ObservableCollection<Cell>());
                    for (int j = 0; j < BoardWidth; j++)
                    {
                        Cells.Last().Add(new Cell());
                    }
                }
            }
            else if (dlg.BoardHeight < BoardHeight)
            {
                int diff = BoardHeight - dlg.BoardHeight;
                for (int i = 0; i < diff; i++)
                {
                    Cells.RemoveAt(BoardHeight - 1);
                }
            }

            if (dlg.BoardWidth > BoardWidth)
            {
                int diff = dlg.BoardWidth - BoardWidth;
                for (int i = 0; i < BoardHeight; i++)
                {
                    for (int j = 0; j < diff; j++)
                    {
                        Cells[i].Add(new Cell());
                    }
                }
            }
            else if (dlg.BoardWidth < BoardWidth)
            {
                int diff = BoardWidth - dlg.BoardWidth;
                for (int i = 0; i < BoardHeight; i++)
                {
                    for (int j = 0; j < diff; j++)
                    {
                        Cells[i].RemoveAt(BoardWidth - 1);
                    }
                }
            }

        }

        private void MenuItem_ChangeGenerationLeap_OnClick(object sender, RoutedEventArgs e)
        {
            GenLeapDialogBox dlg = new GenLeapDialogBox(GenerationLeap);

            if (dlg.ShowDialog() != true) return;

            GenerationLeap = dlg.GenerationLeap;
        }

        private void ButtonIterate_OnClick(object sender, RoutedEventArgs e)
        {
            Iterate(GenerationLeap);
            IterationCounter += GenerationLeap;
        }

        private void MenuItem_Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_LoadGameState_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".text";
            dlg.Filter = "Dokumenty tekstowe (.txt)|*.txt";

            if (dlg.ShowDialog() != true) return;

            try
            {
                string[] gameState = File.ReadAllLines(dlg.FileName);

                Cells.Clear();

                for (int i = 0; i < gameState.Length; i++)
                {
                    Cells.Add(new ObservableCollection<Cell>());

                    for (int j = 0; j < gameState[0].Length; j++)
                    {
                        Cells[i].Add(new Cell() { State = gameState[i][j] == '1' ? CellState.Live : CellState.Dead });
                    }
                }

                ResetIterationCounter();
            }
            catch (Exception)
            {
                MessageBox.Show("Nie udało się wczytać stanu gry.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_SaveGameState_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "StanGry";
            dlg.DefaultExt = ".text"; 
            dlg.Filter = "Dokumenty tekstowe (.txt)|*.txt"; 

            if (dlg.ShowDialog() != true) return;

            File.WriteAllText(dlg.FileName, SerializeGameBoard());
        }

        public void ResetIterationCounter()
        {
            IterationCounter = 0;
        }

        private void MenuItem_HighlightDyingCells_OnClick(object sender, RoutedEventArgs e)
        {
            CalculateNextState();

            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < Cells[i].Count; j++)
                {
                    if (Cells[i][j].IsDying)
                    {
                        Cells[i][j].State |= CellState.Dying;
                    }
                }
            }
        }

        private void MenuItem_HighlightNewbornCells_OnClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < Cells[i].Count; j++)
                {
                    if (Cells[i][j].IsNewborn)
                    {
                        Cells[i][j].State |= CellState.Newborn;
                    }
                }
            }
        }

        private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < Cells[i].Count; j++)
                {
                    Cells[i][j].State = CellState.Dead;
                }
            }

            ResetIterationCounter();  
        }

        private void CalculateNextState()
        {
            List<List<Cell>> tmpBoard = GetBoardCopy();

            for (int y = 0; y < BoardHeight; y++)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    int liveNeighboursCount = GetNumberOfLiveNeighbours(tmpBoard, x, y);

                    if (tmpBoard[y][x].State == CellState.Live || tmpBoard[y][x].State == (CellState.Live | CellState.Newborn) || tmpBoard[y][x].State == (CellState.Live | CellState.Dying) || (tmpBoard[y][x].State == (CellState.Live | CellState.Newborn | CellState.Dying)))
                    {
                        if (liveNeighboursCount < 2 || liveNeighboursCount > 3)
                        {
                            Cells[y][x].NextState = CellState.Dead;
                        }
                        else
                        {
                            Cells[y][x].NextState = CellState.Live;
                        }
                    }
                    else
                    {
                        if (liveNeighboursCount == 3)
                        {
                            Cells[y][x].NextState = CellState.Live;
                        }
                        else
                        {
                            Cells[y][x].NextState = CellState.Dead;
                        }
                    }
                }
            }
        }

        private void Iterate(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                Iterate();
            }    
        }

        private void Iterate()
        {
            List<List<Cell>> tmpBoard = GetBoardCopy();

            for (int y = 0; y < BoardHeight; y++)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    int liveNeighboursCount = GetNumberOfLiveNeighbours(tmpBoard, x, y);

                    Cells[y][x].PreviousState = Cells[y][x].State;

                    if (tmpBoard[y][x].State == CellState.Live || tmpBoard[y][x].State == (CellState.Live | CellState.Newborn) || tmpBoard[y][x].State == (CellState.Live | CellState.Dying) || (tmpBoard[y][x].State == (CellState.Live | CellState.Newborn | CellState.Dying)))
                    {
                        if (liveNeighboursCount < 2 || liveNeighboursCount > 3)
                        {
                            Cells[y][x].State = CellState.Dead;
                        }
                        else
                        {
                            Cells[y][x].State = CellState.Live;
                        }
                    }
                    else
                    {
                        if (liveNeighboursCount == 3)
                        {
                            Cells[y][x].State = CellState.Live;
                        }
                        else
                        {
                            Cells[y][x].State = CellState.Dead;
                        }
                    }
                }
            }
        }

        private List<List<Cell>> GetBoardCopy()
        {
            List<List<Cell>> boardCopy = new List<List<Cell>>();

            for (int y = 0; y < BoardHeight; y++)
            {
                boardCopy.Add(new List<Cell>());
                for (int x = 0; x < BoardWidth; x++)
                {
                    boardCopy[y].Add(new Cell() {State = Cells[y][x].State});
                }
            }

            return boardCopy;
        }

        private int GetNumberOfLiveNeighbours(List<List<Cell>> board , int cellX, int cellY)
        {
            return GetNeighboursCoords(cellX, cellY)
                .Where(p => p.X >= 0 && p.Y >= 0 && p.X < BoardWidth && p.Y < BoardHeight)
                .Count(p => board[(int)p.Y][(int)p.X].State == CellState.Live || board[(int)p.Y][(int)p.X].State == (CellState.Live | CellState.Newborn) || board[(int)p.Y][(int)p.X].State == (CellState.Live | CellState.Dying) || board[(int)p.Y][(int)p.X].State == (CellState.Live | CellState.Newborn | CellState.Dying));
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

        private string SerializeGameBoard()
        {
            StringBuilder sb = new StringBuilder();

            foreach (ObservableCollection<Cell> cells in Cells)
            {
                foreach (Cell cell in cells)
                {
                    sb.Append(cell.State == CellState.Live ? '1' : '0');
                }

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    } 
}