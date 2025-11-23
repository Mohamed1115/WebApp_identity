using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp_identity.Data;

namespace WebApp_identity.Utilites;

public class FacilityRoleHandler:AuthorizationHandler<FacilityRoleRequirement>
{
    private readonly ApplicationDbContext _db;

    public FacilityRoleHandler(ApplicationDbContext db)
    {
        _db = db;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        FacilityRoleRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return;

        // اقرأ الـ FacilityId من الـ Route
        var routeData = (context.Resource as HttpContext)?.GetRouteData();
        var facilityIdStr = routeData?.Values["facilityId"]?.ToString();

        if (!int.TryParse(facilityIdStr, out int facilityId))
            return;
        // var roleName = await _roleManager.FindByIdAsync(userRole.RoleId);

        // Check role + facility
        var isValid = await _db.UserRoles
            .AnyAsync(r =>
                r.UserId == userId &&
                r.FacilityId == facilityId &&
                r.RoleId == requirement.Role);

        if (isValid)
            context.Succeed(requirement);
    }
}