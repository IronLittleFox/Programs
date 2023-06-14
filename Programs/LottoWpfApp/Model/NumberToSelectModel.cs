using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilsWPF;

namespace LottoWpfApp.Model
{
    public class NumberToSelectModel: ObserverVM
    {
        private int number;
        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        private bool isSelect;
        public bool IsSelect
        {
            get { return isSelect; }
            set
            {
                isSelect = value;
                OnPropertyChanged(nameof(IsSelect));
            }
        }

        public ICommand SelectNumberCommand { get; set; }
    }
}
