using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static string GetCaptchaJsonPackageNonSecret()
        {
            return Fast.GetCaptchaJsonLocked();
        }

        internal static string GetGooglePasswordNonSecret()
        {
            return Fast.GetGooglePasswordLocked();
        }
    }
}