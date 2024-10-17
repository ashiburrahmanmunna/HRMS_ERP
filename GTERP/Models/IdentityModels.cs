
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GTERP.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //[Required]
        //public string UserId { get; set; }
        // public bool? IsEnabled { get; set; }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom user claims here
        //    //userIdentity.AddClaim(new Claim("UserId", UserId));
        //    //userIdentity.AddClaim(new Claim("IsEnabled", IsEnabled));

        //    return userIdentity;
        //}


    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public ApplicationDbContext()
        //    : base("ApplicationServices", throwIfV1Schema: false)
        //{
        //}

        //static ApplicationDbContext()
        //{
        //    // Set the database intializer which is run once during application start
        //    // This seeds the database with admin user credentials and admin role
        //    Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        //}

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}
    }
}