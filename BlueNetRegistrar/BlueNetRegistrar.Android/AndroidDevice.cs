using Android.App;
using Android.Provider;
using BlueNetRegistrar.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidDevice))]

namespace BlueNetRegistrar.Droid
{
    public class AndroidDevice : IDevice
    {
        public string GetIdentifier()
        {
            return Settings.Secure.GetString(Application.Context.ContentResolver, Settings.Secure.AndroidId);
        }
    }
}
