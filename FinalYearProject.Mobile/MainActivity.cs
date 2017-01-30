using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Views;
using Android.Content;
using Plugin.Geolocator.Abstractions;
using Android;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Android.Support.V7.App;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms.Android;

namespace FinalYProject
{
    [Activity(Label = "FinalYProject", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        Button searchButton;
        Button barcodeScannerButton;
        AutoCompleteTextView autoTexView;

        Intent nextActivity;

        MobileBarcodeScanner scanner;

        const int RequestLocationId = 0;

        readonly string[] PermissionsLocation =
            {
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation
            };

        View layout;

        Position pos;
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            MobileBarcodeScanner.Initialize(Application);
            Platform.Init();

            scanner = new MobileBarcodeScanner();

            base.SetContentView(Resource.Layout.Main);

            layout = FindViewById(Resource.Layout.Main);
            //StartService(new Intent(this, typeof(Services.LocationService)));

            autoTexView = (AutoCompleteTextView)FindViewById(Resource.Id.autoCompleteTextViewSearch);

            Button clearButton = (Button)FindViewById(Resource.Id.buttonClear);

            clearButton.Click += (object sender, EventArgs e) =>
            {
                autoTexView.Text = "";
            };

            barcodeScannerButton = (Button)FindViewById(Resource.Id.buttonBarcodeScanner);
            barcodeScannerButton.Click += async delegate
            {
                scanner.UseCustomOverlay = false;

                //We can customize the top and bottom text of the default overlay
                scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
                scanner.BottomText = "Wait for the barcode to automatically scan!";

                //Start scanning
                var result = await scanner.Scan();

                HandleScanResult(result);
            };

            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        Android.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
                        //Remove fragment else it will crash as it is already added to backstack
                        Android.App.Fragment prev = FragmentManager.FindFragmentByTag("dialog");
                        if (prev != null)
                        {
                            ft.Remove(prev);
                        }
                        ft.AddToBackStack(null);
                        // Create and show the dialog.
                        DialogPermission newFragment = DialogPermission.NewInstance(null);
                        //Add fragment
                        newFragment.Show(ft, "dialog");
                        //await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    pos = await CrossGeolocator.Current.GetPositionAsync(10000);

                }
                else if (status != PermissionStatus.Unknown)
                {
                    //await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {

                //LabelGeolocation.Text = "Error: " + ex;
            }

            searchButton = (Button)FindViewById(Resource.Id.buttonSearch);
            searchButton.Click += (sender, e) =>
            {

                nextActivity = new Intent(this, typeof(ProductListingsActivity));
                nextActivity.PutExtra("location", pos.Latitude.ToString());
                StartActivity(nextActivity);
            };

            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void HandleScanResult(ZXing.Result result)
        {
            string msg = "";

            if (result != null && !string.IsNullOrEmpty(result.Text))
                msg = "Found Barcode: " + result.Text;
            else
                msg = "Scanning Canceled!";

            RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Long).Show());
        }

        //protected async void OnResume(Bundle bundle)
        //{
        //    var locator = CrossGeolocator.Current;
        //    locator.DesiredAccuracy = 50;

        //    if ((int)Build.VERSION.SdkInt < 23)
        //    {
        //        pos = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
        //    }
        //    else
        //    {
        //        const string permission = Manifest.Permission.AccessFineLocation;

        //        if (ContextCompat.CheckSelfPermission(this, permission) == Permission.Granted)
        //        {
        //            pos = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
        //        }
        //        else
        //        {
        //            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
        //            {
        //                // Provide an additional rationale to the user if the permission was not granted
        //                // and the user would benefit from additional context for the use of the permission.
        //                // For example if the user has previously denied the permission.
        //                Log.Info("OK", "Displaying camera permission rationale to provide additional context.");

        //                Snackbar.Make(layout, "Location access is required to for data collection.", Snackbar.LengthIndefinite)
        //                    .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
        //                    .Show();
        //            }
        //            else
        //            {
        //                // Camera permission has not been granted yet. Request it directly.
        //                IOnRequestPermissionsResultCallback(PermissionsLocation, RequestLocationId);
        //            }
        //        }
        //    }
        //}
        //public async Task GetLocationCompatAsync()
        //{
        //    const string permission = Manifest.Permission.AccessFineLocation;

        //    if (ContextCompat.CheckSelfPermission(this, permission) == Permission.Granted)
        //    {
        //        await GetLocationAsync();
        //        return;
        //    }

        //    if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
        //    {
        //        //Explain to the user why we need to read the contacts
        //        Snackbar.Make(layout, "Location access is required to show coffee shops nearby.",
        //            Snackbar.LengthIndefinite)
        //            .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
        //            .Show();
        //        return;
        //    }

        //    RequestPermissions(PermissionsLocation, RequestLocationId);
        //}

        //async Task GetLocationAsync()
        //{
        //    try
        //    {
        //        var locator = CrossGeolocator.Current;
        //        locator.DesiredAccuracy = 100;
        //        await locator.GetPositionAsync(20000);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        //{
        //    switch (requestCode)
        //    {
        //        case RequestLocationId:
        //            {
        //                if (grantResults[0] == (int)Permission.Granted)
        //                {
        //                    //Permission granted
        //                }
        //                else
        //                {
        //                    //Permission Denied :(
        //                    //Disabling location functionality
        //                    var snack = Snackbar.Make(layout, "Location permission is denied.", Snackbar.LengthShort);
        //                    snack.Show();
        //                }
        //            }
        //            break;
        //    }
        //}

        //async Task GetLocationPermissionAsync()
        //{
        //    //Check to see if any permission in our group is available, if one, then all are
        //    const string permission = Manifest.Permission.AccessFineLocation;
        //    if (CheckSelfPermission(permission) == (int)Permission.Granted)
        //    {
        //        await GetLocationAsync();
        //        return;
        //    }

        //    //need to request permission
        //    if (ShouldShowRequestPermissionRationale(permission))
        //    {
        //        //Explain to the user why we need to read the contacts
        //        Snackbar.Make(layout, "Location access is required to for data collection.", Snackbar.LengthIndefinite)
        //                .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
        //                .Show();
        //        return;
        //    }
        //    //Finally request permissions with the list of permissions and Id
        //    RequestPermissions(PermissionsLocation, RequestLocationId);
        //}

        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    string Provider = LocationManager.GpsProvider;

        //    if (locMgr.IsProviderEnabled(Provider))
        //    {
        //        locMgr.RequestLocationUpdates(Provider, 2000, 1, this);
        //    }
        //    else
        //    {
        //        //Log.Info(tag, Provider + " is not available. Does the device have location services enabled?");
        //    }
        //}
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionBarMenu, menu);
            return base.OnPrepareOptionsMenu(menu);
        }

        //get result of query then pass to paroductListingActivity

        //int permissionCheck = Icont.checkSelfPermission(this, Manifest.Permission.AccessCoarseLocation);

        //locMgr = GetSystemService(Context.LocationService) as LocationManager;

        //Criteria locationCriteria = new Criteria();

        //locationCriteria.Accuracy = Accuracy.Coarse;
        //locationCriteria.PowerRequirement = Power.Medium;

        //string locationProvider = locMgr.GetBestProvider(locationCriteria, false);

        //if (locationProvider != null)
        //{
        //    locMgr.GetLastKnownLocation(locationProvider);
        //}
        //else
        //{
        //    //Log.Info(tag, "No location providers available");
        //}


    }
}

