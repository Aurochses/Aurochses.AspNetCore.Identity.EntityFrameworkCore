using Microsoft.EntityFrameworkCore;

namespace Aurochses.AspNetCore.Identity.EntityFrameworkCore.Tests.Fakes
{
    public class FakeIdentityDbContext : IdentityDbContext
    {
        public FakeIdentityDbContext(DbContextOptions<IdentityDbContext> dbContextOptions, string schemaName = "identity")
            : base(dbContextOptions, schemaName)
        {

        }

        public void TestOnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreating(modelBuilder);
        }
    }
}