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
    }
}
