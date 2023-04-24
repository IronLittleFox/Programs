using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FlyoutPageApp
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            //flyoutMenuMage.menuListView.ItemSelected += OnSelectedItem;
        }

        private void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as FlyoutItemPage;
            if (item!= null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetPage));
                //flyoutMenuMage.menuListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
