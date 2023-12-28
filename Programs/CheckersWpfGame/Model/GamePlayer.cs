using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersWpfGame.Model
{
    public class GamePlayer : ViewObserver
    {
        private string playerColor;
        public string PlayerColor
        {
            get { return playerColor; }
            set
            {
                playerColor = value;
                OnPropertyChanged(nameof(PlayerColor));
            }
        }

        public List<CheckerPiece> CheckerPieces { get; set; }
    }
}
