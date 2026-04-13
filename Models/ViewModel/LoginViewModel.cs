using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.ViewModel
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = " nhap ten username")]
		public string UserName { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = " nhap mat khau")]
		public string PassWord { get; set; }
		public string ReturnUrl { get; set; }
	}
}
