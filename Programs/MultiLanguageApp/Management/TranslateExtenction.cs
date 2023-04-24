using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MultiLanguageApp.Management
{
    [ContentProperty(nameof(Name))]
    class TranslateExtenction : MarkupExtension
    {
        public string Name { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            var targetElement = targetProvider.TargetObject as FrameworkElement;
            var targetProperty = targetProvider.TargetProperty as DependencyProperty;

            if (!string.IsNullOrEmpty(Name)
                &&  targetElement != null
                && targetProperty != null)
            {
                targetElement.DataContextChanged += (sender, args) => ApplyBinding(targetElement, targetProperty, args.NewValue);

                var binding = ApplyBinding(targetElement, targetProperty, targetElement.DataContext);

                return binding.ProvideValue(serviceProvider);
            }
            else
            {
                return "";
            }
        }

        private Binding ApplyBinding(DependencyObject target, DependencyProperty property, object source)
        {
            BindingOperations.ClearBinding(target, property);

            var binding = new Binding($"[{Name}]")
            {
                Mode = BindingMode.OneWay,
                Source = TranslateResourceManager.Instance
            };

            BindingOperations.SetBinding(target, property, binding);
            return binding;
        }
    }
}
