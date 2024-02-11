using CalculatorMauiGame.ViewModel;
using UtilsMaui.Interfaces;

namespace CalculatorMauiGame.View;

public partial class MainCalculatorView : ContentView, IDisposableGameView
{
    public MainCalculatorView(MainCalculatorViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }

    public void Dispose()
    {
        IGameViewModel? gameViewModel = BindingContext as IGameViewModel;
        if (gameViewModel != null)
        {
            gameViewModel.Dispose();
        }
    }
}