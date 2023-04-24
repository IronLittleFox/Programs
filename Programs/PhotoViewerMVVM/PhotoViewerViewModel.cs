using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using UtilsWPF;

namespace PhotoViewerMVVM
{
    class PhotoViewerViewModel: ObserverVM
    {
        public PhotoViewerViewModel(DialogServices dialogServices)
        {
            ListOfImagesUrl = new List<string>();
            currentNumberOfImage = 0;
            this.dialogServices = dialogServices;
        }

        private int currentNumberOfImage = 0;
        private readonly DialogServices dialogServices;

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                OnPropertyChanged();
            }
        }

        private List<string> _listOfImagesUrl;
        public List<string> ListOfImagesUrl
        {
            get => _listOfImagesUrl;
            set
            {
                _listOfImagesUrl = value;
                OnPropertyChanged();
            }
        }

        private ICommand _loadImageCommand;
        public ICommand LoadImageCommand
        {
            get
            {
                if (_loadImageCommand == null)
                    _loadImageCommand = new RelayCommand<object>(
                        o =>
                        {
                            List<string> list = dialogServices.GetListOfImages();
                            if (list.Count == 0)
                            {
                                dialogServices.ShowMessege("Nie ma zdjęć");
                            }
                            else
                            {
                                ListOfImagesUrl = list;
                                currentNumberOfImage = 0;
                                ImageUrl = ListOfImagesUrl[currentNumberOfImage];
                            }

                            /*OpenFileDialog dialog = new OpenFileDialog();
                            dialog.ValidateNames = false;
                            dialog.CheckFileExists = false;
                            dialog.CheckPathExists = true;
                            dialog.FileName = "Folder Selection.";
                            var result = dialog.ShowDialog();
                            if (result == true)
                            {
                                var folder = Path.GetDirectoryName(dialog.FileName);
                                ListOfImagesUrl = Directory.GetFiles(folder, "*.jpg")
                                                      .Concat(Directory.GetFiles(folder, "*.png"))
                                                      .Concat(Directory.GetFiles(folder, "*.bmp"))
                                                      .ToList();
                                currentNumberOfImage = 0;
                                ImageUrl = ListOfImagesUrl[currentNumberOfImage];
                            }
                            else
                            {

                            }*/
                        }
                        );
                return _loadImageCommand;
            }
        }

        private ICommand _nextImageCommand;
        public ICommand NextImageCommand
        {
            get
            {
                if (_nextImageCommand == null)
                    _nextImageCommand = new RelayCommand<object>(
                        o =>
                        {
                            currentNumberOfImage = ++currentNumberOfImage % ListOfImagesUrl.Count;
                            ImageUrl = ListOfImagesUrl[currentNumberOfImage];
                        }
                        );
                return _nextImageCommand;
            }
        }

        private ICommand _prevImageCommand;
        public ICommand PrevImageCommand
        {
            get
            {
                if (_prevImageCommand == null)
                    _prevImageCommand = new RelayCommand<object>(
                        o =>
                        {
                            currentNumberOfImage--;
                            if (currentNumberOfImage < 0)
                                currentNumberOfImage = ListOfImagesUrl.Count-1;
                            ImageUrl = ListOfImagesUrl[currentNumberOfImage];
                        }
                        );
                return _prevImageCommand;
            }
        }


    }
}
