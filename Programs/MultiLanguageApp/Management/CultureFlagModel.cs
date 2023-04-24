using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultiLanguageApp.Management
{
    class CultureFlagModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }
        private string _cultureName;
        public string CultureName
        {
            get { return _cultureName; }
            set { _cultureName = value; }
        }

        private bool _chooseFlag;
        public bool ChooseFlag
        {
            get { return _chooseFlag; }
            set
            {
                _chooseFlag = value;
                OnPropertyChanged(nameof(ChooseFlag));
            }
        }

        public ICommand ChangeCultureCommand { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
