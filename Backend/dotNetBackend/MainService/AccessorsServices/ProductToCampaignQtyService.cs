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

        public async Task<ProductToCampaignQtyDTO> ChangeProductToCampaignQty(int businessOwnerId, ProductToCampaignQtyDTO productToCampaignQtyDTO)
        {
            try
            {
                // TODO: now UserId comes from ProductToCampaignQtyDTO
                // not sure that this approach is good. Maybe better to take UserId from token or pass as additional param

                var productToCampaignQtyExist = await _db.ProductToCampaignQties
                    .Where(row => row.ProductId == productToCampaignQtyDTO.ProductId
                               && row.CampaignId == productToCampaignQtyDTO.CampaignId)
                    .ToListAsync();
                
                if (productToCampaignQtyExist.Count == 0)
                {
                    var productToCampaignQty = FromDto(businessOwnerId, productToCampaignQtyDTO);

                    _db.ProductToCampaignQties.Add(productToCampaignQty);

                    await _db.SaveChangesAsync();

                    var result = ToDto(productToCampaignQty);

                    // return DTO type? without ID?
                    return result;
                }
                else
                {
                    productToCampaignQtyExist[0].ProductQty += productToCampaignQtyDTO.ProductQty;
                    productToCampaignQtyExist[0].UpdateDate = DateTime.Now;
                    productToCampaignQtyExist[0].UpdateUserId = businessOwnerId;

                    await _db.SaveChangesAsync();

                    var result = ToDto(productToCampaignQtyExist[0]);

                    // return DTO type? without ID?
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private ProductToCampaignQty FromDto(int businessOwnerId, ProductToCampaignQtyDTO productToCampaignQtyDTO)
        {
            return new ProductToCampaignQty()
            {
                Id = Const.NonExistId,
                ProductId = productToCampaignQtyDTO.ProductId,
                CampaignId = productToCampaignQtyDTO.CampaignId,
                ProductQty = productToCampaignQtyDTO.ProductQty,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateUserId = businessOwnerId,
                UpdateUserId = businessOwnerId,
                StatusId = (int)Statuses.Active
            };
        }

        private ProductToCampaignQtyDTO ToDto(ProductToCampaignQty productToCampaignQty)
        {
            return new ProductToCampaignQtyDTO()
            {
                ProductId = productToCampaignQty.ProductId,
                CampaignId = productToCampaignQty.CampaignId,
                ProductQty = productToCampaignQty.ProductQty
            };
        }

    }
}
