using Android.App;
using Android.OS;
using Android.Views;

namespace EssentialUIKit.Droid
{
    [Activity(Theme = "@style/Theme.Splash",
              MainLauncher = true,
              NoHistory = true, Icon = "@drawable/Icon")]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)((int)Window.DecorView.SystemUiVisibility ^ (int)SystemUiFlags.LayoutStable ^ (int)SystemUiFlags.LayoutFullscreen);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            this.StartActivity(typeof(MainActivity));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}