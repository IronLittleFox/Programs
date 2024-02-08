using SudokuMauiGame.ViewModel;
using UtilsMaui.Interfaces;

namespace SudokuMauiGame.View;

public partial class SudokuView : ContentView, IDisposableGameView
{
	public SudokuView(SudokuViewModel vm)
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