using PRegSys.DAL.Entities;

using Microsoft.EntityFrameworkCore;

namespace PRegSys.DAL;

public class PregsysDbContext(DbContextOptions<PregsysDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCollation("case_insensitive",
            locale: "und-u-ks-level2", provider: "icu", deterministic: false);

        // automatically apply configurations from all IEntityTypeConfiguration<T> implementations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PregsysDbContext).Assembly);
    }
}
