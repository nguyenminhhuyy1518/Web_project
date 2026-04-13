using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopComputer.Repository.validation;

namespace ShopComputer.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		[Required( ErrorMessage = " yen cau nhap ten san pham ")]
		public string Name { get; set; }
		public string Slug { get; set; }
		[Required, MinLength(4, ErrorMessage = " yen cau nhap mo ta san pham ")]
		public string Description { get; set; }
		[Required ( ErrorMessage = " yen cau  nhap gia san pham ")]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }
        [Required, Range(1,int.MaxValue, ErrorMessage = " yen cau nhap thuong hieu san pham ")]
        public int BrandId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = " yen cau nhap danh muc san pham ")]
        public int CategoryId { get; set; }
		public int Quantity { get; set; }
		public int Sold { get; set; }
		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
		public String Image { get; set; } 
		public CommentModel Comments { get; set; }


		[NotMapped]
		[FiveExtension]
		public IFormFile? ImageUpload { get; set; }
	}
}
