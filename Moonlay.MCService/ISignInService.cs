using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Moonlay.MCService
{
    public interface ISignInService
    {
        string CurrentUser { get; }

        bool Demo { get; }
    }

    internal class SignInService : ISignInService
    {
        private readonly IHttpContextAccessor _httpContext;

        public SignInService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string CurrentUser => _httpContext.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

        public bool Demo => _httpContext.HttpContext.User == null ? false : _httpContext.HttpContext.User.FindFirst(c => c.ValueType == "demo") != null;
    }
}