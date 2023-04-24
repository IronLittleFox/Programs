using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CarouselPageApp
{
    public partial class MainPage : CarouselPage
    {
        ObservableCollection<PhotoInfo> PhotoInfoList;
        public MainPage()
        {
            InitializeComponent();
            PhotoInfoList = new ObservableCollection<PhotoInfo>();
            PhotoInfoList.Add(new PhotoInfo()
            {
                Name = "Pies",
                PhotoUrl = "https://img.freepik.com/darmowe-wektory/zmeczony-mlody-pies_1308-25268.jpg"
            });
            PhotoInfoList.Add(new PhotoInfo()
            {
                Name = "Piesek",
                PhotoUrl = "https://img.freepik.com/darmowe-wektory/ladny-pies-ugryzienie-kosci-kreskowka-wektor-ikona-ilustracja-koncepcja-ikona-zywnosci-zwierzat-na-bialym-tle-premium-wektor-plaski-styl-kreskowki_138676-4161.jpg"
            });
            PhotoInfoList.Add(new PhotoInfo()
            {
                Name = "Kotki",
                PhotoUrl = "https://img.freepik.com/darmowe-wektory/ladny-kot-i-pies-corgi-kreskowka-wektor-ikona-ilustracja-koncepcja-ikona-przyjaciela-zwierzat-na-bialym-tle-premium-wektor-plaski-styl-kreskowki_138676-3558.jpg"
            });
            ItemsSource = PhotoInfoList;
        }
    }
}
