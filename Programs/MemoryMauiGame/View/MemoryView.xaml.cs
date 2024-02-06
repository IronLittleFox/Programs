using MemoryMauiGame.ViewModel;
using UtilsMaui.Interfaces;

namespace MemoryMauiGame.View;

public partial class MemoryView : ContentView, IDisposableGameView
{
	public MemoryView(MemoryViewModel vm)
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