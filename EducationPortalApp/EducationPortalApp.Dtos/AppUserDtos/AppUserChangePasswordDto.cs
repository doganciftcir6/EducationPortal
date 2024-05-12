namespace EducationPortalApp.Dtos.AppUserDtos
{
    public class AppUserChangePasswordDto
    {
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
