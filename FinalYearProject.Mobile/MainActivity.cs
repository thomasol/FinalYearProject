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
using FinalYearProject.Mobile.Helpers;

namespace FinalYearProject.Mobile
{
    [Activity(Label = "MainActivity")]
    public class MainActivity : Activity
    {
        public GoogleSignOn GoogleSignOn { get; } = new GoogleSignOn();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            var button = FindViewById<Button>(Resource.Id.btnGoogle);
            button.Click += OnButtonClicked;

            GoogleSignOn.Init(this);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            GoogleSignOn.Resolve(requestCode, resultCode == Result.Ok);

            base.OnActivityResult(requestCode, resultCode, data);
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            var user = await GoogleSignOn.SignInAsync();
        }
    }
}