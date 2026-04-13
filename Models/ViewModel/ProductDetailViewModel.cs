using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models.ViewModel
{
	public class ProductDetailViewModel
	{
		public ProductModel ProductDetail { get; set; }
		[Required(ErrorMessage = " yen cau nhap mo ta san pham ")]
		public string Comment { get; set; }
		[Required(ErrorMessage = " yen cau nhap ten san pham ")]
		public string Name { get; set; }
		[Required(ErrorMessage = " yen cau nhap Email ")]
		public string Email { get; set; }
	}
}
