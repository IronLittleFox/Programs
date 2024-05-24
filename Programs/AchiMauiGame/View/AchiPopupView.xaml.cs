using AchiMauiGame.ViewModel;
using CommunityToolkit.Maui.Views;

namespace AchiMauiGame.View;

public partial class AchiPopupView : Popup
{
	public AchiPopupView(AchiPopupViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}