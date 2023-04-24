using System;
using System.Collections.Generic;
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

namespace DrowLineWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel _vm;
        private Line _linia;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel(canvas);
            _vm = (ViewModel)DataContext;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _linia = new Line
            {
                Stroke = new SolidColorBrush(_vm.WybranyKolor),
                StrokeThickness = _vm.GruboscLinii
            };
            _linia.X1 = e.GetPosition(canvas).X;
            _linia.Y1 = e.GetPosition(canvas).Y;
            _linia.X2 = e.GetPosition(canvas).X;
            _linia.Y2 = e.GetPosition(canvas).Y;
            canvas.Children.Add(_linia);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _linia != null)
            {
                _linia.X2 = e.GetPosition(canvas).X;
                _linia.Y2 = e.GetPosition(canvas).Y;
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _linia = null;
        }

        private void canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
