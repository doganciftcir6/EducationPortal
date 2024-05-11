namespace EducationPortalApp.Shared.Utilities.Security
{
    public class JwtTokenDefaults
    {
        public const string ValidAudience = "http://localhost";
        public const string ValidIssuer = "http://localhost";
        public const string Key = "bu-32-karakter-uzunlukta-olmali-bir-anahtar-";
        public const int Expire = 5;
    }
}
