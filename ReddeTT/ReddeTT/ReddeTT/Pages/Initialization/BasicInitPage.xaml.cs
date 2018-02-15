using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Settings;
using ReddeTT.ViewModels;
using Xamarin.Forms;

namespace ReddeTT.Pages.Initialization
{
    public partial class BasicInitPage : ContentPage
    {
        private BasicInitViewModel _bivm;
        public BasicInitPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = "Basic setup";
            
            _bivm = this.BindingContext as BasicInitViewModel;
        }
        
        private void Button_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_bivm.SelectedInterest))
            {
                return;
            }
            else
            {
                CrossSettings.Current.AddOrUpdateValue("looking_for", _bivm.SelectedInterest);
                if (_bivm.SelectedInterest == "Women" || _bivm.SelectedInterest == "Men" || _bivm.SelectedInterest == "Everyone")
                {
                    // Navigate to picture page
                }
            }

            // navigate to next page
            
        }

        private void BindableRadioGroup_OnCheckedChanged(object sender, int e)
        {
            _bivm.SelectedInterest = _bivm.Interests[e];
        }
    }
}
