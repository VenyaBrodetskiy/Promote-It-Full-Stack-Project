//using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using dotNetBackend.Common;
using dotNetBackend.DTO;
using dotNetBackend.Models;

namespace dotNetBackend.AccessorsServices
{
    public class TransactionService
    {
        private readonly MasaProjectDbContext _db;

        public TransactionService(MasaProjectDbContext db)
        {
            _db = db;
        }

        public async Task<int> CreateTransaction(TransactionDTO transactionDTO)
        {
            try
            {
                var transaction = FromDto(transactionDTO);

                _db.Transactions.Add(transaction);

                await _db.SaveChangesAsync();

                return transaction.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransactionDTO>> GetOrdered(int businessOwnerId)
        {

            try
            {
                var result = await _db.Transactions
                    .Where(t => t.Product.UserId == businessOwnerId)
                    .Where(t => t.StateId == (int)TransactionStates.Ordered)
                    .Where(t => t.StatusId == (int)Statuses.Active)
                    .Select(t => ToDto(t))
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private Transaction FromDto(TransactionDTO transactionDTO)
        {
            return new Transaction()
            {
                Id = Const.NonExistId,
                UserId = transactionDTO.UserId,
                ProductId = transactionDTO.ProductId,
                CampaignId = transactionDTO.CampaignId,
                StateId = transactionDTO.StateId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreateUserId = transactionDTO.UserId,
                UpdateUserId = transactionDTO.UserId,
                StatusId = (int)Statuses.Active
            };
        }

        private static TransactionDTO ToDto(Transaction transaction)
        {
            return new TransactionDTO()
            {
                UserId = transaction.UserId,
                ProductId = transaction.ProductId,
                CampaignId = transaction.CampaignId,
                StateId = transaction.StateId
            };
        }
    }
}