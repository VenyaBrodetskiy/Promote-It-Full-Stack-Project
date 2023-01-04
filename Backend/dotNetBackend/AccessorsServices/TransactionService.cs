using Microsoft.EntityFrameworkCore.Storage;
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
    }
}