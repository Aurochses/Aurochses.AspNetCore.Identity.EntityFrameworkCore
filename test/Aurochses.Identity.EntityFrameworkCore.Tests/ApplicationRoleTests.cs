using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xunit;

namespace Aurochses.Identity.EntityFrameworkCore.Tests
{
    public class ApplicationRoleTests
    {
        [Fact]
        public void Inherit_IdentityRole()
        {
            // Arrange & Act
            var applicationRole = new ApplicationRole();

            // Assert
            Assert.IsAssignableFrom<IdentityRole<Guid>>(applicationRole);
        }
    }
}