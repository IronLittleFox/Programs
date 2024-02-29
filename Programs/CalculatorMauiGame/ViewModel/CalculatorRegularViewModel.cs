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
    public class CalculatorRegularViewModel : BindableObject, IGameViewModel, ICalculatorViewModel
    {
        private long previewValue = 0;
        private long currentValue = 0;
        private string previewOperator = "+";

        private bool operatorCommandFlag = false;
        private bool operatorEqualFlag = false;

        public bool IsParenthesisAvailable { get; set; } = false;

        public string NameOfViewModel { get; set; } = "Kalkulator zwykły";

        private string _showValue = "";
        public string ShowValue
        {
            get
            {
                return _showValue;
            }
            set
            {
                _showValue = value;
                OnPropertyChanged(nameof(ShowValue));
            }
        }

        private ICommand? _numberCommand;
        public ICommand NumberCommand
        {
            get
            {
                if (_numberCommand == null)
                    _numberCommand = new Command<string>(
                        (string o) =>
                        {
                            int value = int.Parse(o);

                            if (operatorCommandFlag)
                            {
                                currentValue = 0;
                                operatorCommandFlag = false;
                            }
                            if (operatorEqualFlag)
                            {
                                previewValue = 0;
                                previewOperator = "+";
                                operatorEqualFlag = false;
                            }

                            currentValue = currentValue * 10 + value;

                            ShowValue = currentValue.ToString();
                        });
                return _numberCommand;
            }
        }

        private ICommand? _arithmeticOperationsCommand;
        public ICommand ArithmeticOperationsCommand
        {
            get
            {
                if (_arithmeticOperationsCommand == null)
                    _arithmeticOperationsCommand = new Command<string>(
                        (string o) =>
                        {
                            string selectOperator = o;

                            if (operatorEqualFlag)
                            {
                                currentValue = 0;
                                previewOperator = "+";
                                operatorEqualFlag = false;
                            }

                            currentValue = CalculatePreviewOperation();

                            ShowValue = currentValue.ToString();

                            previewValue = currentValue;
                            previewOperator = selectOperator;

                            operatorCommandFlag = true;
                        });
                return _arithmeticOperationsCommand;
            }
        }

        private ICommand? _equalCommand;
        public ICommand EqualCommand
        {
            get
            {
                if (_equalCommand == null)
                    _equalCommand = new Command<object>(
                        (object o) =>
                        {
                            previewValue = CalculatePreviewOperation();
                            ShowValue = previewValue.ToString();

                            operatorCommandFlag = true;
                            operatorEqualFlag = true;
                        });
                return _equalCommand;
            }
        }

        private ICommand? _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                    _clearCommand = new Command<object>(
                        (object o) =>
                        {
                            currentValue = 0;
                            ShowValue = currentValue.ToString();
                            operatorCommandFlag = false;
                            operatorEqualFlag = false;
                            previewValue = 0;
                            previewOperator = "+";
                        });
                return _clearCommand;
            }
        }

        private ICommand? _backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                    _backCommand = new Command<object>(
                        (object o) =>
                        {
                            if (operatorCommandFlag || operatorEqualFlag)
                                return;
                            currentValue = currentValue / 10;
                            ShowValue = currentValue.ToString();
                        },
                        (object o) =>
                        {
                            if (operatorCommandFlag || operatorEqualFlag)
                                return false;
                            else
                                return true;
                        });
                return _backCommand;
            }
        }

        private ICommand? _keyDownCommand;
        public ICommand KeyDownCommand
        {
            get
            {
                if (_keyDownCommand == null)
                    _keyDownCommand = new Command<object>(
                        (object o) =>
                        {
                            /*KeyEventArgs eventArgs = o as KeyEventArgs;
                            if (eventArgs is not null)
                            {
                                if (eventArgs.Key >= Key.NumPad0 && eventArgs.Key <= Key.NumPad9)
                                    NumberCommand.Execute(((int)eventArgs.Key - 74).ToString());

                                if (eventArgs.KeyboardDevice.Modifiers == ModifierKeys.None
                                    && eventArgs.Key >= Key.D0
                                    && eventArgs.Key <= Key.D9)
                                    NumberCommand.Execute(((int)eventArgs.Key - 34).ToString());

                                switch (eventArgs.Key)
                                {
                                    case Key.Add:
                                        ArithmeticOperationsCommand.Execute("+");
                                        break;
                                    case Key.Subtract:
                                        ArithmeticOperationsCommand.Execute("-");
                                        break;
                                    case Key.Multiply:
                                        ArithmeticOperationsCommand.Execute("*");
                                        break;
                                    case Key.Divide:
                                        ArithmeticOperationsCommand.Execute("/");
                                        break;
                                    case Key.D5:
                                        if (eventArgs.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                                            ArithmeticOperationsCommand.Execute("%");
                                        break;
                                    case Key.Return:
                                        EqualCommand.Execute(null);
                                        break;
                                    case Key.Back:
                                        BackCommand.Execute(null);
                                        break;
                                    case Key.Delete:
                                        ClearCommand.Execute(null);
                                        break;
                                };
                            }*/
                        });
                return _keyDownCommand;
            }
        }

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        public ICommand? CloseParenthesisOperationsCommand => null;
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

        private ICommand? _functionCommand;
        public ICommand FunctionCommand
        {
            get
            {
                if (_functionCommand == null)
                    _functionCommand = new Command<object>(
                        (object o) =>
                        {
                            if (operatorEqualFlag)
                            {
                                previewValue *= -1;
                                ShowValue = previewValue.ToString();
                            }
                            else
                            {
                                currentValue = currentValue * -1;
                                ShowValue = currentValue.ToString();
                            }

                        });
                return _functionCommand;
            }
        }

#pragma warning disable CS8603 // Possible null reference return.
        public ICommand OpenParenthesisOperationsCommand => null;
#pragma warning restore CS8603 // Possible null reference return.

        public CalculatorRegularViewModel()
        {
            ShowValue = currentValue.ToString();
        }

        private long CalculatePreviewOperation()
        {
            if (previewOperator == "+")
                return previewValue + currentValue;
            else if (previewOperator == "-")
                return previewValue - currentValue;
            else if (previewOperator == "*")
                return previewValue * currentValue;
            else if (previewOperator == "/")
                return previewValue / currentValue;
            else if (previewOperator == "%")
                return previewValue % currentValue;
            else if (previewOperator == "^")
                return (int)Math.Pow(previewValue, currentValue);

            return 0;
        }

        public void Dispose()
        {

        }
    }
}
