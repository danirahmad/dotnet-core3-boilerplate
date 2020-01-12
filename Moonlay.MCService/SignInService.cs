using Microsoft.AspNetCore.Http;
using Moonlay.Core.Models;
using System.Security.Claims;

namespace Moonlay.MCService
{
    internal class SignInService : ISignInService
    {
        private readonly IHttpContextAccessor _httpContext;

        public SignInService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string CurrentUser => _httpContext.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier) ? _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value : null;

        public bool Demo => _httpContext.HttpContext.User == null ? false : _httpContext.HttpContext.User.FindFirst(c => c.ValueType == "demo") != null;
    }
}