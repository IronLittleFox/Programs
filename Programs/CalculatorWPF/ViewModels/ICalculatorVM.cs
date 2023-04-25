using System.ComponentModel;
using System.Windows.Input;

namespace CalculatorWPF.ViewModels
{
    interface ICalculatorVM
    {
        ICommand ArithmeticOperationsCommand { get; }
        ICommand BackCommand { get; }
        ICommand ClearCommand { get; }
        ICommand CloseParenthesisOperationsCommand { get; }
        ICommand EqualCommand { get; }
        ICommand FunctionCommand { get; }
        bool IsParenthesisAvailable { get; set; }
        ICommand KeyDownCommand { get; }
        ICommand NumberCommand { get; }
        ICommand OpenParenthesisOperationsCommand { get; }
        string ShowValue { get; set; }
        string NameOfViewModel { get; set; }
    }
}