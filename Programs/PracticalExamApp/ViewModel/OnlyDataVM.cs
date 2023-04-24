using PracticalExamApp.Validation.TypesOfValidation;
using PracticalExamApp.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilsWPF;

namespace PracticalExamApp.ViewModel
{
    public  class OnlyDataVM : ObserverVM
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private int _Age;
        public int Age
        {
            get { return _Age; }
            set
            {
                _Age = value;
                OnPropertyChanged();
            }
        }

        private string _helloMessage;
        public string HelloMessage
        {
            get { return _helloMessage; }
            set
            {
                _helloMessage = value;
                OnPropertyChanged();
            }
        }

        private string _legalAgeMessage;
        public string LegalAgeMessage
        {
            get { return _legalAgeMessage; }
            set
            {
                _legalAgeMessage = value;
                OnPropertyChanged();
            }
        }

        private ICommand _commandCheckBinding;
        public ICommand CommandCheckBinding
        {
            get
            {
                if (_commandCheckBinding == null)
                {
                    _commandCheckBinding = new RelayCommand<object>((o) =>
                    {
                        HelloMessage = "Witaj " + Name;
                        LegalAgeMessage = Age >= 18 ? "Pełnoletni" : "Niepełnoletni";
                    });
                }
                return _commandCheckBinding;
            }
            set { _commandCheckBinding = value; }
        }
    }
}
