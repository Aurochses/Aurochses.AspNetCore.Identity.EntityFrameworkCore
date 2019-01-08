using System;
using System.ComponentModel.DataAnnotations;
using Xunit;
using System.ComponentModel.DataAnnotations.Schema;
using Aurochses.Data.Extensions.MsSql;
using Aurochses.Xunit;
using Microsoft.AspNetCore.Identity;

namespace Aurochses.AspNetCore.Identity.EntityFrameworkCore.Tests
{
    public class ApplicationUserTests
    {
        private readonly ApplicationUser _applicationUser;

        public ApplicationUserTests()
        {
            _applicationUser = new ApplicationUser();
        }

        [Fact]
        public void Inherit_IdentityUser()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<IdentityUser<Guid>>(_applicationUser);
        }

        [Fact]
        public void Inherit_IApplicationUser()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<IApplicationUser>(_applicationUser);
        }

        [Theory]
        [InlineData(typeof(DatabaseGeneratedAttribute), nameof(DatabaseGeneratedAttribute.DatabaseGeneratedOption), DatabaseGeneratedOption.Identity)]
        public void Id_Attribute_Defined(Type attributeType, string attributePropertyName, object attributePropertyValue)
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<ApplicationUser>("Id", attributeType);

            AttributeAssert.ValidateProperty(propertyInfo, attributeType, attributePropertyName, attributePropertyValue);
        }

        [Fact]
        public void Id_Property_Success()
        {
            // Arrange
            var id = new Guid("00000000-0000-0000-0000-000000000000");

            // Act
            _applicationUser.Id = id;

            // Assert
            Assert.Equal(id, _applicationUser.Id);
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute), null, null)]
        [InlineData(typeof(MaxLengthAttribute), nameof(MaxLengthAttribute.Length), 50)]
        public void FirstName_Attribute_Defined(Type attributeType, string attributePropertyName, object attributePropertyValue)
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<ApplicationUser>("FirstName", attributeType);

            if (attributePropertyName != null)
            {
                AttributeAssert.ValidateProperty(propertyInfo, attributeType, attributePropertyName, attributePropertyValue);
            }
        }

        [Fact]
        public void FirstName_Property_Success()
        {
            // Arrange
            const string firstName = "John";

            // Act
            _applicationUser.FirstName = firstName;

            // Assert
            Assert.Equal(firstName, _applicationUser.FirstName);
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute), null, null)]
        [InlineData(typeof(MaxLengthAttribute), nameof(MaxLengthAttribute.Length), 50)]
        public void LastName_Attribute_Defined(Type attributeType, string attributePropertyName, object attributePropertyValue)
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<ApplicationUser>("LastName", attributeType);

            if (attributePropertyName != null)
            {
                AttributeAssert.ValidateProperty(propertyInfo, attributeType, attributePropertyName, attributePropertyValue);
            }
        }

        [Fact]
        public void LastName_Property_Success()
        {
            // Arrange
            const string lastName = "Black";

            // Act
            _applicationUser.LastName = lastName;

            // Assert
            Assert.Equal(lastName, _applicationUser.LastName);
        }

        [Theory]
        [InlineData(typeof(DatabaseGeneratedAttribute), nameof(DatabaseGeneratedAttribute.DatabaseGeneratedOption), DatabaseGeneratedOption.Computed)]
        [InlineData(typeof(ColumnAttribute), nameof(ColumnAttribute.TypeName), ColumnTypes.DateTime)]
        public void CreatedDate_Attribute_Defined(Type attributeType, string attributePropertyName, object attributePropertyValue)
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<ApplicationUser>("CreatedDate", attributeType);

            AttributeAssert.ValidateProperty(propertyInfo, attributeType, attributePropertyName, attributePropertyValue);
        }

        [Fact]
        public void CreatedDate_Property_Success()
        {
            // Arrange
            var createdDate = DateTime.UtcNow;

            // Act
            _applicationUser.CreatedDate = createdDate;

            // Assert
            Assert.Equal(createdDate, _applicationUser.CreatedDate);
        }
    }
}