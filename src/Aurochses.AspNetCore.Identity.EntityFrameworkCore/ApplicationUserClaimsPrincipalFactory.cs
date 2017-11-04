using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Aurochses.AspNetCore.Identity.EntityFrameworkCore
{
    /// <summary>
    /// User Claims
    /// </summary>
    // http://benfoster.io/blog/customising-claims-transformation-in-aspnet-core-identity
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserClaimsPrincipalFactory"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="optionsAccessor">The options accessor.</param>
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {

        }

        /// <summary>
        /// Creates a <see cref="ClaimsPrincipal"/> from an user asynchronously.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsPrincipal"/> from.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ClaimsPrincipal"/>.
        /// </returns>
        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.FirstName)) claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            if (!string.IsNullOrWhiteSpace(user.LastName)) claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));

            if (UserManager.SupportsUserEmail && !string.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
                claims.Add(new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed.ToString(), ClaimValueTypes.Boolean));
            }

            if (UserManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                claims.Add(new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber));
                claims.Add(new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed.ToString(), ClaimValueTypes.Boolean));
            }

            ((ClaimsIdentity) principal.Identity).AddClaims(claims);

            return principal;
        }
    }
}