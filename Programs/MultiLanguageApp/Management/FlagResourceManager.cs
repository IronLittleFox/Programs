using MultiLanguageApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace MultiLanguageApp.Management
{
    class FlagResourceManager : INotifyPropertyChanged
    {
        private ResourceDictionary resourceDictionary;
        public event PropertyChangedEventHandler PropertyChanged;

        public static FlagResourceManager Instance { get; } = new();

        public ObservableCollection<CultureFlagModel> FlagCollection { get; set; }

        private ICommand changeCultureCommand;
        public ICommand ChangeCultureCommand
        {
            get
            {
                if (changeCultureCommand == null)
                    changeCultureCommand = new RelayCommand<CultureFlagModel>(
                        o =>
                        {
                            Instance.SetFlag(o.CultureName);
                            TranslateResourceManager.Instance.SetCulture(new System.Globalization.CultureInfo(o.CultureName));
                        }
                        );
                return changeCultureCommand;
            }
        }

        public FlagResourceManager()
        {
            FlagCollection = new ObservableCollection<CultureFlagModel>();
            resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri("pack://application:,,,/MultiLanguageApp;Component/Flag/FlagResource.xaml", UriKind.Absolute);
            foreach (DictionaryEntry item in resourceDictionary)
            {
                FlagCollection.Add(new CultureFlagModel()
                {
                    CultureName = item.Key.ToString(),
                    FilePath = item.Value.ToString(),
                    ChooseFlag = false,
                    ChangeCultureCommand = ChangeCultureCommand
                });
            }

            string currentCultureName = Thread.CurrentThread.CurrentCulture.ToString().ToLower();
            CultureFlagModel cultureFlagModel = FlagCollection.FirstOrDefault(cfm => cfm.CultureName == currentCultureName);
            if (cultureFlagModel == null)
                throw new Exception("Brak falgi " + currentCultureName);
            cultureFlagModel.ChooseFlag = true;
        }

        private void SetFlag(string cultureName)
        {
            FlagCollection.ForAll(cfm => cfm.ChooseFlag = false);
            CultureFlagModel cultureFlagModel = FlagCollection.FirstOrDefault(cfm => cfm.CultureName == cultureName);
            if (cultureFlagModel == null)
                throw new Exception("Brak falgi " + cultureName);
            cultureFlagModel.ChooseFlag = true;
        }
    }
}
