using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using UtilsWPF;

namespace DrowLineWpfApp
{
    class ViewModel: ObserverVM
    {
        private readonly Canvas _canvas;
        public ViewModel(Canvas canvas)
        {
            _canvas = canvas;
        }


        public ObservableCollection<Color> Kolory { get; } = new ObservableCollection<Color>
        {
            Colors.Black,
            Colors.Red,
            Colors.Green,
            Colors.Blue,
            Colors.Yellow,
            Colors.Purple,
            Colors.Orange
        };

        private Color _wybranyKolor = Colors.Black;
        public Color WybranyKolor
        {
            get => _wybranyKolor;
            set
            {
                _wybranyKolor = value;
                OnPropertyChanged();
            }
        }

        private int _gruboscLinii = 1;
        public int GruboscLinii
        {
            get => _gruboscLinii;
            set
            {
                _gruboscLinii = value;
                OnPropertyChanged();
            }
        }

        private ICommand _zapiszRysunekCommand;
        public ICommand ZapiszRysunekCommand => _zapiszRysunekCommand ?? (_zapiszRysunekCommand = new RelayCommand<object>(
            (o) =>
            {
                var dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    var document = new XDocument();
                    var elementRoot = new XElement("Rysunek");
                    foreach (var element in _canvas.Children)
                    {
                        if (element is Line line)
                        {
                            var elementLine = new XElement("Linia",
                                new XAttribute("Kolor", ((SolidColorBrush)line.Stroke).Color),
                                new XAttribute("Grubosc", line.StrokeThickness),
                                new XAttribute("X1", line.X1),
                                new XAttribute("Y1", line.Y1),
                                new XAttribute("X2", line.X2),
                                new XAttribute("Y2", line.Y2));
                            elementRoot.Add(elementLine);
                        }
                    }
                    document.Add(elementRoot);
                    document.Save(dialog.FileName);
                }
            }));

        private ICommand _otworzRysunekCommand;
        public ICommand OtworzRysunekCommand => _otworzRysunekCommand ?? (_otworzRysunekCommand = new RelayCommand<object>(
            (o) =>
            {
                var dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    _canvas.Children.Clear();
                    var document = XDocument.Load(dialog.FileName);
                    var elementRoot = document.Root;
                    if (elementRoot != null)
                    {
                        foreach (var element in elementRoot.Elements())
                        {
                            if (element.Name == "Linia")
                            {
                                var line = new Line
                                {
                                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(element.Attribute("Kolor").Value)),
                                    StrokeThickness = (double)element.Attribute("Grubosc"),
                                    X1 = (double)element.Attribute("X1"),
                                    Y1 = (double)element.Attribute("Y1"),
                                    X2 = (double)element.Attribute("X2"),
                                    Y2 = (double)element.Attribute("Y2")
                                };
                                _canvas.Children.Add(line);
                            }
                        }
                    }
                }
            }));
    }
}
