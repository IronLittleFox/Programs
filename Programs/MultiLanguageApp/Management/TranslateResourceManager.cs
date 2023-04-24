using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace MultiLanguageApp.Management
{
    public class TranslateResourceManager : INotifyPropertyChanged
    {
        private ResourceDictionary dict;
        private TranslateResourceManager()
        {
            InitListOfTranslateResources();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
            SetTranslateDictionary();
        }

        public static TranslateResourceManager Instance { get; } = new();

        public object this[string resourceKey] => dict[resourceKey] ?? "";

        public event PropertyChangedEventHandler PropertyChanged;

        private static Dictionary<string, string> listOfTranslateResources;

        public void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            SetTranslateDictionary();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        private static void InitListOfTranslateResources()
        {
            listOfTranslateResources = new Dictionary<string, string>();
            ResourceDictionary resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri("pack://application:,,,/MultiLanguageApp;Component/Translation/TranslationDictionary.xaml", UriKind.Absolute);
            foreach (DictionaryEntry dictionaryEntry in resourceDictionary)
            {
                listOfTranslateResources.Add(dictionaryEntry.Key.ToString().ToLower(), dictionaryEntry.Value.ToString());
            }
        }

        private void SetTranslateDictionary()
        {
            dict = new ResourceDictionary();

            string currentCultureName = Thread.CurrentThread.CurrentCulture.ToString().ToLower();
            if (listOfTranslateResources.ContainsKey(currentCultureName))
            {
                dict.Source = new Uri(listOfTranslateResources[currentCultureName], UriKind.Absolute);
            }
            else
                throw new Exception("Brak pliku z językiem " + currentCultureName);
        }
    }
}
