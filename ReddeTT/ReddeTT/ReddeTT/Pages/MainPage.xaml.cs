using System.Threading.Tasks;
using ReddeTT.Pages.Initialization;
using Xamarin.Forms;

namespace ReddeTT.Pages
{
    public partial class MainPage : ContentPage
    {
        public static MainPage Mainpage;
        public MainPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (System.Exception ex)
            {

            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Mainpage = this;
            App.ReddeTT = new API.ReddeTT();
            if (!App.ReddeTT.IsAuthenticated)
            {
                LoadingLabel.Text = "Authenticating...";
                await LoadingBar.ProgressTo(0.2, 750, Easing.Linear);
                await Navigation.PushModalAsync(new AuthenticationPage(), true);
            }
            else
            {
                if (App.ReddeTT.IsExpired)
                {
                    LoadingLabel.Text = "Getting refresh token...";
                    await LoadingBar.ProgressTo(0.5, 750, Easing.Linear);
                    var tokenTask = App.ReddeTT.RefreshAccessToken();
                    await tokenTask.ContinueWith(task =>
                    {
                        GetUserData();
                    });
                }
                else
                {
                    GetUserData();
                }
            }
        }

        public async void AuthenticationModalClosed()
        {
            if (!string.IsNullOrEmpty(App.ReddeTT.Code))
            {
                LoadingLabel.Text = "Getting token...";
                await LoadingBar.ProgressTo(0.5, 750, Easing.Linear);

                var tokenTask = App.ReddeTT.GetToken(App.ReddeTT.Code);
                await tokenTask.ContinueWith(async task1 =>
                {
                    if (App.ReddeTT.IsAuthenticated)
                    {
                        var userTask = App.ReddeTT.GetCurrentUser();
                        await userTask.ContinueWith(task2 =>
                        {
                            Device.BeginInvokeOnMainThread(async () => {
                                LoadingLabel.Text = "Done!";
                                await LoadingBar.ProgressTo(1.0, 750, Easing.Linear);
                                await Task.Delay(500);

                                Application.Current.MainPage = new NavigationPage(new BasicInitPage())
                                {
                                    BarBackgroundColor = Color.FromHex("#e74c3c"),
                                    BarTextColor = Color.White
                                };
                            });
                        });
                    }
                });
            }
            else
            {
                await Navigation.PushModalAsync(new AuthenticationPage(), true);
            }
        }

        private async void GetUserData()
        {
            Device.BeginInvokeOnMainThread(async () => {
                LoadingLabel.Text = "Getting user data...";
                await LoadingBar.ProgressTo(0.8, 750, Easing.Linear);
            });
            var userTask = App.ReddeTT.GetCurrentUser();
            await userTask.ContinueWith(task2 =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    LoadingLabel.Text = "Done!";
                    await LoadingBar.ProgressTo(1.0, 750, Easing.Linear);
                    await Task.Delay(500);

                    Application.Current.MainPage = new NavigationPage(new BasicInitPage())
                    {
                        BarBackgroundColor = Color.FromHex("#e74c3c"),
                        BarTextColor = Color.White
                    };
                });
            });
        }
    }
}
