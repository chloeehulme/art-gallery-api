using System.Security.Claims;
using art_gallery_api.Authentication;
using art_gallery_api.Persistence;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Allows snake case mapping from DB
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddControllers();

// Adds Persistence layer to build
builder.Services.AddScoped<IStateDataAccess, StateRepository>();
builder.Services.AddScoped<IArtistDataAccess, ArtistRepository>();
builder.Services.AddScoped<IArtefactDataAccess, ArtefactRepository>();
builder.Services.AddScoped<IUserDataAccesss, UserRepository>();

// Adds Authentication layer to build
builder.Services.AddAuthentication("BasicAuthentication")
.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", default);

// Adds Authorization and auth policies to build
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "admin"));

    options.AddPolicy("UserOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "admin", "user"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();