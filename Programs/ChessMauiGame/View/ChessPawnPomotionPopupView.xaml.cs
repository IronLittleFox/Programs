using ChessMauiGame.ViewModel;
using CommunityToolkit.Maui.Views;

namespace ChessMauiGame.View;

public partial class ChessPawnPomotionPopupView : Popup
{
	public ChessPawnPomotionPopupView(ChessPawnPomotionPopupViewModel vm)
	{
		BindingContext = vm;
        vm.OnClose += async result => await CloseAsync(result, CancellationToken.None);
        InitializeComponent();
	}
}