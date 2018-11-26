using System;
using Bowgum.GraphQL.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Bowgum.Models {
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