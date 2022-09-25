using System.Security.Claims;
using art_gallery_api.Authentication;
using art_gallery_api.Persistence;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IStateDataAccess, StateRepository>();
builder.Services.AddScoped<IArtistDataAccess, ArtistRepository>();
builder.Services.AddScoped<IArtefactDataAccess, ArtefactRepository>();
builder.Services.AddScoped<IUserDataAccesss, UserRepository>();

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", default);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "admin"));

    options.AddPolicy("UserOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "admin", "user", "developer"));

    options.AddPolicy("DeveloperOnly", policy =>
        policy.RequireClaim(ClaimTypes.Email, "chloeehulme@gmail.com"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();