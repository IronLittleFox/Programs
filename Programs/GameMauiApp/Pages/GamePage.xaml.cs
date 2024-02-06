using GameMauiApp.ViewModel;
using UtilsMaui.Interfaces;

namespace GameMauiApp.Pages;

public partial class GamePage : ContentPage
{
	public GamePage(GameViewModel vm)
	{
        BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnDisappearing()
    {
        IGameViewModel gameViewModel = BindingContext as IGameViewModel;
        if (gameViewModel != null)
        {
            gameViewModel.Dispose();
        }
        base.OnDisappearing();
    }
}