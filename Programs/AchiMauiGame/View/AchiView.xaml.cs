using AchiMauiGame.ViewModel;
using UtilsMaui.Interfaces;

namespace AchiMauiGame.View;

public partial class AchiView : ContentView, IDisposableGameView
{
	public AchiView(AchiViewModel vm)
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