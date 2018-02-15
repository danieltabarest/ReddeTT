using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReddeTT.Droid.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ProgressBar), typeof(CustomProgressBar))]
namespace ReddeTT.Droid.Helpers
{
    public class CustomProgressBar : ProgressBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            Control.ProgressDrawable.SetColorFilter(new Android.Graphics.Color(231, 76, 60), Android.Graphics.PorterDuff.Mode.SrcIn);
            //"", Android.Graphics.PorterDuff.Mode.SrcIn);




        }
    }
}
