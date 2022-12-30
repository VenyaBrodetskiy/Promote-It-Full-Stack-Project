using Microsoft.EntityFrameworkCore;
using SocialActivistAPI.Models;
using SocialActivistAPI.DTO;
using SocialActivistAPI.Common;

namespace SocialActivistAPI.Services
{
    public class SocialActivistService
    {
        private readonly MasaProjectDbContext _db;

        public SocialActivistService(MasaProjectDbContext db)
        {
            _db = db;
        }

        public async Task<List<SocialActivistDTO>> GetAll()
        {
            try
            {
                var result = await _db.SocialActivists
                    .Where(sa => sa.StatusId == (int)Statuses.Active)
                    .Select(sa => ToDto(sa))
                    .ToListAsync();

                return result;
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
