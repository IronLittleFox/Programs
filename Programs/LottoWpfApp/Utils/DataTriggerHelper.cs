using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LottoWpfApp.Utils
{
    public class DataTriggerHelper
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached(
                "Value",
                typeof(object),
                typeof(DataTriggerHelper),
                new FrameworkPropertyMetadata(0, OnValueChanged));

        public static object GetValue(DependencyObject d)
        {
            return d.GetValue(ValueProperty);
        }

        public static void SetValue(DependencyObject d, object value)
        {
            d.SetValue(ValueProperty, value);
        }

        public static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (d is DataTrigger trigger)
                trigger.Value = args.NewValue;
        }
    }
}
