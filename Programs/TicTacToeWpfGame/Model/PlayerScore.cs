using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeWpfGame.Model
{
    public class PlayerScore : ViewObserver
    {

        private Player player;
        public Player Player
        {
            get { return player; }
            set
            {
                player = value;
                OnPropertyChanged(nameof(Player));
            }
        }

        private int score;
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                OnPropertyChanged(nameof(Score));
            }
        }
    }
}
