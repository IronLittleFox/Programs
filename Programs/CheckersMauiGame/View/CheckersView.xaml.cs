using CheckersMauiGame.ViewModel;
using UtilsMaui.Interfaces;

namespace CheckersMauiGame.View;

public partial class CheckersView : ContentView, IDisposableGameView
{
	public CheckersView(CheckersViewModel vm)
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