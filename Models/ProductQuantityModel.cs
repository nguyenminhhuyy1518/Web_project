using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Models
{
    public class ProductQuantityModel
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "yêu cầu nhập số lượng sản phẩm")]
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public DateTime DataCreated { get; set; }
    }
}
