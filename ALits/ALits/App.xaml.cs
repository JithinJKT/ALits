﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ALits.ViewPages;

namespace ALits
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainTabPage();
           
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
