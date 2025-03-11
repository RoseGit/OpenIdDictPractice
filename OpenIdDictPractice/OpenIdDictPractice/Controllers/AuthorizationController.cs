using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;

namespace OpenIdDictPractice.Controllers
{
    /// <summary>
    /// Auth controller
    /// </summary>
    [ApiController]
    [Route("connect")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IOpenIddictApplicationManager _applicationManager;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="applicationManager"><see cref="IOpenIddictApplicationManager"/></param>
        public AuthorizationController(IOpenIddictApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }

        /// <summary>
        /// Service that allows generating a bearer token.
        /// </summary>
        /// <returns>A bearer token</returns>
        [HttpPost("token"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request.IsPasswordGrantType())
            {
                // Validar credenciales del usuario
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Agregar claims
                identity.AddClaim(Claims.Subject, "cf703583-4366-49c2-906c-67683e83970c");
                if (request.Username.Equals("admin"))
                {
                    identity.AddClaim("UserType", "Admin");
                }
                else
                {
                    identity.AddClaim("UserType", "Normal");
                }


                var principal = new ClaimsPrincipal(identity);

                // 🔹 Establecer los destinos de los claims (IMPORTANTE)
                principal.SetDestinations(claim => claim.Type switch
                {
                    Claims.Subject => new[] { OpenIddict.Abstractions.OpenIddictConstants.Destinations.AccessToken },
                    "UserType" => new[] { OpenIddict.Abstractions.OpenIddictConstants.Destinations.AccessToken },
                    _ => Array.Empty<string>()
                });

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
