using GoMauiGame.ViewModel;
using UtilsMaui.Interfaces;

namespace GoMauiGame.View;

public partial class GoView : ContentView, IDisposableGameView
{
	public GoView(GoViewModel vm)
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