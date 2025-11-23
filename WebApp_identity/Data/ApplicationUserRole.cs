using Microsoft.AspNetCore.Identity;

namespace WebApp_identity.Data;

public class ApplicationUserRole: IdentityUserRole<string>
{
    public int? FacilityId { get; set; }
}