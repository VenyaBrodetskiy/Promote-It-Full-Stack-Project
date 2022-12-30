using SocialActivistAPI.DTO;

namespace SocialActivistAPI.EngineServices
{
    public class TransactionValidationService
    {
        private readonly ILogger _logger;
        public TransactionValidationService(ILogger<TransactionValidationService> logger) 
        {
            _logger = logger;
        }

        public async Task<bool> IsTransactionPossible(TransactionDTO transactionInfo)
        {
            _logger.LogInformation("1/5 Transaction: Validating transaction...");

            // not implemented yet
            await Task.Delay(100);
            return true;
        }
    }
}
