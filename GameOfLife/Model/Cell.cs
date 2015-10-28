using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using GameOfLife.Annotations;

namespace GameOfLife
{
    public class Cell : INotifyPropertyChanged
    {
        public static SolidColorBrush BrushLive = Brushes.Crimson;
        public static SolidColorBrush BrushDead = Brushes.MintCream;
        public static SolidColorBrush BrushNewborn = Brushes.DodgerBlue;
        public static SolidColorBrush BrushDying = Brushes.LightCoral;
        public static SolidColorBrush BrushNewbornDying = Brushes.Aquamarine;

        private CellState _state = CellState.Dead;

        public CellState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        public CellState PreviousState { get; set; } = CellState.Dead;
        public CellState NextState { get; set; } = CellState.Dead;

        public bool IsNewborn => PreviousState == CellState.Dead && (State == CellState.Live || (State == (CellState.Live | CellState.Newborn)) || (State == (CellState.Live | CellState.Dying)) || (State == (CellState.Live | CellState.Newborn | CellState.Dying)));
        public bool IsDying => NextState == CellState.Dead && (State == CellState.Live || (State == (CellState.Live | CellState.Newborn)) || (State == (CellState.Live | CellState.Dying)) || (State == (CellState.Live | CellState.Newborn | CellState.Dying)));

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}