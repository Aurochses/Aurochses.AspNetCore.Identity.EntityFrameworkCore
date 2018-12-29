using System;
using Aurochses.Data.Extensions.MsSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aurochses.AspNetCore.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Identity DbContext
    /// </summary>
    public class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        private readonly string _schemaName;

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityDbContext" />.
        /// </summary>
        /// <param name="dbContextOptions">The options to be used by a <see cref="DbContext" />.</param>
        /// <param name="schemaName">The schema name.</param>
        public IdentityDbContext(DbContextOptions<IdentityDbContext> dbContextOptions, string schemaName = "identity")
            : base(dbContextOptions)
        {
            _schemaName = schemaName;
        }

        /// <summary>
        /// Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationRole>().ToTable("Role", _schemaName);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim", _schemaName);
            modelBuilder.Entity<ApplicationUser>().ToTable("User", _schemaName);
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaim", _schemaName);
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin", _schemaName);
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole", _schemaName);
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken", _schemaName);

            modelBuilder.Entity<ApplicationRole>()
                .Property(b => b.Id)
                .HasDefaultValueSql(Functions.NewSequentialId);

            modelBuilder.Entity<ApplicationUser>()
                .Property(b => b.Id)
                .HasDefaultValueSql(Functions.NewSequentialId);

            modelBuilder.Entity<ApplicationUser>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql(Functions.GetUtcDate);
        }
    }
}