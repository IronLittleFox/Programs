using System.Windows.Input;

namespace TicTacToeMauiGame.Model
{
    public class PlayingField : BindableObject
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }

        private string text = "";
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        private Color backgroundColor;

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set 
            { 
                backgroundColor = value;
                OnPropertyChanged();
            }
        }


        public ICommand BoardFieldCommand { get; set; } = new Command<object>(o => { });
    }
}
