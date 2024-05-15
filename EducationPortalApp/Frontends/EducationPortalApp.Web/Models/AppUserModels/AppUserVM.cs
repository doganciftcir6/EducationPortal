namespace EducationPortalApp.Web.Models.AppUserModels
{
    public class AppUserVM
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Fullname
        {
            get
            {
                if (Firstname == null && Lastname == null)
                {
                    return null;
                }

                return $"{Firstname} {Lastname}".Trim();
            }
        }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Gender { get; set; }
    }
}
