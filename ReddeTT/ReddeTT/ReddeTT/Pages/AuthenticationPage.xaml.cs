using System;
using System.Text.RegularExpressions;
using Plugin.Settings;
using Xamarin.Forms;

namespace ReddeTT.Pages
{
    public partial class AuthenticationPage : ContentPage
    {
        public AuthenticationPage()
        {
            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var source = new UrlWebViewSource
            {
                Url = App.ReddeTT.CodeAuthenticationUri.ToString()
            };
            this.WebView.Source = source;
            this.WebView.Navigated += WebView_Navigated;
        }

        private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url.StartsWith("https://www.reddit.com/"))
            {

            }
            else if (e.Url.StartsWith("ReddeTT://callback"))
            {
                var code = Regex.Split(e.Url, "code=")[1];
                CrossSettings.Current.AddOrUpdateValue("code", code);
                await Navigation.PopModalAsync(true);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MainPage.Mainpage.AuthenticationModalClosed();
        }
    }
}