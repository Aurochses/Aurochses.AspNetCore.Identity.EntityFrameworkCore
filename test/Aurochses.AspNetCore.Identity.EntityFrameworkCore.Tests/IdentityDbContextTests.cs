using System;
using System.Linq.Expressions;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore.Tests.Fakes;
using Aurochses.Data.Extensions.MsSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Aurochses.AspNetCore.Identity.EntityFrameworkCore.Tests
{
    public class IdentityDbContextTests
    {
        private readonly FakeIdentityDbContext _dbContext;
        private const string SchemaName = "dbo";
        private readonly ModelBuilder _modelBuilder;

        public IdentityDbContextTests()
        {
            var dbContextOptions = new DbContextOptions<IdentityDbContext>();

            _dbContext = new FakeIdentityDbContext(dbContextOptions, SchemaName);

            _modelBuilder = new ModelBuilder(new ConventionSet());

            _dbContext.TestOnModelCreating(_modelBuilder);
        }

        [Fact]
        public void Inherit_IdentityDbContext()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<IdentityDbContext<ApplicationUser, ApplicationRole, Guid>>(_dbContext);
        }

        [Fact]
        public void OnModelCreating_IdentityDbContext_ApplicationRoleMapped()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_modelBuilder.Model.FindEntityType(typeof(ApplicationRole)));

            CheckTable<ApplicationRole>(_modelBuilder, SchemaName, "Role");
            CheckPropertyDefaultValueSql<ApplicationRole, Guid>(_modelBuilder, x => x.Id, Functions.NewSequentialId);
        }

        [Fact]
        public void OnModelCreating_IdentityDbContext_IdentityRoleClaimOfGuidMapped()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_modelBuilder.Model.FindEntityType(typeof(IdentityRoleClaim<Guid>)));

            CheckTable<IdentityRoleClaim<Guid>>(_modelBuilder, SchemaName, "RoleClaim");
        }

        [Fact]
        public void OnModelCreating_IdentityDbContext_ApplicationUserMapped()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_modelBuilder.Model.FindEntityType(typeof(ApplicationUser)));

            CheckTable<ApplicationUser>(_modelBuilder, SchemaName, "User");
            CheckPropertyDefaultValueSql<ApplicationUser, Guid>(_modelBuilder, x => x.Id, Functions.NewSequentialId);
            CheckPropertyDefaultValueSql<ApplicationUser, DateTime>(_modelBuilder, x => x.CreatedDate, Functions.GetUtcDate);
        }

        [Fact]
        public void OnModelCreating_IdentityDbContext_IdentityUserClaimOfGuidMapped()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_modelBuilder.Model.FindEntityType(typeof(IdentityUserClaim<Guid>)));

            CheckTable<IdentityUserClaim<Guid>>(_modelBuilder, SchemaName, "UserClaim");
        }

        [Fact]
        public void OnModelCreating_IdentityDbContext_IdentityUserLoginOfGuidMapped()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_modelBuilder.Model.FindEntityType(typeof(IdentityUserLogin<Guid>)));

            CheckTable<IdentityUserLogin<Guid>>(_modelBuilder, SchemaName, "UserLogin");
        }

        [Fact]
        public void OnModelCreating_IdentityDbContext_IdentityUserRoleOfGuidMapped()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_modelBuilder.Model.FindEntityType(typeof(IdentityUserRole<Guid>)));

            CheckTable<IdentityUserRole<Guid>>(_modelBuilder, SchemaName, "UserRole");
        }

        [Fact]
        public void OnModelCreating_IdentityDbContext_IdentityUserTokenOfGuidMapped()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_modelBuilder.Model.FindEntityType(typeof(IdentityUserToken<Guid>)));

            CheckTable<IdentityUserToken<Guid>>(_modelBuilder, SchemaName, "UserToken");
        }

        private static void CheckTable<TEntity>(ModelBuilder modelBuilder, string schemaName, string tableName)
            where TEntity : class
        {
            if (schemaName == null) throw new ArgumentNullException(nameof(schemaName));
            if (tableName == null) throw new ArgumentNullException(nameof(tableName));

            var entityTypeBuilder = modelBuilder.Entity<TEntity>();

            var relational = entityTypeBuilder.Metadata.Relational();

            Assert.Equal(schemaName, relational.Schema);
            Assert.Equal(tableName, relational.TableName);
        }

        private static void CheckPropertyDefaultValueSql<TEntity, TProperty>(ModelBuilder modelBuilder, Expression<Func<TEntity, TProperty>> propertyExpression, string expectedValue)
            where TEntity : class
        {
            if (expectedValue == null) throw new ArgumentNullException(nameof(expectedValue));

            var propertyBuilder = modelBuilder.Entity<TEntity>().Property(propertyExpression);

            var relational = propertyBuilder.Metadata.Relational();

            Assert.Equal(expectedValue, relational.DefaultValueSql);
        }
    }
}