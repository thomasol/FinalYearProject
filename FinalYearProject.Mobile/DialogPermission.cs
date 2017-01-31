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

namespace FinalYearProjectMobile
{
    public class DialogPermission : DialogFragment
    {
        public static DialogPermission NewInstance(Bundle bundle)
        {
            DialogPermission fragment = new DialogPermission();
            fragment.Arguments = bundle;
            return fragment;
        }
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
            alert.SetTitle("Confirm Access");
            alert.SetMessage("Need Location Access.");
            alert.SetPositiveButton("Allow", (senderAlert, args) => {
                Toast.MakeText(Activity, "Thanks!", ToastLength.Short).Show();
            });
            alert.SetNegativeButton("Deny", (senderAlert, args) => {
                Toast.MakeText(Activity, "Denied!", ToastLength.Short).Show();
            });
            return alert.Create();
        }
    }
}