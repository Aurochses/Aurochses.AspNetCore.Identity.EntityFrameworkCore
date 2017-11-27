using System;
using System.Linq;
using System.Security.Claims;
using Aurochses.Xunit;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using IdentityModel;

namespace Aurochses.AspNetCore.Identity.EntityFrameworkCore.Tests
{
    public class ApplicationUserClaimsPrincipalFactoryTests
    {
        private const int BaseClaimsCount = 2; // amount of claims that added in base class

        private static Guid _applicationUserId = new Guid("00000000-0000-0000-0000-000000000000");
        private const string ApplicationUserUserName = "john.black";

        private readonly ApplicationUserClaimsPrincipalFactory _applicationUserClaimsPrincipalFactory;

        public ApplicationUserClaimsPrincipalFactoryTests()
        {
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict).Object, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(x => x.GetUserIdAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(_applicationUserId.ToString());
            mockUserManager.Setup(x => x.GetUserNameAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(ApplicationUserUserName);
            mockUserManager.SetupGet(x => x.SupportsUserEmail).Returns(true);
            mockUserManager.SetupGet(x => x.SupportsUserPhoneNumber).Returns(true);

            var mockRoleManager = new Mock<RoleManager<ApplicationRole>>(new Mock<IRoleStore<ApplicationRole>>(MockBehavior.Strict).Object, null, null, null, null);

            var mockOptionsAccessor = new Mock<IOptions<IdentityOptions>>(MockBehavior.Strict);
            mockOptionsAccessor.SetupGet(x => x.Value).Returns(new IdentityOptions());

            _applicationUserClaimsPrincipalFactory = new ApplicationUserClaimsPrincipalFactory(mockUserManager.Object, mockRoleManager.Object, mockOptionsAccessor.Object);
        }

        [Fact]
        public void Inherit_UserClaimsPrincipalFactory()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>(_applicationUserClaimsPrincipalFactory);
        }

        private void ValidateClaim(ClaimsPrincipal principal, string expectedClaimType, string expectedClaimValue, string expectedClaimValueType)
        {
            if (expectedClaimValue == null) throw new ArgumentNullException(nameof(expectedClaimValue));
            if (expectedClaimValueType == null) throw new ArgumentNullException(nameof(expectedClaimValueType));

            var claim = Assert.Single(principal.FindAll(expectedClaimType));
            Assert.NotNull(claim);
            Assert.Equal(expectedClaimValue, claim.Value);
            Assert.Equal(expectedClaimValueType, claim.ValueType);
        }

        [Fact]
        public void CreateAsync_Success()
        {
            // Arrange
            const int claimsCount = 6; // amount of claims that must be added

            var applicationUser = new ApplicationUser
            {
                Id = _applicationUserId,
                UserName = ApplicationUserUserName,
                FirstName = "John",
                LastName = "Black",
                Email = GetType().GenerateEmail(ApplicationUserUserName),
                EmailConfirmed = true,
                PhoneNumber = "+375297841506",
                PhoneNumberConfirmed = true
            };

            // Act
            var principal = _applicationUserClaimsPrincipalFactory.CreateAsync(applicationUser).Result;

            // Assert
            ValidateClaim(principal, JwtClaimTypes.GivenName, applicationUser.FirstName, ClaimValueTypes.String);
            ValidateClaim(principal, JwtClaimTypes.FamilyName, applicationUser.LastName, ClaimValueTypes.String);
            ValidateClaim(principal, JwtClaimTypes.Email, applicationUser.Email, ClaimValueTypes.String);
            ValidateClaim(principal, JwtClaimTypes.EmailVerified, applicationUser.EmailConfirmed.ToString(), ClaimValueTypes.Boolean);
            ValidateClaim(principal, JwtClaimTypes.PhoneNumber, applicationUser.PhoneNumber, ClaimValueTypes.String);
            ValidateClaim(principal, JwtClaimTypes.PhoneNumberVerified, applicationUser.PhoneNumberConfirmed.ToString(), ClaimValueTypes.Boolean);

            Assert.Equal(BaseClaimsCount + claimsCount, principal.Claims.Count());
        }

        [Fact]
        public void CreateAsync_Success_WhenApplicationUserValuesAreNull()
        {
            // Arrange
            const int claimsCount = 0; // amount of claims that must be added

            var applicationUser = new ApplicationUser
            {
                Id = _applicationUserId,
                UserName = ApplicationUserUserName,
                FirstName = null,
                LastName = null,
                Email = null,
                EmailConfirmed = true,
                PhoneNumber = null,
                PhoneNumberConfirmed = true
            };

            // Act
            var principal = _applicationUserClaimsPrincipalFactory.CreateAsync(applicationUser).Result;

            // Assert
            Assert.Equal(BaseClaimsCount + claimsCount, principal.Claims.Count());
        }
    }
}