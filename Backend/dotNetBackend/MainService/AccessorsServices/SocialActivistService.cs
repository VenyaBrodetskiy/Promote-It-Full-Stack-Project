using Microsoft.EntityFrameworkCore;
using dotNetBackend.Models;
using dotNetBackend.DTO;
using dotNetBackend.Common;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace dotNetBackend.Services
{
    public class SocialActivistService
    {
        private readonly MasaProjectDbContext _db;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SocialActivistService> _logger;

        public SocialActivistService(MasaProjectDbContext db, HttpClient httpClient, ILogger<SocialActivistService> logger)
        {
            _db = db;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<SocialActivistDTO>> GetAll()
        {
            try
            {
                // removed direct access to DB
                // now here I send request to Accessor Layer service 
                _logger.LogInformation("Sending request to {accessor}", Endpoints.AccessorGetSocialActivists);

                var result = await _httpClient.GetAsync(Endpoints.AccessorGetSocialActivists);
                result.EnsureSuccessStatusCode();

                List<SocialActivistDTO>? socialActivists = await result.Content.ReadFromJsonAsync<List<SocialActivistDTO>>();
                if (socialActivists == null)
                {
                    _logger.LogError($"Couldn't read social activists from response body");
                    throw new Exception($"Couldn't read social activists from response body");
                }

                return socialActivists;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SocialActivistDTO?> Get(int id)
        {
            try
            {
                var result = await _db.SocialActivists
                    .Where(sa => sa.StatusId == (int)Statuses.Active && sa.UserId == id)
                    .Select(sa => ToDto(sa))
                    .FirstAsync();

                if (result == null)
                { 
                    return null;
                }
                else
                {
                    return result;
                }
            }
            catch (Exception) 
            {
                throw;  
            }
        }

        private static SocialActivistDTO ToDto(SocialActivist socialActivist)
        {
            return new SocialActivistDTO()
            {
                UserId = socialActivist.UserId,
                TwitterHandle = socialActivist.TwitterHandle,
                Email = socialActivist.Email,
                Address = socialActivist.Address,
                PhoneNumber = socialActivist.PhoneNumber
            };
        }
    }
}
