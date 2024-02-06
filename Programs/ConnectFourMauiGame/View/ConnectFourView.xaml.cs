using ConnectFourMauiGame.ViewModel;
using UtilsMaui.Interfaces;

namespace ConnectFourMauiGame.View;

public partial class ConnectFourView : ContentView, IDisposableGameView
{
    public ConnectFourView(ConnectFourViewModel vm)
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