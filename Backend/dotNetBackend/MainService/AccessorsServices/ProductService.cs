using Microsoft.EntityFrameworkCore;
using dotNetBackend.Common;
using dotNetBackend.DTO;
using dotNetBackend.Models;
using System.ComponentModel.DataAnnotations;
using AutoServiceRegistrator;

namespace dotNetBackend.AccessorsServices
{
    [ScopedService]
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
                    .Select(p => p.User.BusinessOwnerUser!.TwitterHandle)
                    .FirstAsync();
            }
            catch (Exception) 
            {
                throw;
            }
        }

        public async Task<List<ProductDTO>> GetProductList(int businessOwnerId)
        {
            try
            {
                var product = await _db.Products
                    .Where(p => p.UserId == businessOwnerId
                             && p.StatusId == (int)Statuses.Active)
                    .ToListAsync();

                var result = ToDto(product);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> AddProduct(int businessOwnerId, NewProductDTO newProductDTO)
        {
            try
            {
                Product product = FromDto(businessOwnerId, newProductDTO);

                _db.Products.Add(product);

                await _db.SaveChangesAsync();

                return product.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Product FromDto(int businessOwnerId, NewProductDTO newProductDTO)
        {
            return new Product()
            {
                Id = Const.NonExistId,
                UserId = businessOwnerId,
                Title = newProductDTO.Title,
                Price = newProductDTO.Price,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateUserId = businessOwnerId,
                UpdateUserId = businessOwnerId,
                StatusId = (int)Statuses.Active
            };
        }

        private List<ProductDTO> ToDto(List<Product> product)
        {
            return product.Select(p => new ProductDTO()
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price
            }).ToList();
        }


    }
}