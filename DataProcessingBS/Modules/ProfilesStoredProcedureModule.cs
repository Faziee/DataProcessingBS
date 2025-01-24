using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class ProfilesStoredProcedureModule
{
    public static void AddProfileStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-profile",
            async ([FromBody] CreateProfileRequest createProfileRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC CreateProfile @AccountId={createProfileRequest.Account_Id}, @ProfileImage={createProfileRequest.Profile_Image}, @ChildProfile={createProfileRequest.Child_Profile}, @UserAge={createProfileRequest.User_Age}, @Language={createProfileRequest.Language}");
                return Results.Ok();
            });

        app.MapPut("/stored-procedure-update-profile-by-id",
            async ([FromBody] UpdateProfileRequest updateProfileRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateProfileById @ProfileId={updateProfileRequest.Profile_Id}, @AccountId={updateProfileRequest.Account_Id}, @ProfileImage={updateProfileRequest.Profile_Image}, @ChildProfile={updateProfileRequest.Child_Profile}, @UserAge={updateProfileRequest.User_Age}, @Language={updateProfileRequest.Language}");
                return Results.Ok();
            });

        app.MapGet("/stored-procedure-get-profiles", async (AppDbcontext dbContext) =>
        {
            var profiles = await dbContext.Profiles.FromSqlRaw("EXEC GetAllProfiles").ToListAsync();
            return Results.Ok(profiles);
        });

        app.MapGet("/stored-procedure-get-profile-by-id/{profileId:int}", async (int profileId, [FromServices] AppDbcontext dbContext) =>
        {
            var profile = await dbContext.Profiles
                .FromSqlInterpolated($"EXEC GetProfileById @ProfileId={profileId}")
                .ToListAsync()
                .ContinueWith(task => task.Result.Select(p => new ProfileDto()
                {
                    Profile_Id = p.Profile_Id,
                    Account_Id = p.Account_Id,
                    Profile_Image = p.Profile_Image,
                    Child_Profile = p.Child_Profile,
                    User_Age = p.User_Age,
                    Language = p.Language
                }).FirstOrDefault());

            return profile == null
                ? Results.NotFound()
                : Results.Ok(profile);
        });

        app.MapDelete("/stored-procedure-delete-profile-by-id/{profileId}", async (int profileId, [FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteProfileById @ProfileId={profileId}");
            return Results.Ok();
        });
    }
}
