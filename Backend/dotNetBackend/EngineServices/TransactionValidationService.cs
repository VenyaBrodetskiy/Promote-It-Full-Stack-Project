using dotNetBackend.DTO;
using dotNetBackend.Common;
using System.ComponentModel.DataAnnotations;

namespace dotNetBackend.EngineServices
{
    public class TransactionValidationService
    {
        private readonly ILogger _logger;
        public TransactionValidationService(ILogger<TransactionValidationService> logger) 
        {
            _logger = logger;
        }

        public void IsTransactionPossible(TransactionDTO transactionInfo)
        {
            _logger.LogInformation("1/4 Transaction: Validating transaction...");

            if (transactionInfo.StateId == (int)TransactionStates.Shipped)
            {
                throw new ValidationException("Transaction State is not valid");
            }

            // no need to check balance and N of products, because they are checked in
            // UserToCampaignBalance service and in ProductsToCampaignQty service
        }


    }
}
