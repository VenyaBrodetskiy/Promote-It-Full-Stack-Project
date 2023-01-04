using dotNetBackend.DTO;
using dotNetBackend.Common;
using System.ComponentModel.DataAnnotations;
using dotNetBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNetBackend.EngineServices
{
    public class ShippingValidationService
    {
        private readonly ILogger _logger;
        private readonly MasaProjectDbContext _db;
        public ShippingValidationService(ILogger<ShippingValidationService> logger, MasaProjectDbContext db) 
        {
            _logger = logger;
            _db = db;
        }

        public async void IsShippingPossible(int transactionId)
        {
            _logger.LogInformation("Validating transaction for shipping...");

            var transaction = await _db.Transactions
                .Where(row => row.Id == transactionId)
                .FirstAsync();

            if (transaction.StateId == (int)TransactionStates.Shipped ||
                transaction.StateId == (int)TransactionStates.Donated)
            {
                throw new ValidationException("Transaction State is not valid");
            }

        }


    }
}
