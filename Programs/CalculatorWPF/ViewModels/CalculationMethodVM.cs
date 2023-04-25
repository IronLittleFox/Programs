using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilsWPF;

namespace CalculatorWPF.ViewModels
{
    class CalculationMethodVM : ObserverVM
    {
        public CalculationMethodVM()
        {
            ChoseVM = new CalculatorOnpVM();
            ListOfViewModel = new List<ICalculatorVM>();
            ListOfViewModel.Add(ChoseVM);
            ListOfViewModel.Add(new CalculatorVM());
            ListOfViewModel.Add(new CodeLockVM());
        }

        private ICalculatorVM _choseVM;
        public ICalculatorVM ChoseVM
        {
            get { return _choseVM; }
            set 
            { 
                _choseVM = value;
                _choseVM.ClearCommand.Execute(null);
                OnPropertyChanged(nameof(ChoseVM));
                OnPropertyChanged(nameof(KeyDownCommand));
            }
        }

        private List<ICalculatorVM> _listOfViewModel;
        public List<ICalculatorVM> ListOfViewModel
        {
            get { return _listOfViewModel; }
            set 
            { 
                _listOfViewModel = value;
                OnPropertyChanged(nameof(ListOfViewModel));
            }
        }

        public ICommand KeyDownCommand
        {
            get
            {
                return ChoseVM.KeyDownCommand;
            }
        }
    }
}
