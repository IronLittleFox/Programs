using GameUtils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SudokuWpfGame.Model
{
    public class NumberToChoose : ViewObserver
    {
        private string number;
        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        private bool isChoose;
        public bool IsChoose
        {
            get { return isChoose; }
            set
            {
                isChoose = value;
                OnPropertyChanged(nameof(IsChoose));
            }
        }
        public ICommand NumberToChooseCommand { get; set; }
    }
}
