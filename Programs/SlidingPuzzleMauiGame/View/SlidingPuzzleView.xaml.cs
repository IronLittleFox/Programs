using SlidingPuzzleMauiGame.ViewModel;
using UtilsMaui.Interfaces;

namespace SlidingPuzzleMauiGame.View;

public partial class SlidingPuzzleView : ContentView, IDisposableGameView
{
	public SlidingPuzzleView(SlidingPuzzleViewModel vm)
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