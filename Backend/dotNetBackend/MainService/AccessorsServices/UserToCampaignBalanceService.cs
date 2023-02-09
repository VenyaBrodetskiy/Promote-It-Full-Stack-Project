

using Microsoft.EntityFrameworkCore;
using dotNetBackend.Models;
using dotNetBackend.DTO;
using dotNetBackend.Common;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using AutoServiceRegistrator;

namespace dotNetBackend.Services
{
    [ScopedService]
    public class UserToCampaignBalanceService
    {
        private readonly MasaProjectDbContext _db;
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserToCampaignBalanceService> _logger;

        public UserToCampaignBalanceService(MasaProjectDbContext db, HttpClient httpClient, ILogger<UserToCampaignBalanceService> logger)
        {
            _db = db;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<UserToCampaignBalanceDTO>> GetUserBalances(int UserId)
        {
            try
            {
                var result = from userToCampaign in _db.UserToCampaignBalances
                             where userToCampaign.UserId == UserId 
                                && userToCampaign.StatusId == (int)Statuses.Active
                             join campaign in _db.Campaigns on userToCampaign.CampaignId equals campaign.Id
                             join socialActivist in _db.SocialActivists on userToCampaign.UserId equals socialActivist.UserId
                             select ToDto(userToCampaign, socialActivist.TwitterHandle, campaign.Hashtag);

                return await result.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<List<UserToCampaignBalanceDTO>> GetBalance(int UserId, int CampaignId)
        {
            try
            {
                string? campaignHashtag = await _db.Campaigns
                    .Where(c => c.Id == CampaignId)
                    .Select(c => c.Hashtag)
                    .FirstOrDefaultAsync();

                var userBalances = await GetUserBalances(UserId);

                var result = userBalances
                    .Where(user => user.CampaignHashtag == campaignHashtag)
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<UserToCampaignBalanceDTO> AddNewBalance(UserToCampaignTwitterInfo userToCampaign)
        {
            try
            {
                var newLine = new UserToCampaignBalance()
                {
                    Id = Const.NonExistId,
                    UserId = userToCampaign.UserId,
                    CampaignId = userToCampaign.CampaignId,
                    Balance = userToCampaign.CurrentTweetCount,
                    PreviousTweetCount = userToCampaign.CurrentTweetCount,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CreateUserId = userToCampaign.UserId,
                    UpdateUserId = userToCampaign.UserId,
                    StatusId = (int)Statuses.Active
                };

                _db.UserToCampaignBalances.Add(newLine);
                await _db.SaveChangesAsync();

                var userTwitterHandle = await _db.SocialActivists.Where(user => user.UserId == userToCampaign.UserId).Select(user => user.TwitterHandle).FirstOrDefaultAsync();
                var campaignHashtag = await _db.Campaigns.Where(c => c.Id == userToCampaign.CampaignId).Select(c => c.Hashtag).FirstOrDefaultAsync();

                return ToDto(newLine, userTwitterHandle, campaignHashtag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<string> ChangeBalance(UserToCampaignTwitterInfo userToCampaign)
        {
            try
            {
                // removed direct access to DB
                // now here I send request to Accessor Layer service 
                _logger.LogInformation("Sending request to {accessor}", Endpoints.AccessorGetSocialActivists);

                var result = await _httpClient.PutAsJsonAsync(Endpoints.AccessorChangeBalance, userToCampaign);
                result.EnsureSuccessStatusCode();

                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
            catch (Exception ex) 
            { 
                _logger.LogError(ex.Message); 
                throw; 
            }
        }

        private static UserToCampaignBalanceDTO ToDto(UserToCampaignBalance userToCampaign, string? userTwitterHandle, string? campaignHashtag)
        {
            return new UserToCampaignBalanceDTO()
            {
                Id = userToCampaign.Id,
                TwitterHandle = userTwitterHandle,
                CampaignHashtag = campaignHashtag,
                Balance = userToCampaign.Balance,
                PreviousTweetCount = userToCampaign.PreviousTweetCount,
                UpdateDate = userToCampaign.UpdateDate
            };
        }
    }
}
