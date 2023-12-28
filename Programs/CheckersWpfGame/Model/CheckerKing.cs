using CheckersWpfGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersWpfGame.Model
{
    public class CheckerKing : CheckerPiece
    {
        public CheckerKing(string color, int distance, List<(TypeOfDirection typeOfDirection, int col, int row)> directions) : base("King", color, distance, directions)
        {
        }
    }
}
