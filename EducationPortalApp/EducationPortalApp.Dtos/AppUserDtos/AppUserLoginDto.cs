﻿namespace EducationPortalApp.Dtos.AppUserDtos
{
    public class AppUserLoginDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
