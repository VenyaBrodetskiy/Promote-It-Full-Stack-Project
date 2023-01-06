using Microsoft.EntityFrameworkCore;
using dotNetBackend.Models;
using dotNetBackend.DTO;
using dotNetBackend.Common;
using System.ComponentModel.DataAnnotations;

namespace dotNetBackend.Services
{
    public class ProductToCampaignQtyService
    {
        private readonly MasaProjectDbContext _db;

        public ProductToCampaignQtyService(MasaProjectDbContext db)
        {
            _db = db;
        }

        public async Task<int> DecreaseNumOfProducts(TransactionDTO transactionInfo)
        {
            try
            {
                var productToCampaign = await _db.ProductToCampaignQties
                    .Where(row => row.ProductId == transactionInfo.ProductId && row.CampaignId == transactionInfo.CampaignId)
                    .FirstAsync();

                // TODO: (low priority) make possible to buy any number of products at once. Now just by 1 piece
                if (productToCampaign.ProductQty - 1 < 0)
                {
                    throw new ValidationException("Transaction cancelled. Not enough stock of product");
                }
                productToCampaign.ProductQty--;
                productToCampaign.UpdateDate = DateTime.Now;
                productToCampaign.UpdateUserId = transactionInfo.UserId;

                await _db.SaveChangesAsync();

                return productToCampaign.ProductQty;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<int> CreateProductToCampaignQty(ProductToCampaignQtyDTO productToCampaignQtyDTO)
        {
            try
            {

                var user = await _db.Users
                    .Where(row => row.Id == productToCampaignQtyDTO.UserId)
                    .FirstAsync();

                if (user.UserTypeId != (int)UserTypes.BusinessOwner)
                {
                    throw new ValidationException("Qty of products to campaign wasn`t added. Type of user is wrong");
                }


                var productToCampaignQty = FromDto(productToCampaignQtyDTO);

                _db.ProductToCampaignQties.Add(productToCampaignQty);

                await _db.SaveChangesAsync();

                return productToCampaignQty.Id;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private ProductToCampaignQty FromDto(ProductToCampaignQtyDTO productToCampaignQtyDTO)
        {
            return new ProductToCampaignQty()
            {
                Id = Const.NonExistId,
                ProductId = productToCampaignQtyDTO.ProductId,
                CampaignId = productToCampaignQtyDTO.CampaignId,
                ProductQty = productToCampaignQtyDTO.ProductQty,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateUserId = productToCampaignQtyDTO.UserId,
                UpdateUserId = productToCampaignQtyDTO.UserId,
                StatusId = (int)Statuses.Active
            };
        }

    }
}
