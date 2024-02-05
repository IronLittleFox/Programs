using MinesweeperMauiGame.ViewModel;
using UtilsMaui.Interfaces;

namespace MinesweeperMauiGame.View;

public partial class MinesweeperView : ContentView, IDisposableGameView
{
	public MinesweeperView(MinesweeperViewModel vm)
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