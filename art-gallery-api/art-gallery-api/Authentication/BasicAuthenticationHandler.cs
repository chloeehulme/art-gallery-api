using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using art_gallery_api.Models;
using art_gallery_api.Persistence;

namespace art_gallery_api.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        private UserRepository _repo = new();

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            base.Response.Headers.Add("WWW-Authenticate", @"Basic realm=""Access to the root controller.""");
            var authHeader = base.Request.Headers["Authorization"].ToString();

            if (authHeader == String.Empty)
            {
                Response.StatusCode = 401;
                return Task.FromResult(AuthenticateResult.Fail(""));
            }

            var authHeaderEncoded = authHeader[6..];
            var authHeaderDecoded = DecodeBase64String(authHeaderEncoded);

            string[] credentials = authHeaderDecoded.Split(":", 2);
            string email = credentials[0];
            string password = credentials[1];

            User? user = _repo.GetUserByEmail(email);

            if (user is null)
            {
                Response.StatusCode = 401;
                return Task.FromResult(AuthenticateResult.Fail($"User with {email} does not exist."));
            }

            var hasher = new PasswordHasher<User>();
            var pwVerificationResult = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (pwVerificationResult == PasswordVerificationResult.Success)
            {
                if (user.Role is null) user.Role = String.Empty;

                var claims = new[]
                    {
                        new Claim("name", $"{user.FirstName} {user.LastName}"),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                var identity = new ClaimsIdentity(claims, "Basic");
                var claimsPrincipal = new ClaimsPrincipal(identity);
                var authTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(authTicket));
            }

            Response.StatusCode = 401;
            return Task.FromResult(AuthenticateResult.Fail(""));
        }

        // Convert base64 string to UTF-8 string
        public static string DecodeBase64String(string encodedString)
        {
            var encodedBytes = System.Convert.FromBase64String(encodedString);
            return System.Text.Encoding.UTF8.GetString(encodedBytes);
        }
    }
}