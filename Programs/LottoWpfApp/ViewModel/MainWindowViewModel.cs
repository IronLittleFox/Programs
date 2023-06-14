using LottoWpfApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilsWPF;

namespace LottoWpfApp.ViewModel
{
    public class MainWindowViewModel : ObserverVM
    {
        private int numberOfNumbersToChoose;
        public int NumberOfNumbersToChoose
        {
            get { return numberOfNumbersToChoose; }
            set
            {
                numberOfNumbersToChoose = value;
                OnPropertyChanged(nameof(NumberOfNumbersToChoose));
            }
        }

        private int minRangeOfNumbers;
        public int MinRangeOfNumbers
        {
            get { return minRangeOfNumbers; }
            set
            {
                minRangeOfNumbers = value;
                OnPropertyChanged(nameof(MinRangeOfNumbers));
            }
        }

        private int maxRangeOfNumbers;
        public int MaxRangeOfNumbers
        {
            get { return maxRangeOfNumbers; }
            set
            {
                maxRangeOfNumbers = value;
                OnPropertyChanged(nameof(MaxRangeOfNumbers));
            }
        }

        private ICommand selectNumberCommand;
        public ICommand SelectNumberCommand
        {
            get
            {
                if (selectNumberCommand == null)
                    selectNumberCommand = new RelayCommand<NumberToSelectModel>(
                        o =>
                        {
                            if (o.IsSelect)
                                CollectionOfSelectNumbers.Remove(o);
                            else
                                CollectionOfSelectNumbers.Add(o);
                            o.IsSelect = !o.IsSelect;
                        }
                        );
                return selectNumberCommand;
            }
        }

        public ObservableCollection<NumberToSelectModel> CollectionOfNumbers { get; set; } = new ObservableCollection<NumberToSelectModel>();

        public ObservableCollection<NumberToSelectModel> CollectionOfSelectNumbers { get; set; } = new ObservableCollection<NumberToSelectModel>();

        public ObservableCollection<ObservableCollection<int>> ColectionOfSelectedNumbers { get; set; } = new ObservableCollection<ObservableCollection<int>>();

        private ICommand addSelectedNumbersCommand;
        public ICommand AddSelectedNumbersCommand
        {
            get
            {
                if (addSelectedNumbersCommand == null)
                    addSelectedNumbersCommand = new RelayCommand<object>(
                        o =>
                        {
                            ObservableCollection<int> newCollection = new ObservableCollection<int>();
                            foreach (var item in CollectionOfSelectNumbers)
                            {
                                newCollection.Add(item.Number);
                                item.IsSelect = false;
                            }
                            ColectionOfSelectedNumbers.Add(newCollection);
                            CollectionOfSelectNumbers.Clear();
                        }
                        );
                return addSelectedNumbersCommand;
            }
        }

        private ICommand drawNumbersCommand;
        public ICommand DrawNumbersCommand
        {
            get
            {
                if (drawNumbersCommand == null)
                    drawNumbersCommand = new RelayCommand<object>(
                        o =>
                        {
                            DrawNumbers.Clear();
                            Random random = new Random();
                            foreach (NumberToSelectModel item in CollectionOfNumbers.OrderBy(x=> random.Next()).Take(NumberOfNumbersToChoose))
                            {
                                DrawNumbers.Add(item.Number);
                            }
                            
                        }
                        );
                return drawNumbersCommand;
            }
        }

        public ObservableCollection<int> DrawNumbers { get; set; } = new ObservableCollection<int>();

        public MainWindowViewModel()
        {
            MinRangeOfNumbers = 1;
            MaxRangeOfNumbers = 49;
            NumberOfNumbersToChoose = 6;
            for (int i = MinRangeOfNumbers; i <= MaxRangeOfNumbers; i++)
            {
                CollectionOfNumbers.Add(new NumberToSelectModel()
                {
                    Number = i,
                    IsSelect = false,
                    SelectNumberCommand = SelectNumberCommand
                });
            };
        }
    }
}
