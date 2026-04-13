using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models
{
	public class BrandModel
	{
		[Key]
		public int Id { get; set; }
		[Required( ErrorMessage = " yen cau nhap ten thuong hieu ")]
		public string Name { get; set; }
		[Required( ErrorMessage = " yen cau nhap mo ta thuong hieu ")]
		public string Description { get; set; }
		
		public string Slug { get; set; }

		public int status { get; set; }
	}
}
