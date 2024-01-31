using CommunityToolkit.Maui.Views;
using TicTacToeMauiGame.ViewModel;

namespace TicTacToeMauiGame.View;

public partial class TicTacToePopupPage : Popup
{
	public TicTacToePopupPage(TicTacToePopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}