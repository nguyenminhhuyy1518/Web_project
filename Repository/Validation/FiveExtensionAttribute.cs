using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ShopComputer.Repository.validation
{
    public class FiveExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            if( value is IFormFile five)
            {
                var extension = Path.GetExtension(five.FileName);
                string[] extensions = { "png", "jpg", "jpeg" };

                bool result = extensions.Any(x => extension.EndsWith(x));
                if (!result)
                {
                    return new ValidationResult("Allow extension are jpg or png or jpeg");
                }
            }
            return ValidationResult.Success;
        }

    }
}
