using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Zwemclub.Identity
{
    public class CustomSignInManager : SignInManager<Gebruiker>
    {
        public CustomSignInManager(
            UserManager<Gebruiker> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<Gebruiker> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<Gebruiker>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<Gebruiker> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        // Identity roept deze methode aan vóór elke aanmelding; false geeft SignInResult.NotAllowed.
        public override async Task<bool> CanSignInAsync(Gebruiker user)
        {
            if (!user.IsActief || user.IsVerwijderd == true)
            {
                return false;
            }

            return await base.CanSignInAsync(user);
        }
    }
}