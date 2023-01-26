using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Webly.Models.Entity;

namespace Webly.Models;

public class ApplicationDbContext: IdentityDbContext<AccountEntity>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProjectAccountEntity>()
            .Property(p => p.ProjectAccountType)
            .HasConversion<string>();

        builder.Entity<ActionLogEntity>()
            .Property(e => e.ActionType)
            .HasConversion<string>();
    }

    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<ActionLogEntity> ActionLogs { get; set; }
    public DbSet<FolderEntity> Folders { get; set; }
    public DbSet<LinkEntity> Links { get; set; }
    public DbSet<ProjectAccountEntity> ProjectAccounts { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
}
