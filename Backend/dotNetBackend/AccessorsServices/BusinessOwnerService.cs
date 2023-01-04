using Microsoft.EntityFrameworkCore;
using dotNetBackend.Models;
using dotNetBackend.DTO;
using dotNetBackend.Common;

namespace dotNetBackend.Services
{
    public class BusinessOwnerService
    {
        private readonly MasaProjectDbContext _db;

        public BusinessOwnerService(MasaProjectDbContext db)
        {
            _db = db;
        }

        public async Task<List<BusinessOwnerDTO>> GetAll()
        {
            try
            {
                var result = await _db.BusinessOwners
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

        public async Task<BusinessOwnerDTO?> Get(int id)
        {
            try
            {
                var result = await _db.BusinessOwners
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

        private static BusinessOwnerDTO ToDto(BusinessOwner businessOwner)
        {
            return new BusinessOwnerDTO()
            {
                UserId = businessOwner.UserId,
                TwitterHandle = businessOwner.TwitterHandle,
                Name = businessOwner.Name
            };
        }
    }
}
