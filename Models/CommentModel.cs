using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models
{
	public class CommentModel
	{
		[Key]
		public int Id { get; set; }
		public int ProductId { get; set; }
		[Required(ErrorMessage = " yen cau nhap danh gia san pham ")]
		public string Comment { get; set; }
		[Required(ErrorMessage = " yen cau nhap ten san pham ")]
		public string Name { get; set; }
		[Required(ErrorMessage = " yen cau nhap email ")]
		public string Email { get; set; }

		public string Star { get; set; }

		[ForeignKey("ProductId")]
		public ProductModel Product { get; set; }
	}
}
