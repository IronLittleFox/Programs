using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersWpfGame.Model
{
    public class Pawn : ViewObserver
    {
        private string pawnColor = "Transparent";
        public string PawnColor
        {
            get { return pawnColor; }
            set
            {
                pawnColor = value;
                OnPropertyChanged(nameof(PawnColor));
            }
        }

        private string pawnMustMoveColor = "Transparent";
        public string PawnMustMoveColor
        {
            get { return pawnMustMoveColor; }
            set
            {
                pawnMustMoveColor = value;
                OnPropertyChanged(nameof(PawnMustMoveColor));
            }
        }

        public int Distance { get; set; }

        public List<(int col, int row)> DirectionsOfMovements;
    }
}
