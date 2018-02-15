﻿using ReddeTT.Pages;
using Xamarin.Forms;

namespace ReddeTT
{
    public partial class App : Application
    {
        public static API.ReddeTT ReddeTT;
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
