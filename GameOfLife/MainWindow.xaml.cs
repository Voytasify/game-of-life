using System;
using System.Collections.Generic;
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

        private int _boardWidth = MaxBoardWidth;
        private int _boardHeight = MaxBoardHeight;
        private int _generationLeap = 1;
        private int _iterationCounter = 0;

        public int BoardWidth
        {
            get { return _boardWidth; }
            set
            {
                if (value != _boardWidth)
                {
                    _boardWidth = value;
                    OnPropertyChanged("BoardWidth");
                }
            }
        }

        public int BoardHeight
        {
            get { return _boardHeight; }
            set
            {
                if (value != _boardHeight)
                {
                    _boardHeight = value;
                    OnPropertyChanged("BoardHeight");
                }
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        private GameBoard gameBoard;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            gameBoard = new GameBoard(BoardGrid, BoardWidth, BoardHeight);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MenuItem_ChangeBoardDimensions_OnClick(object sender, RoutedEventArgs e)
        {
            BoardDimDialogBox dlg = new BoardDimDialogBox(BoardWidth, BoardHeight);

            if (dlg.ShowDialog() != true) return;

            SetBoardDimensions(dlg.Width, dlg.Height);
            ClearBoardGrid();
            ResetIterationCounter();

            gameBoard = new GameBoard(BoardGrid, BoardWidth, BoardHeight);
        }

        private void MenuItem_ChangeGenerationLeap_OnClick(object sender, RoutedEventArgs e)
        {
            GenLeapDialogBox dlg = new GenLeapDialogBox(GenerationLeap);

            if (dlg.ShowDialog() != true) return;

            GenerationLeap = dlg.GenerationLeap;
        }

        private void ButtonIterate_OnClick(object sender, RoutedEventArgs e)
        {
            gameBoard.Iterate(GenerationLeap);
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

            string[] gameState = File.ReadAllLines(dlg.FileName);

            SetBoardDimensions(gameState[0].Length, gameState.Length);
            ClearBoardGrid();
            ResetIterationCounter();

            gameBoard = new GameBoard(BoardGrid, BoardWidth, BoardHeight, gameState);
        }

        private void MenuItem_SaveGameState_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "StanGry";
            dlg.DefaultExt = ".text"; 
            dlg.Filter = "Dokumenty tekstowe (.txt)|*.txt"; 

            if (dlg.ShowDialog() != true) return;

            File.WriteAllText(dlg.FileName, gameBoard.ToString());
        }

        public void ClearBoardGrid()
        {
            BoardGrid.Children.Clear();
        }

        public void ResetIterationCounter()
        {
            IterationCounter = 0;
        }

        public void SetBoardDimensions(int width, int height)
        {
            BoardWidth = width;
            BoardHeight = height;
        }

        private void MenuItem_HighlightDyingCells_OnClick(object sender, RoutedEventArgs e)
        {
            gameBoard.HighlightDyingCells();
        }

        private void MenuItem_HighlightNewbornCells_OnClick(object sender, RoutedEventArgs e)
        {
            gameBoard.HighlightNewbornCells();
        }
    }
}