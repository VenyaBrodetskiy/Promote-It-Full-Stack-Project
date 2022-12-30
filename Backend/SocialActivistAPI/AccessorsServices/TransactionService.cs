using SocialActivistAPI.Common;
using SocialActivistAPI.DTO;
using SocialActivistAPI.Models;

namespace SocialActivistAPI.AccessorsServices
{
    public class TransactionService
    {
        private readonly MasaProjectDbContext _db;

        public TransactionService(MasaProjectDbContext db)
        {
            _db = db;
        }

        public async Task<int> CreateTransaction(Transaction transaction)
        {
            _db.Transactions.Add(transaction);

            await _db.SaveChangesAsync();

            return transaction.Id;
        }
    }
}