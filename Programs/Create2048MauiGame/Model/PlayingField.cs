using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Create2048MauiGame.Model
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

        private string color;

        public string Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged(); }
        }

        public ICommand? BoardFieldCommand { get; set; }
    }
}
