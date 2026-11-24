using System.Security.Claims;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Security;

public sealed class JwtSessionValidator(AppDbContext context) : JwtBearerEvents
{
    public override async Task TokenValidated(TokenValidatedContext validationContext)
    {
        var principal = validationContext.Principal;
        if (!int.TryParse(principal?.FindFirstValue("IdLoginUser"), out var userId) ||
            !Guid.TryParse(principal?.FindFirstValue("SecurityStamp"), out var stamp))
        {
            validationContext.Fail("Invalid session.");
            return;
        }

        var role = principal?.FindFirstValue(ClaimTypes.Role);
        var user = await context.LoginUsers.AsNoTracking()
            .Where(user => user.Idloginuser == userId && user.IsActive)
            .Select(user => new { user.SecurityStamp, user.UserRole })
            .SingleOrDefaultAsync(validationContext.HttpContext.RequestAborted);

        if (user is null || user.SecurityStamp != stamp ||
            !string.Equals(user.UserRole, role, StringComparison.Ordinal))
        {
            validationContext.Fail("Invalid session.");
        }
    }
}
