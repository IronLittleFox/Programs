using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheckersWpfGame.Model
{
    public class PlayingField : ViewObserver
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }

        private string _fieldColor = "white";
        public string FieldColor
        {
            get { return _fieldColor; }
            set
            {
                _fieldColor = value;
                OnPropertyChanged(nameof(FieldColor));
            }
        }

        public string SpareColor { get; set; }

        public ICommand? BoardFieldCommand { get; set; } = null;

        private Pawn playerPawn = null;
        public Pawn PlayerPawn
        {
            get { return playerPawn; }
            set
            {
                playerPawn = value;
                OnPropertyChanged(nameof(PlayerPawn));
            }
        }
    }
}
