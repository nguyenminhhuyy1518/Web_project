using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models
{
	public class CategoryModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage =" yen cau nhap ten danh muc")]
		public string Name { get; set; }
		[Required(ErrorMessage = " yen cau nhap mo ta danh muc")]
		public string Description { get; set; }
		
		public string Slug { get; set; }

		public int status { get; set; }
	}
}
