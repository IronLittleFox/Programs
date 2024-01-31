using CommunityToolkit.Maui.Core;
using UtilsMaui.Interfaces;

namespace TicTacToeMauiGame.View;
using TicTacToeMauiGame.ViewModel;

public partial class TicTacToeView : ContentView, IDisposableGameView
{
	public TicTacToeView(TicTacToeViewModel vm)
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