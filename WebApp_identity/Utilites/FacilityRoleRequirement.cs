using Microsoft.AspNetCore.Authorization;

namespace WebApp_identity.Utilites;

public class FacilityRoleRequirement: IAuthorizationRequirement
{
    public string Role { get; }
    public FacilityRoleRequirement(string role)
    {
        Role = role;
    }
}