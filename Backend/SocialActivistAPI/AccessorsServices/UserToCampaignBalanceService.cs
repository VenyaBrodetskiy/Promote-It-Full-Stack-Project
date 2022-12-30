using Microsoft.EntityFrameworkCore;
using SocialActivistAPI.Models;
using SocialActivistAPI.DTO;
using SocialActivistAPI.Common;

namespace SocialActivistAPI.Services
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

        private static UserToCampaignBalanceDTO ToDto(UserToCampaignBalance userToCampaign, Campaign campaign, SocialActivist socialActivist)
        {
            return new UserToCampaignBalanceDTO()
            {
                Id = userToCampaign.Id,
                TwitterHandle = socialActivist.TwitterHandle,
                CampaignHashtag = campaign.Hashtag,
                Balance = userToCampaign.Balance,
                UpdateDate = userToCampaign.UpdateDate
            };
        }
    }
}
