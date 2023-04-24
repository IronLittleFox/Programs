using PracticalExamApp.Validation;
using PracticalExamApp.Validation.TypesOfValidation;
using PracticalExamApp.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PracticalExamApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FirstErrorVM FirstErrorVM { get; set; }
        public AllErrorsVM AllErrorsVM { get; set; }
        public OnlyDataVM OnlyDataVM { get; set; }

        public MainWindow()
        {
            FirstErrorVM = new FirstErrorVM();
            AllErrorsVM = new AllErrorsVM();
            OnlyDataVM = new OnlyDataVM();
            InitializeComponent();
        }

        private int ConvertAgeStringToInt(string strAge)
        {
            return int.Parse(strAge);
        }

        private void buttonCheck_Clicked(object sender, EventArgs e)
        {
            string name = entryName.Text;
            string strAge = entryAge.Text;
            string adres = "ulica Majowa 7, kielce";
            Validate validate = new Validate();

            validate.AddValidator(new Validator<string>(name, "Imie",
                new List<ISpecyficValidation<string>>()
                {
                    new ValidateStringEmpty()
                }));

            validate.AddValidator(new Validator<string>(strAge, "Wiek",
                new List<ISpecyficValidation<string>>()
                {
                    new ValidateStringEmpty(),
                    new ValidateStringIsNumber(),
                    new ValidateStringNumberIsInRange(1,150)
                }));

            validate.AddValidator(new Validator<string>(adres, "Adres",
                new List<ISpecyficValidation<string>>()
                {
                    new ValidateStringEmpty(),
                    //new ValidateAdressExists()
                }));

            if (!validate.Validation(out string message))
            {
                labelHello.Text = message;
                labelLegalAge.Text = "";
                return;
            }

            labelHello.Text = "Witaj " + name;
            labelLegalAge.Text = ConvertAgeStringToInt(strAge) >= 18 ? "Pełnoletni" : "Niepełnoletni";
        }
    }
}
