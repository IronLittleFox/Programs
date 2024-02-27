using ChessMauiGame.ViewModel;
using UtilsMaui.Interfaces;

namespace ChessMauiGame.View;

public partial class ChessView : ContentView, IDisposableGameView
{
	public ChessView(ChessViewModel vm)
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