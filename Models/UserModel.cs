using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = " nhap ten username")]
		public string UserName { get; set; }
		[Required(ErrorMessage ="nhap email")]
		public string Email { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage =" nhap mat khau")]

		public string PassWord { get; set; }
	}
}
