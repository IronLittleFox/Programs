using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiKonwersje
{
    /// <summary>
    /// doubleeraction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double redComponent;
        public double RedComponent
        {
            get 
            { 
                return redComponent; 
            }
            set 
            { 
                redComponent = value;
                OnPropertyChanged(nameof(RedComponent));
            }
        }

        private double greenComponent;
        public double GreenComponent
        {
            get
            {
                return greenComponent;
            }
            set
            {
                greenComponent = value;
                OnPropertyChanged(nameof(GreenComponent));
            }
        }

        private double blueComponent;
        public double BlueComponent
        {
            get
            {
                return blueComponent;
            }
            set
            {
                blueComponent = value;
                OnPropertyChanged(nameof(BlueComponent));
            }
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            RedComponent = 255;
            GreenComponent = 0;
            BlueComponent = 0;
        }
    }
}
