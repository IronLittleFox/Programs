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
    public  class FirstErrorVM : ObserverVM
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

        private string _strAge;
        public string StrAge
        {
            get { return _strAge; }
            set
            {
                _strAge = value;
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
                        Validate validate = new Validate();
                        validate.AddValidator(new Validator<string>(Name, "Imie",
                            new List<ISpecyficValidation<string>>()
                            {
                                new ValidateStringEmpty()
                            }));
                        validate.AddValidator(new Validator<string>(StrAge, "Wiek",
                            new List<ISpecyficValidation<string>>()
                            {
                                new ValidateStringEmpty(),
                                new ValidateStringIsNumber(),
                                new ValidateStringNumberIsInRange(1,150)
                            }));

                        if (!validate.Validation(out string message))
                        {
                            HelloMessage = message;
                            LegalAgeMessage = "";
                            return;
                        }

                        HelloMessage = "Witaj " + Name;
                        LegalAgeMessage = ConvertAgeStringToInt(StrAge) >= 18 ? "Pełnoletni" : "Niepełnoletni";
                    });
                }
                return _commandCheckBinding;
            }
            set { _commandCheckBinding = value; }
        }

        private int ConvertAgeStringToInt(string strAge)
        {
            return int.Parse(strAge);
        }
    }
}
