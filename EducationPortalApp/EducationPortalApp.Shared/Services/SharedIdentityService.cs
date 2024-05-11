﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EducationPortalApp.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor _httpContextAccessor;
        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetUserId
        {
            get
            {
                var nameIdentifierClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                return nameIdentifierClaim != null ? int.Parse(nameIdentifierClaim.Value) : (int?)null;
            }
        }
    }
}
