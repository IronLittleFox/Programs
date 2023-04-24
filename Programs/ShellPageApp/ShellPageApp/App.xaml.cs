﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShellPageApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ShellApp();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
