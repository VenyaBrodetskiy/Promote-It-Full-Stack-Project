using Microsoft.EntityFrameworkCore;
using dotNetBackend.Common;
using dotNetBackend.DTO;
using dotNetBackend.Models;
using System.Net;
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

        public async Task<int> CreateProduct(ProductDTO productDTO)
        {
            try
            {
                var user = await _db.Users
                    .Where(row => row.Id == productDTO.UserId)
                    .FirstAsync();

                if (user.UserTypeId != (int)UserTypes.BusinessOwner)
                {
                    throw new ValidationException("Product wasn`t created. Type of user is wrong");
                }


                var product = FromDto(productDTO);

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