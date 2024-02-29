
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoMauiGame.Model
{
    public class GoField : BindableObject
    {
        private int colIndex;
        public int ColIndex
        {
            get { return colIndex; }
            set
            {
                colIndex = value;
                OnPropertyChanged(nameof(ColIndex));
            }
        }

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

        private string sign = "";
        public string Sign
        {
            get { return sign; }
            set
            {
                sign = value;
                OnPropertyChanged(nameof(Sign));
            }
        }

        private string colorPawn = "";
        public string ColorPawn
        {
            get { return colorPawn; }
            set
            {
                colorPawn = value;
                OnPropertyChanged(nameof(ColorPawn));
            }
        }

        public ICommand? BoardFieldCommand { get; set; }
    }
}
