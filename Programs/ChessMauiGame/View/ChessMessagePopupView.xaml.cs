using ChessMauiGame.ViewModel;
using CommunityToolkit.Maui.Views;

namespace ChessMauiGame.View;

public partial class ChessMessagePopupView : Popup
{
	public ChessMessagePopupView(ChessMessagePopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}