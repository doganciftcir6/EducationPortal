﻿namespace EducationPortalApp.Web.Models.AppUserModels
{
    public class AppUserChangePasswordInput
    {
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}