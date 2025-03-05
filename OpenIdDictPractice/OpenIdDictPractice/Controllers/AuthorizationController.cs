using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;

namespace OpenIdDictPractice.Controllers
{
    [ApiController]
    [Route("connect")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IOpenIddictApplicationManager _applicationManager;

        public AuthorizationController(IOpenIddictApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }

        [HttpPost("token"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request.IsPasswordGrantType())
            {
                // Validar las credenciales del usuario aquí
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                identity.AddClaim(Claims.Subject, "cf703583-4366-49c2-906c-67683e83970c", Destinations.AccessToken);
                var principal = new ClaimsPrincipal(identity);
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            return BadRequest(new OpenIddictResponse
            {
                Error = Errors.InvalidGrant,
                ErrorDescription = "The specified grant is not supported."
            });
        }
    }
}
