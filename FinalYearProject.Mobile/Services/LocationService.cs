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
using System.Threading.Tasks;
using Android;
using Android.Support.Design.Widget;
using Plugin.Geolocator;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Plugin.Geolocator.Abstractions;
using System.Threading;

namespace FinalYearProject.Mobile.Services
{
    [Service]
    public class LocationService 
    {
    //    Position position;

    //    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    //    {
    //        Location l = new Location();
    //        Thread t = new Thread(async () =>
    //        {
    //            await l.GetLocationCompatAsync(position);
    //        });
    //        t.Start();
    //        return StartCommandResult.Sticky;
    //    }

    //    public override IBinder OnBind(Intent intent)
    //    {
    //        return null;
    //    }
    //}

    //public class Location: Activity
    //{
    //    const int RequestLocationId = 0;

    //    readonly string[] PermissionsLocation =
    //        {
    //            Manifest.Permission.AccessCoarseLocation,
    //            Manifest.Permission.AccessFineLocation
    //        };

    //    TextView textLocation;
    //    View layout;

    //    async Task TryGetLocationAsync(Position position)
    //    {
    //        if ((int)Build.VERSION.SdkInt < 23)
    //        {
    //            await GetLocationAsync(position);
    //            return;
    //        }

    //        await GetLocationPermissionAsync(position);
    //    }

    //    async Task GetLocationPermissionAsync(Position position)
    //    {
    //        //Check to see if any permission in our group is available, if one, then all are
    //        const string permission = Manifest.Permission.AccessFineLocation;
    //        if (CheckSelfPermission(permission) == (int)Permission.Granted)
    //        {
    //            await GetLocationAsync(position);
    //            return;
    //        }

    //        //need to request permission
    //        if (ShouldShowRequestPermissionRationale(permission))
    //        {
    //            //Explain to the user why we need to read the contacts
    //            Snackbar.Make(layout, "Location access is required to for data collection.", Snackbar.LengthIndefinite)
    //                    .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
    //                    .Show();
    //            return;
    //        }
    //        //Finally request permissions with the list of permissions and Id
    //        RequestPermissions(PermissionsLocation, RequestLocationId);
    //    }

    //    async Task GetLocationAsync(Position position)
    //    {
    //        textLocation.Text = "Getting Location";
    //        try
    //        {
    //            var locator = CrossGeolocator.Current;
    //            locator.DesiredAccuracy = 100;
    //            position = await locator.GetPositionAsync(20000);

                
    //        }
    //        catch (Exception ex)
    //        {
    //            textLocation.Text = "Unable to get location: " + ex.ToString();
    //        }
    //    }

    //    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
    //    {
    //        switch (requestCode)
    //        {
    //            case RequestLocationId:
    //                {
    //                    if (grantResults[0] == (int)Permission.Granted)
    //                    {
    //                        //Permission granted
    //                    }
    //                    else
    //                    {
    //                        //Permission Denied :(
    //                        //Disabling location functionality
    //                        var snack = Snackbar.Make(layout, "Location permission is denied.", Snackbar.LengthShort);
    //                        snack.Show();
    //                    }
    //                }
    //                break;
    //        }
    //    }

    //    public async Task GetLocationCompatAsync(Position position)
    //    {
    //        const string permission = Manifest.Permission.AccessFineLocation;

    //        if C(ContextCompat.CheckSelfPermission(this, permission) == Permission.Granted)
    //        {
    //            await GetLocationAsync(position);
    //            return;
    //        }

    //        if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
    //        {
    //            //Explain to the user why we need to read the contacts
    //            Snackbar.Make(layout, "Location access is required to show coffee shops nearby.",
    //                Snackbar.LengthIndefinite)
    //                .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
    //                .Show();
    //            return;
    //        }

    //        RequestPermissions(PermissionsLocation, RequestLocationId);
    //    }
    }
}