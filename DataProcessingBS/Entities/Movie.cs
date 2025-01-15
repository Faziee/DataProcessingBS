using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Movie
{
    [Key]
    public int Movie_Id { get; set; }
    
    
    public int Media_Id { get; set; }

    [Required]
    [StringLength(3, MinimumLength = 2)]
    public string Has_Subtitles { get; set; } = "no";
    
    //In dbContext you add this but not sure if we need to doa ll this since it is being done in the DB
    
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    // 
    //     modelBuilder.Entity<Movie>()
    //         .Property(m => m.Blocked)
    //         .IsRequired() // Ensures NOT NULL
    //         .HasMaxLength(3) // Enforces CHAR(3)
    //         .HasDefaultValue("no"); // Optional: Default value if none is provided
    // 
    //     // Adding the CHECK constraint for valid values
    //     modelBuilder.Entity<Movie>()
    //         .HasCheckConstraint("CK_Movie_Blocked", "[Blocked] IN ('yes', 'no')");
    // }
    // 

}