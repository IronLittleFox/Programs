
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiMauiGame.Model
{
    public class GamePlayer : BindableObject
    {
        private string playerColor = "";
        public string PlayerColor
        {
            get { return playerColor; }
            set
            {
                playerColor = value;
                OnPropertyChanged(nameof(PlayerColor));
            }
        }

        public List<Pawn> Pawns { get; set; } = new();
    }
}
