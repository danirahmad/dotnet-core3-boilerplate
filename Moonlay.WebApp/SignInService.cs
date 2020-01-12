using Microsoft.AspNetCore.Http;
using Moonlay.Core.Models;
using System.Security.Claims;

namespace Moonlay.WebApp
{
    internal class SignInService : ISignInService
    {
        private readonly IHttpContextAccessor _httpContext;

        public SignInService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string CurrentUser => _httpContext.HttpContext == null ? null : (_httpContext.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier) ? _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value : "guest");

        public bool Demo => _httpContext.HttpContext == null ? false : (_httpContext.HttpContext.User == null ? false : _httpContext.HttpContext.User.FindFirst(c => c.ValueType == "demo") != null);
    }
}