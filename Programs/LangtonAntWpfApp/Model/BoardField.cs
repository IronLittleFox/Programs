using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace LangtonAntWpfApp.Model
{
    public class BoardField : ViewObserver
    {
        private int rowIndex;
        public int RowIndex
        {
            get { return rowIndex; }
            set
            {
                rowIndex = value;
                OnPropertyChanged(nameof(RowIndex));
            }
        }

        private int columnIndex;
        public int ColumnIndex
        {
            get { return columnIndex; }
            set
            {
                columnIndex = value;
                OnPropertyChanged(nameof(ColumnIndex));
                Button b;
            }
        }

        private bool isWhite;
        public bool IsWhite
        {
            get { return isWhite; }
            set
            {
                isWhite = value;
                OnPropertyChanged(nameof(IsWhite));
            }
        }

        private string antText = "";
        public string AntText
        {
            get { return antText; }
            set
            {
                antText = value;
                OnPropertyChanged(nameof(AntText));
            }
        }
    }
}
