﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShellPageApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArchHistoryPage : ContentPage
    {
        public ArchHistoryPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopAsync();
        }
    }
}