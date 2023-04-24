using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MultiLanguageApp.Management
{
    class FlagExtenction : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            var targetElement = targetProvider.TargetObject as FrameworkElement;
            var targetProperty = targetProvider.TargetProperty as DependencyProperty;

            if (targetElement != null && targetProperty != null)
            {
                targetElement.DataContextChanged += (sender, args) => ApplyBinding(targetElement, targetProperty, args.NewValue);

                var binding = ApplyBinding(targetElement, targetProperty, targetElement.DataContext);

                return binding.ProvideValue(serviceProvider);
            }
            else
            {
                return null;
            }
        }

        private Binding ApplyBinding(DependencyObject target, DependencyProperty property, object source)
        {
            BindingOperations.ClearBinding(target, property);

            var binding = new Binding()
            {
                Mode = BindingMode.OneWay,
                Source = FlagResourceManager.Instance.FlagCollection
            };

            BindingOperations.SetBinding(target, property, binding);
            return binding;
        }
    }
}
