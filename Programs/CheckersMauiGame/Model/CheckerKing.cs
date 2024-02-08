
using CheckersMauiGame.Enums;


namespace CheckersMauiGame.Model
{
    public class CheckerKing : CheckerPiece
    {
        public CheckerKing(string color, int distance, List<(TypeOfDirection typeOfDirection, int col, int row)> directions) : base("King", color, distance, directions)
        {
        }
    }
}
