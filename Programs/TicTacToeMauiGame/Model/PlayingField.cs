using System.Windows.Input;

namespace TicTacToeMauiGame.Model
{
    public class PlayingField : BindableObject
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public ICommand BoardFieldCommand { get; set; }
    }
}
