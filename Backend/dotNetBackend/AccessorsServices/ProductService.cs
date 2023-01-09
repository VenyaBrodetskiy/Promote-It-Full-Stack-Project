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

        public async Task<string> GetOwnerByProductId(int id) 
        {
            try
            {
                return await _db.Products
                    .Where(p => p.Id == id)
                    .Select(p => p.User.BusinessOwnerUser.TwitterHandle)
                    .FirstAsync();
            }
            catch (Exception) 
            {
                throw;
            }
        }

        public async Task<int> AddProduct(ProductDTO productDTO)
        {
            try
            {
                // TODO: remove when we add Authorization / middleware
                User user = await _db.Users
                    .Where(row => row.Id == productDTO.UserId)
                    .FirstAsync();
                if (user.UserTypeId != (int)UserTypes.BusinessOwner)
                {
                    throw new ValidationException("Product wasn`t created. Type of user is wrong");
                }
                //

                Product product = FromDto(productDTO);

                _db.Products.Add(product);

                await _db.SaveChangesAsync();

                return product.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Product FromDto(ProductDTO productDTO)
        {
            return new Product()
            {
                Id = Const.NonExistId,
                UserId = productDTO.UserId,
                Title = productDTO.Title,
                Price = productDTO.Price,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateUserId = productDTO.UserId,
                UpdateUserId = productDTO.UserId,
                StatusId = (int)Statuses.Active
            };
        }

        
    }
}