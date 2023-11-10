using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConnectFourWpfGame.Model
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

        public ICommand? BoardFieldCommand { get; set; } = null;

    }
}
