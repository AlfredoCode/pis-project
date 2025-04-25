using PRegSys.DAL.Entities;

using Microsoft.EntityFrameworkCore;

namespace PRegSys.DAL;

public class PregsysDbContext(DbContextOptions<PregsysDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<SignUpRequest> SignRequests => Set<SignUpRequest>();
    public DbSet<Evaluation> Evaluations => Set<Evaluation>();
    public DbSet<Solution> Solutions => Set<Solution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCollation("case_insensitive",
            locale: "und-u-ks-level2", provider: "icu", deterministic: false);

        // automatically apply configurations from all IEntityTypeConfiguration<T> implementations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PregsysDbContext).Assembly);

        Seeds.UserSeed.Seed(modelBuilder);
        Seeds.ProjectSeed.Seed(modelBuilder);
        Seeds.TeamSeed.Seed(modelBuilder);
        Seeds.EvaluationSeed.Seed(modelBuilder);
        Seeds.SignUpRequestSeed.Seed(modelBuilder);
        Seeds.SolutionSeed.Seed(modelBuilder);
    }
}