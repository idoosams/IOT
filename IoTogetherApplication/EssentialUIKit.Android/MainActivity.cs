using Android;
using Android.Util;
using Android.Gms.Common;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace EssentialUIKit.Droid
{
    [Activity(Label = "Essential UI Kit", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public const string TAG = "MainActivity";
        internal static readonly string CHANNEL_ID = "my_notification_channel";

        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
                {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    // Permissions already granted - display a message.
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzE1NTE4QDMxMzgyZTMxMmUzMGRQVnFaZUc4Zk9Pc0dSeGdSOUc0T2tyZVNtMldld3hmRS9KUkthOHRnMW89");

            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

            base.OnCreate(savedInstanceState);

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    if (key != null)
                    {
                        var value = Intent.Extras.GetString(key);
                        Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                    }
                }
            }

            IsPlayServicesAvailable();
            CreateNotificationChannel();


            Forms.SetFlags("CollectionView_Experimental");

            Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            Syncfusion.XForms.Android.PopupLayout.SfPopupLayoutRenderer.Init();

            Syncfusion.XForms.Android.Core.Core.Init(this);
            
            this.LoadApplication(new App());

            // Change the status bar color
            this.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));

            // Enable scrolling to the page when the keyboard is enabled
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(TAG, "This device is not supported");
                    Finish();
                }
                return false;
            }

            Log.Debug(TAG, "Google Play Services is available.");
            return true;
        }


        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = CHANNEL_ID;
            var channelDescription = string.Empty;
            var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

    }
}