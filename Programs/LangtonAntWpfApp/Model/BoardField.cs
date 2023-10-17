using GameUtils.Utils;

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

        private string antColor;
        public string AntColor
        {
            get { return antColor; }
            set
            {
                antColor = value;
                OnPropertyChanged(nameof(AntColor));
            }
        }
    }
}
