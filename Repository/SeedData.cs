using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopComputer.Models;

namespace ShopComputer.Repository
{
    public class Seeddata
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                CategoryModel macbook = new CategoryModel { Name = "Macbook", Slug = "Macbook", Description = "Macbook is the best", status = 1 };
                CategoryModel pc = new CategoryModel { Name = "PC", Slug = "PC", Description = "PC is the best", status = 1 };

                BrandModel apple = new BrandModel { Name = "Apple", Slug = "Apple", Description = "Apple is the best", status = 1 };
                BrandModel samsung = new BrandModel { Name = "SamSung", Slug = "SamSung", Description = "SamSung is the best", status = 1 };
                _context.Products.AddRange(
                    new ProductModel { Name = "Macbook", Slug = "Macbook", Description = "Macbook is the best", Image = "1.jpg", Category = macbook, Brand = apple, Price = 9 },
                    new ProductModel { Name = "PC", Slug = "PC", Description = "PC is the best", Image = "2.jpg", Category = pc, Brand = samsung, Price = 9 }
                );
                _context.SaveChanges();
            }
        }
		
	}
}
