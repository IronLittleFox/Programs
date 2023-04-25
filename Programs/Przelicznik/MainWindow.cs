using Przelicznik.Database.Context;
using Przelicznik.Database.Entities;
using Przelicznik.Database.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Przelicznik
{
    public partial class MainWindow : Form
    {
        private IConverterRepository converterRepository;

        public MainWindow()
        {
            InitializeComponent();

            converterRepository = new ConverterDbRepository();

            List<UnitType> listOfUnitType = converterRepository.ReadAllUnitType();

            comboBoxUnitType.SelectedIndexChanged -= comboBoxUnitType_SelectedIndexChanged;

            comboBoxUnitType.DataSource = listOfUnitType;
            comboBoxUnitType.DisplayMember = "Name";

            comboBoxUnitType.SelectedIndexChanged += comboBoxUnitType_SelectedIndexChanged;
            comboBoxUnitType_SelectedIndexChanged(null, null);

            /* List<UnitType> listOfUnitType = converterDbRepository.ReadAllUnitType();
             foreach (UnitType element in listOfUnitType)
                 comboBoxUnitType.Items.Add(element.Name);*/
            /*
             * 
                select *
                  from Units u
                  join UnitTypes ut on ut.id = u.UnitTypeId
                 where ut.Name = "Masa"


             */
        }

        private void comboBoxUnitType_SelectedIndexChanged(object sender, EventArgs e)
        {

            UnitType selectedUnitType = comboBoxUnitType.SelectedItem as UnitType;

            List<Unit> listOfSourceUnits = converterRepository.ReadChosenUnit(selectedUnitType.Id);
            List<Unit> listOfTargetUnits = converterRepository.ReadChosenUnit(selectedUnitType.Id);

            comboBoxSourceUnit.DataSource = listOfSourceUnits;
            comboBoxSourceUnit.DisplayMember = "Name";

            comboBoxTargetUnit.DataSource = listOfTargetUnits;
            comboBoxTargetUnit.DisplayMember = "Name";
        }

        private void buttonConvertValue_Click(object sender, EventArgs e)
        {
            Unit sourceUnit = comboBoxSourceUnit.SelectedItem as Unit;
            Unit targetUnit = comboBoxTargetUnit.SelectedItem as Unit;

            UnitConverter unitConverter 
                = converterRepository.ReadUnitConverter(sourceUnit.Id, targetUnit.Id);

            decimal targetValue = numericUpDownValue.Value * (decimal)unitConverter.ConvertValue;

            labelResult.Text = numericUpDownValue.Value + sourceUnit.Symbol 
                + " = " + targetValue + targetUnit.Symbol;
        }
    }
}
