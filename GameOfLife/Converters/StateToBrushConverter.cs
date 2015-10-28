using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GameOfLife.Converters
{
    public class StateToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CellState cellState = (CellState)value;

            if (cellState == (CellState.Live | CellState.Newborn | CellState.Dying))
            {
                return Cell.BrushNewbornDying;
            }
            else if(cellState == (CellState.Live | CellState.Newborn))
            {
                return Cell.BrushNewborn;
            }
            else if(cellState == (CellState.Live | CellState.Dying))
            {
                return Cell.BrushDying;
            }
            else if (cellState == CellState.Live)
            {
                return Cell.BrushLive;
            }
            else
            {
                return Cell.BrushDead;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = value as SolidColorBrush;

            if (Equals(brush, Cell.BrushLive))
            {
                return CellState.Live;
            }
            else
            {
                return CellState.Dead;
            }
        }
    }
}
