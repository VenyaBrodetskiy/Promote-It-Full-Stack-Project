using Microsoft.EntityFrameworkCore;
using SocialActivistAPI.Controllers;
using SocialActivistAPI.Models;

namespace SocialActivistAPI.Services
{
    public class SocialActivistService
    {
        private readonly MasaProjectDbContext _db;

        public SocialActivistService(MasaProjectDbContext db)
        {
            _db = db;
        }

        public async Task<List<SocialActivistLocal>> GetAll()
        {
            var result = await _db.SocialActivists
                .Select(sa => new SocialActivistLocal()
                {
                    UserId = sa.UserId,
                    TwitterHandle = sa.TwitterHandle,
                    Email = sa.Email,
                    Address = sa.Address,
                    PhoneNumber = sa.PhoneNumber
                })
                .ToListAsync();

            return result;

            // ask Alon how to make it async

            //var result2 = from sa in _db.SocialActivists
            //              select new SocialActivistLocal()
            //              {
            //                  UserId = sa.UserId,
            //                  TwitterHandle = sa.TwitterHandle,
            //                  UserIdFromUser = sa.User.Id,
            //                  UserType = sa.User.UserType.Title

            //              };
            //return result2.ToList();
        }

        public async Task<SocialActivistLocal?> Get(int id)
        {
            var result = await _db.SocialActivists.FindAsync(id);

            if (result == null)
            { 
                return null;

            }
            else
            {
                var resultFiltered = new SocialActivistLocal()
                {
                    UserId = result.UserId,
                    TwitterHandle = result.TwitterHandle,
                    Email = result.Email,
                    Address = result.Address,
                    PhoneNumber = result.PhoneNumber
                };

                return resultFiltered;
            }
        }
    }
}
