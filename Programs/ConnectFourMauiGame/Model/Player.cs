using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourMauiGame.Model
{
    public class Player : BindableObject
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
    }
}
