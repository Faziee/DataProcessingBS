using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class ProfileModule
{
    public static void AddProfileEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/profiles", async ([FromBody] CreateProfileRequest createProfileRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var profile = new Profile()
            {
                Account_Id = createProfileRequest.Account_Id,
                Profile_Image = createProfileRequest.Profile_Image,
                Child_Profile = createProfileRequest.Child_Profile,
                User_Age = createProfileRequest.User_Age,
                Language = createProfileRequest.Language
            };

            await dbContext.Profiles.AddAsync(profile);
            await dbContext.SaveChangesAsync();
            return Results.Ok(profile);
        });

        app.MapPut("/profiles/{profileId}", async (int profileId, [FromBody] UpdateProfileRequest updateProfileRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var profile = await dbContext.Profiles.FirstOrDefaultAsync(p => p.Profile_Id == profileId);

            if (profile == null) return Results.NotFound();

            profile.Account_Id = updateProfileRequest.Account_Id;
            profile.Profile_Image = updateProfileRequest.Profile_Image;
            profile.Child_Profile = updateProfileRequest.Child_Profile;
            profile.User_Age = updateProfileRequest.User_Age;
            profile.Language = updateProfileRequest.Language;

            await dbContext.SaveChangesAsync();
            return Results.Ok(profile);
        });

        app.MapGet("/profiles", async (AppDbcontext dbContext) =>
        {
            var profiles = await dbContext.Profiles.ToListAsync();
            return Results.Ok(profiles);
        });

        app.MapGet("/profiles/{profileId:int}", async (int profileId, [FromServices] AppDbcontext dbContext) =>
        {
            var profile = await dbContext.Profiles.FindAsync(profileId);
            return profile == null ? Results.NotFound() : Results.Ok(profile);
        });

        app.MapDelete("/profiles/{profileId:int}", async (int profileId, [FromServices] AppDbcontext dbContext) =>
        {
            var profile = await dbContext.Profiles.FindAsync(profileId);

            if (profile == null) return Results.NotFound("Profile not found.");

            dbContext.Profiles.Remove(profile);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Message = "Profile deleted successfully." });
        });
    }
}
