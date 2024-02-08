
using CheckersMauiGame.Enums;

namespace CheckersMauiGame.Model
{
    public class CheckerPawn : CheckerPiece
    {
        public CheckerPawn(string color, List<(TypeOfDirection typeOfDirection, int col, int row)> directions) : base("Pawn", color, 1, directions)
        {
        }
    }
}
