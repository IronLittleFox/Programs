using Microsoft.Win32;
//using Microsoft.Win32;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace PhotoViewerMVVM
{
    class DialogServices
    {
        public List<string> GetListOfImages()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.FileName = "Folder Selection.";
            var result = dialog.ShowDialog();
            if (result == true)
            {
                var folder = Path.GetDirectoryName(dialog.FileName);
                return Directory.GetFiles(folder, "*.jpg")
                                      .Concat(Directory.GetFiles(folder, "*.png"))
                                      .Concat(Directory.GetFiles(folder, "*.bmp"))
                                      .ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        public void ShowMessege(string message)
        {
            MessageBox.Show(message);
        }

    }
}
