using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Zwemclub.Identity
{
    public class CustomSignInManager : SignInManager<CustomUser>
    {
        public CustomSignInManager(
            UserManager<CustomUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<CustomUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<CustomUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<CustomUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        // Identity roept deze methode aan vóór elke aanmelding; false geeft SignInResult.NotAllowed.
        public override async Task<bool> CanSignInAsync(CustomUser user)
        {
            if (!user.IsActief || user.IsVerwijderd == true)
            {
                return false;
            }

            return await base.CanSignInAsync(user);
        }
    }
}