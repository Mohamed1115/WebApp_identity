using Microsoft.AspNetCore.Identity;

namespace WebApp_identity.Data;

public class ApplicationUser:IdentityUser
{
    public string Name { get; set; }
}