using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    [Flags]
    public enum CellState
    {
        None = 0x0,
        Dead = 0x2,
        Live = 0x4,
        Dying = 0x8,
        Newborn = 0x10
    }
}
