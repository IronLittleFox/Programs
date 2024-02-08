using CheckersMauiGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersMauiGame.Model
{
    public abstract class CheckerPiece
    {
        public string Color { get; }
        public string Type { get;}

        public int Distance { get; set; }

        public List<(TypeOfDirection typeOfDirection, int col, int row)> Directions;

        public CheckerPiece(string type, string color, int distance, List<(TypeOfDirection typeOfDirection, int col, int row)> direction)
        {
            Type = type;
            Color = color;
            Distance = distance;
            Directions = direction;
        }
    }
}
