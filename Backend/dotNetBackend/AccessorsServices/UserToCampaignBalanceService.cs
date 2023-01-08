

using Microsoft.EntityFrameworkCore;
using dotNetBackend.Models;
using dotNetBackend.DTO;
using dotNetBackend.Common;
using System.ComponentModel.DataAnnotations;

namespace dotNetBackend.Services
{
    public class UserToCampaignBalanceService
    {
        private readonly MasaProjectDbContext _db;
        public UserToCampaignBalanceService(MasaProjectDbContext db)
        {
            _db = db;
        }

        public async Task<List<UserToCampaignBalanceDTO>> GetBalance(int UserId)
        {
            try
            {
                var result = from userToCampaign in _db.UserToCampaignBalances
                             where userToCampaign.UserId == UserId 
                                && userToCampaign.StatusId == (int)Statuses.Active
                             join campaign in _db.Campaigns on userToCampaign.CampaignId equals campaign.Id
                             join socialActivist in _db.SocialActivists on userToCampaign.UserId equals socialActivist.UserId
                             select ToDto(userToCampaign, campaign, socialActivist);

                return await result.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DecreaseBalance(TransactionDTO transactionInfo)
        {
            try
            {
                var userToCampaign = await _db.UserToCampaignBalances
                    .Where(row => row.UserId == transactionInfo.UserId && row.CampaignId == transactionInfo.CampaignId)
                    .FirstAsync();

                // TODO: maybe here better to call accessor of Products table
                var priceOfProduct = await _db.Products.FindAsync(transactionInfo.ProductId);

                if (priceOfProduct == null)
                {
                    throw new ValidationException("Couldn't find product by id");
                }

                if (userToCampaign.Balance - priceOfProduct.Price < 0)
                {
                    throw new ValidationException("Transaction cancelled. Not enough money");
                }

                userToCampaign.Balance -= priceOfProduct.Price;
                userToCampaign.UpdateDate = DateTime.Now;
                userToCampaign.UpdateUserId = transactionInfo.UserId;

                await _db.SaveChangesAsync();

                return userToCampaign.Balance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static UserToCampaignBalanceDTO ToDto(UserToCampaignBalance userToCampaign, Campaign campaign, SocialActivist socialActivist)
        {
            return new UserToCampaignBalanceDTO()
            {
                Id = userToCampaign.Id,
                TwitterHandle = socialActivist.TwitterHandle,
                CampaignHashtag = campaign.Hashtag,
                Balance = userToCampaign.Balance,
                PreviousTweetCount = userToCampaign.PreviousTweetCount,
                UpdateDate = userToCampaign.UpdateDate
            };
        }
    }
}
