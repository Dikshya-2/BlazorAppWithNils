using Microsoft.AspNetCore.Identity;

namespace BlazorAppWithNils.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        // we implement CPR here because CPR number stored in the user's profile
        //public string? CprNumber { get; set; }
    }

}
