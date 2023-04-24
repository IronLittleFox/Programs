using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TicTacToeWpfGame.Model
{
    public class PlayingField : ViewObserver
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public ICommand BoardFieldCommand { get; set; }
    }
}
