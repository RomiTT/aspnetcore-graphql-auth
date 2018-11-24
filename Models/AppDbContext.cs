using System;
using aspnetcore_graphql_auth.Authentication;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_graphql_auth.Models {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder
                .Entity<User>()
                .Property(e => e.Role)
                .HasConversion(
                    v => (int)v,
                    v => (UserRole)v);
        }

        public DbSet<User> Users { get; set; }
    }
}