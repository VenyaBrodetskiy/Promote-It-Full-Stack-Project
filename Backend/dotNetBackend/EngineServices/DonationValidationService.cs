using dotNetBackend.DTO;
using dotNetBackend.Common;
using System.ComponentModel.DataAnnotations;
using dotNetBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNetBackend.EngineServices
{
    public class DonationValidationService
    {
        private readonly ILogger _logger;
        private readonly MasaProjectDbContext _db;
        public DonationValidationService(ILogger<DonationValidationService> logger, MasaProjectDbContext db) 
        {
            _logger = logger;
            _db = db;
        }

        public async Task<bool> IsDonationPossible(int businessOwnerId, ProductToCampaignQtyDTO productToCampaignQtyDTO)
        {
            _logger.LogInformation("Validating donation...");

            var product = await _db.Products
                    .Where(row => row.Id == productToCampaignQtyDTO.ProductId)
                    .ToListAsync();

            if (product[0].UserId != businessOwnerId)
            {
                return false;
            }

            return true;

        }


    }
}
