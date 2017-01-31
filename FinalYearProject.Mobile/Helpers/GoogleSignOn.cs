using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Plus;
using Android.OS;
using Exception = System.Exception;
using Object = Java.Lang.Object;

#pragma warning disable 618

namespace FinalYearProject.Mobile.Helpers
{
    public class GoogleSignOn : Object, GoogleApiClient.IConnectionCallbacks,
        GoogleApiClient.IOnConnectionFailedListener
    {
        public const int RC_SIGN_IN = 0;
        private TaskCompletionSource<User> _tcs;
        public Activity Context { get; private set; }
        public GoogleApiClient ApiClient { get; private set; }
        public bool Resolving { get; private set; }
        public bool ShouldResolve { get; private set; }

        public void Init(Activity context)
        {
            Context = context;

            ApiClient = new GoogleApiClient.Builder(Context)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddScope(new Scope(Scopes.Email))
                .AddScope(new Scope(Scopes.Profile))
                .AddScope(new Scope(Scopes.PlusLogin))
                .AddScope(new Scope(Scopes.PlusMe))
                .AddApi(PlusClass.API)
                .Build();
        }

        public void SignOut()
        {
            if (ApiClient.IsConnected)
            {
                PlusClass.AccountApi.ClearDefaultAccount(ApiClient);

                ApiClient.Disconnect();
            }
        }
        public Task<User> SignInAsync()
        {
            _tcs = new TaskCompletionSource<User>();

            ShouldResolve = true;

            ApiClient.Connect();

            return _tcs.Task;
        }

        public void Resolve(int requestCode, bool isOk)
        {
            if (requestCode == RC_SIGN_IN)
            {
                ShouldResolve = !isOk;
                Resolving = false;
                ApiClient.Connect();
            }
        }

        public void OnConnected(Bundle connectionHint)
        {
            ShouldResolve = false;

            if (PlusClass.PeopleApi.GetCurrentPerson(ApiClient) != null)
            {
                var user = new User();

                var currentPerson = PlusClass.PeopleApi.GetCurrentPerson(ApiClient);

                user.Id = currentPerson.Id;
                user.FullName = currentPerson.DisplayName;
                user.ProfilePictureUrl = currentPerson.Image.Url;
                user.Email = PlusClass.AccountApi.GetAccountName(ApiClient);

                _tcs.SetResult(user);
            }
        }

        public void OnConnectionSuspended(int cause)
        {
            _tcs.SetException(new Exception("Connection Suspended"));
        }

        public void OnConnectionFailed(ConnectionResult connectionResult)
        {
            if (!Resolving && ShouldResolve)
                if (connectionResult.HasResolution)
                    try
                    {
                        connectionResult.StartResolutionForResult(Context, RC_SIGN_IN);
                        Resolving = true;
                    }
                    catch (IntentSender.SendIntentException)
                    {
                        Resolving = false;
                        ApiClient.Connect();
                    }
                else
                    _tcs.SetException(new Exception("No Resolution with: " + connectionResult.ErrorMessage));
        }

        public class User
        {
            public string FullName { get; set; }
            public string Id { get; set; }
            public string ProfilePictureUrl { get; set; }
            public string Email { get; set; }
        }
    }
}