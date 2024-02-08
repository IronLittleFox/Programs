using CheckersMauiGame.ViewModel;
using CommunityToolkit.Maui.Views;

namespace CheckersMauiGame.View;

public partial class CheckersPopupView : Popup
{
	public CheckersPopupView(CheckersPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}