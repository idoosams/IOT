using Android;
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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjgwMjE1QDMxMzgyZTMxMmUzMFpVVGFVcVNiK25MaWZYSmJYK0ZwUFM1MGJUVEtKMXZRTG9nWVpCRDJBT2s9");

            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

            base.OnCreate(savedInstanceState);            

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
    }
}