using Microsoft.AspNetCore.Identity;

namespace ShopComputer.Models
{
	public class AppUserModel : IdentityUser
	{
		public string Occupation { get; set; }
		public string RoleId { get; set; }
	}
}
