using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using MotivationAdmin.Droid;
using MotivationAdmin.Controls;

//using Android.Widget;
[assembly: ExportRenderer(typeof(SwitchText), typeof(CustomSwitchRenderer))]
namespace MotivationAdmin.Droid
{
    class CustomSwitchRenderer : SwitchRenderer
    {
        
        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.TextOn = "Male";
                Control.TextOff = "Female";
                //  Control.SetTextColor(Color.AntiqueWhite);
            }

            if (Control.Checked == true)
            {
                //   Control.SetBackgroundColor(Color.Green);
            }
        }
    }
}
