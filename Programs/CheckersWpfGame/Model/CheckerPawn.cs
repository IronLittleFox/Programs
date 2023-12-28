using CheckersWpfGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersWpfGame.Model
{
    public class CheckerPawn : CheckerPiece
    {
        public CheckerPawn(string color, List<(TypeOfDirection typeOfDirection, int col, int row)> directions) : base("Pawn", color, 1, directions)
        {
        }
    }
}
