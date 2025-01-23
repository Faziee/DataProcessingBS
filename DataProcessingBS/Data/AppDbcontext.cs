using DataProcessingBS.Contracts;
using Microsoft.AspNetCore.Mvc;
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

        
        // Define relationship between Account and Profile
        modelBuilder.Entity<Profile>()
            .HasOne(p => p.Account)  // Profile has one Account
            .WithMany(a => a.Profiles) // Account can have many Profiles
            .HasForeignKey(p => p.Account_Id); // Account_Id is the foreign key in Profile
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .EnableSensitiveDataLogging()  // Useful for debugging purposes
            .LogTo(Console.WriteLine, LogLevel.Information);  // Log SQL queries to console
    }

}