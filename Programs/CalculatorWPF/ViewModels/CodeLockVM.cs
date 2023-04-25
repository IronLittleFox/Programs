using System;
using System.Windows.Input;
using UtilsWPF;

namespace CalculatorWPF.ViewModels
{
    class CodeLockVM : ObserverVM, ICalculatorVM
    {
        public string NameOfViewModel { get => "Zamek szyfrowy"; set => throw new NotImplementedException(); }

        private string _showValue;
        public string ShowValue
        {
            get => _showValue;
            set
            {
                _showValue = value;
                OnPropertyChanged(nameof(ShowValue));
            }
        }

        private ICommand _numberCommand;
        public ICommand NumberCommand
        {
            get
            {
                if (_numberCommand == null)
                    _numberCommand = new RelayCommand<object>(
                        o =>
                        {
                            ShowValue += o.ToString();
                        },
                        o => ShowValue.Length != 4
                        );
                return _numberCommand;
            }
        }

        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                    _clearCommand = new RelayCommand<object>(
                        o =>
                        {
                            ShowValue = "";
                        }
                        );
                return _clearCommand;
            }
        }

        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                    _backCommand = new RelayCommand<object>(
                        o =>
                        {
                            ShowValue = ShowValue.Substring(0, ShowValue.Length - 1);
                        },
                        o => ShowValue.Length != 0
                        );
                return _backCommand;
            }
        }

        private ICommand _keyDownCommand;
        public ICommand KeyDownCommand
        {
            get
            {
                if (_keyDownCommand == null)
                    _keyDownCommand = new RelayCommand<object>(
                        (object o) =>
                        {
                            KeyEventArgs eventArgs = o as KeyEventArgs;
                            if (eventArgs is not null)
                            {
                                if (eventArgs.Key >= Key.NumPad0
                                    && eventArgs.Key <= Key.NumPad9
                                    && NumberCommand.CanExecute(null))
                                    NumberCommand.Execute(((int)eventArgs.Key - 74).ToString());

                                if (eventArgs.KeyboardDevice.Modifiers == ModifierKeys.None
                                    && eventArgs.Key >= Key.D0
                                    && eventArgs.Key <= Key.D9
                                    && NumberCommand.CanExecute(null))
                                    NumberCommand.Execute(((int)eventArgs.Key - 34).ToString());

                                switch (eventArgs.Key)
                                {
                                    case Key.Return:
                                        if (EqualCommand.CanExecute(null))
                                            EqualCommand.Execute(null);
                                        break;
                                    case Key.Back:
                                        if (BackCommand.CanExecute(null))
                                            BackCommand.Execute(null);
                                        break;
                                    case Key.Delete:
                                        if (ClearCommand.CanExecute(null))
                                            ClearCommand.Execute(null);
                                        break;
                                };
                            }
                        });
                return _keyDownCommand;
            }
        }

        private ICommand _equalCommand;
        public ICommand EqualCommand
        {
            get
            {
                if (_equalCommand == null)
                    _equalCommand = new RelayCommand<object>(
                        o =>
                        {
                            //tutaj sprawdzamy ShowValue i reagujemy odpowiednio czy jest poprawne czy nie
                        },
                        o => ShowValue.Length == 4
                        );
                return _equalCommand;
            }
        }

        //Not use commands

        private ICommand _arithmeticOperationsCommand;
        public ICommand ArithmeticOperationsCommand
        {
            get
            {
                if (_arithmeticOperationsCommand == null)
                    _arithmeticOperationsCommand = new RelayCommand<object>(
                        o =>
                        {
                        },
                        o => false
                        );
                return _arithmeticOperationsCommand;
            }
        }

        private ICommand _closeParenthesisOperationsCommand;
        public ICommand CloseParenthesisOperationsCommand
        {
            get
            {
                if (_closeParenthesisOperationsCommand == null)
                    _closeParenthesisOperationsCommand = new RelayCommand<object>(
                        o =>
                        {
                        },
                        o => false
                        );
                return _closeParenthesisOperationsCommand;
            }
        }

        private ICommand _openParenthesisOperationsCommand;
        public ICommand OpenParenthesisOperationsCommand
        {
            get
            {
                if (_openParenthesisOperationsCommand == null)
                    _openParenthesisOperationsCommand = new RelayCommand<object>(
                        o =>
                        {
                        },
                        o => false
                        );
                return _openParenthesisOperationsCommand;
            }
        }

        private ICommand _functionCommand;
        public ICommand FunctionCommand
        {
            get
            {
                if (_functionCommand == null)
                    _functionCommand = new RelayCommand<object>(
                        o =>
                        {
                        },
                        o => false
                        );
                return _functionCommand;
            }
        }

        public bool IsParenthesisAvailable { get => false; set => throw new NotImplementedException(); }


    }
}
