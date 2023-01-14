using Microsoft.EntityFrameworkCore;
using dotNetBackend.Common;
using dotNetBackend.DTO;
using dotNetBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace dotNetBackend.AccessorsServices
{
    public class ProductService
    {
        private readonly MasaProjectDbContext _db;

        public ProductService(MasaProjectDbContext db)
        {
            _db = db;
        }

        public async Task<string> GetOwnerByProductId(int productId) 
        {
            try
            {
                return await _db.Products
                    .Where(p => p.Id == productId)
                    .Select(p => p.User.BusinessOwnerUser.TwitterHandle)
                    .FirstAsync();
            }
            catch (Exception) 
            {
                throw;
            }
        }

        public async Task<int> AddProduct(int businessOwnerId, ProductDTO productDTO)
        {
            try
            {
                Product product = FromDto(businessOwnerId, productDTO);

                _db.Products.Add(product);

                await _db.SaveChangesAsync();

                return product.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Product FromDto(int businessOwnerId, ProductDTO productDTO)
        {
            return new Product()
            {
                Id = Const.NonExistId,
                UserId = businessOwnerId,
                Title = productDTO.Title,
                Price = productDTO.Price,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateUserId = businessOwnerId,
                UpdateUserId = businessOwnerId,
                StatusId = (int)Statuses.Active
            };
        }

        
    }
}