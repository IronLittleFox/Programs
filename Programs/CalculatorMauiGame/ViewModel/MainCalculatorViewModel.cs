using CalculatorMauiGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilsMaui.Interfaces;
using UtilsMaui.Utils;

namespace CalculatorMauiGame.ViewModel
{
    public class MainCalculatorViewModel : BindableObject, IGameViewModel
    {
        public MainCalculatorViewModel()
        {
            ChoseVM = new CalculatorOnpViewModel();
            ListOfViewModel = new List<ICalculatorViewModel>();
            ListOfViewModel.Add(ChoseVM);
            ListOfViewModel.Add(new CalculatorRegularViewModel());
            //ListOfViewModel.Add(new CodeLockVM());
        }

        private ICalculatorViewModel _choseVM;
        public ICalculatorViewModel ChoseVM
        {
            get { return _choseVM; }
            set
            {
                (ChoseVM as IGameViewModel)?.Dispose();
                _choseVM = value;
                _choseVM.ClearCommand.Execute(null);
                OnPropertyChanged(nameof(ChoseVM));
                OnPropertyChanged(nameof(KeyDownCommand));
            }
        }

        private List<ICalculatorViewModel> _listOfViewModel;
        public List<ICalculatorViewModel> ListOfViewModel
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
        public void Dispose()
        {
            (ChoseVM as IGameViewModel)?.Dispose();
        }
    }
}
