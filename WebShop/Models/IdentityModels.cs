using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebShop.Models
{
	public enum Title { Mr, Mrs, Ms, Miss }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
		[MaxLength(50)]
		public string FirstName { get; set; }

		[MaxLength(50)]
		public string LastName { get; set; }

		public Title? Title { get; set; }

		[MaxLength(50)]
		public string Address { get; set; }

		[MaxLength(50)]
		public string House { get; set; }

		[MaxLength(10)]
		public string Zip { get; set; }

		[MaxLength(50)]
		public string City { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			
			// Add custom user claims here
			userIdentity.AddClaim(new Claim(ClaimTypes.GivenName, FirstName));

            return userIdentity;
        }
    }
}