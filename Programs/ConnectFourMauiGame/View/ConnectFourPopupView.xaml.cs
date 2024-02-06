using CommunityToolkit.Maui.Views;
using ConnectFourMauiGame.ViewModel;

namespace ConnectFourMauiGame.View;

public partial class ConnectFourPopupView : Popup
{
	public ConnectFourPopupView(ConnectFourPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}