using Microsoft.AspNetCore.Identity;
using csiro_mvc.Models;

namespace csiro_mvc.Middleware
{
    public class ProfileCompletionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProfileCompletionMiddleware> _logger;

        public ProfileCompletionMiddleware(RequestDelegate next, ILogger<ProfileCompletionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var user = await userManager.GetUserAsync(context.User);
                if (user != null && !user.CheckProfileComplete())
                {
                    // Allow access to profile page and logout
                    if (!IsProfileRelatedPath(context.Request.Path))
                    {
                        _logger.LogInformation("Redirecting user {UserId} to complete profile", user.Id);
                        context.Response.Redirect("/Profile/Complete");
                        return;
                    }
                }
            }

            await _next(context);
        }

        private bool IsProfileRelatedPath(PathString path)
        {
            return path.StartsWithSegments("/Profile") ||
                   path.StartsWithSegments("/Account/Logout") ||
                   path.StartsWithSegments("/lib") ||
                   path.StartsWithSegments("/css") ||
                   path.StartsWithSegments("/js");
        }
    }

    public static class ProfileCompletionMiddlewareExtensions
    {
        public static IApplicationBuilder UseProfileCompletion(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ProfileCompletionMiddleware>();
        }
    }
}
