using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Drawing;
using SkiaSharp;

namespace UtilsMaui.Utils
{
    public static class AutoTextSize
    {
        public static readonly BindableProperty EnableProperty =
        BindableProperty.CreateAttached(
            "Enable",    // Nazwa właściwości
            typeof(bool),          // Typ właściwości
            typeof(AutoTextSize), // Typ, do którego właściwość jest dołączana
            false,                 // Wartość domyślna
            propertyChanged: OnEnableChanged);

        public static bool GetEnable(BindableObject view)
        {
            return (bool)view.GetValue(EnableProperty);
        }

        public static void SetEnable(BindableObject view, bool value)
        {
            view.SetValue(EnableProperty, value);
        }

        private static void OnEnableChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PropertyInfo? propertyInfo = bindable?.GetType()?.GetProperty("Text");

            if (propertyInfo == null)
                return;

            if ((bool) newValue)
            {
                bindable.PropertyChanged -= Bin_PropertyChanged;
                bindable.PropertyChanged += Bin_PropertyChanged;
            }
            else
                bindable.PropertyChanged -= Bin_PropertyChanged;
        }

        private static void Bin_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender == null
                || e.PropertyName == "Window"
                || e.PropertyName == "Parent"
                || e.PropertyName == "FontSize")
            {
                return;
            }
            
            PropertyInfo? propertyTextInfo = sender?.GetType()?.GetProperty("Text");

            if (propertyTextInfo == null)
                return;

            string? textValue = propertyTextInfo.GetValue(sender)?.ToString();

            if (textValue == null)
                return;

            PropertyInfo? propertyFontFamilyInfo = sender?.GetType()?.GetProperty("FontFamily");
            if (propertyFontFamilyInfo == null)
                return;

            string? fontFamily = propertyFontFamilyInfo.GetValue(sender)?.ToString();
            if (fontFamily == null)
                return;

            PropertyInfo? propertyWidthInfo = sender?.GetType()?.GetProperty("Width");
            if (propertyWidthInfo == null)
                return;

            if (!double.TryParse(propertyWidthInfo.GetValue(sender)?.ToString(), out double width)
                || width == -1)
                return;

            PropertyInfo? propertyHeightInfo = sender?.GetType()?.GetProperty("Height");
            if (propertyHeightInfo == null)
                return;

            if (!double.TryParse(propertyHeightInfo.GetValue(sender)?.ToString(), out double height)
                || height == -1)
                return;

            PropertyInfo? propertyPaddingInfo = sender?.GetType()?.GetProperty("Padding");
            if (propertyPaddingInfo == null)
                return;

            if (!(propertyPaddingInfo.GetValue(sender) is Thickness padding))
            {
                return;
            }

            width -= padding.Left + padding.Right;
            height -= padding.Top + padding.Bottom;

            SKSize size = new SKSize((float)width, (float)height);

             var d = ZnajdzOdpowiedniRozmiarCzcionki(textValue, fontFamily, size);
            if (d > 0)
            {
                PropertyInfo? propertyFontSizeInfo = sender?.GetType()?.GetProperty("FontSize");
                if (propertyFontSizeInfo == null)
                    return;

                var g = propertyFontSizeInfo.GetValue(sender);

                propertyFontSizeInfo.SetValue(sender, d);

                g = propertyFontSizeInfo.GetValue(sender);

                if (g != null)
                {

                }
            }
        }

        public static float ZnajdzOdpowiedniRozmiarCzcionki(string tekst, string fontFamily, SKSize miejsce)
        {
            float minRozmiar = 1; // Minimalny rozmiar czcionki
            float maxRozmiar = 100; // Maksymalny rozmiar czcionki
            float aktualnyRozmiar = 1; // Rozmiar czcionki startowy

            while (true)
            {
                SKPaint paint = new SKPaint
                {
                    TextSize = aktualnyRozmiar,
                    Typeface = SKTypeface.FromFamilyName(fontFamily)
                };
                SKRect rozmiar = new SKRect();
                paint.MeasureText(tekst, ref rozmiar);

                if (rozmiar.Width > miejsce.Width || rozmiar.Height > miejsce.Height)
                {
                    maxRozmiar = aktualnyRozmiar;
                    aktualnyRozmiar = (minRozmiar + maxRozmiar) / 2;
                    if (Math.Abs(maxRozmiar - minRozmiar) < 0.1)
                        return aktualnyRozmiar;
                }
                else
                {
                    minRozmiar = aktualnyRozmiar;
                    aktualnyRozmiar = (minRozmiar + maxRozmiar) / 2;
                    if (Math.Abs(maxRozmiar - minRozmiar) < 0.1)
                        return aktualnyRozmiar;
                }
            }
        }

    }
}
