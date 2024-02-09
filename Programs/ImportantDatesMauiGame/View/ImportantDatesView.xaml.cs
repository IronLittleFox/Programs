using ImportantDatesMauiGame.ViewModel;
using UtilsMaui.Interfaces;

namespace ImportantDatesMauiGame.View;

public partial class ImportantDatesView : ContentView, IDisposableGameView
{
	public ImportantDatesView(ImportantDatesViewModel vm)
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