using DataProcessingBS.Contracts;
using DataProcessingBS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Data;

public class AppDbcontext : DbContext
{
    public AppDbcontext(DbContextOptions<AppDbcontext> options) : base(options)
    {
    }

    public DbSet<ApiKey> ApiKeys { get; set; }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<Media> Media { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Subtitle> Subtitles { get; set; }
    public DbSet<Watch> Watches { get; set; }
    public DbSet<WatchList> WatchLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MovieDto>().HasNoKey();

        modelBuilder.Entity<SubscriptionDto>().HasNoKey();

        modelBuilder.Entity<Profile>()
            .HasOne(p => p.Account)
            .WithMany(a => a.Profiles)
            .HasForeignKey(p => p.Account_Id);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
    }
}