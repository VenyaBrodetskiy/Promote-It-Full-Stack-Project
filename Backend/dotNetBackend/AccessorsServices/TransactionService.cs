using Microsoft.EntityFrameworkCore;
using dotNetBackend.Common;
using dotNetBackend.DTO;
using dotNetBackend.Models;
using System.Net;

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

        public async Task<List<OrderDTO>> GetOrdered(int businessOwnerId)
        {

            try
            {
                var result = from transaction in _db.Transactions

                             join socialActivist in _db.SocialActivists on transaction.UserId equals socialActivist.UserId
                             join product in _db.Products on transaction.ProductId equals product.Id
                             join transactionState in _db.TransactionStates on transaction.StateId equals transactionState.Id

                             where product.UserId == businessOwnerId
                                    && transaction.StateId == (int)TransactionStates.Ordered
                                    && transaction.StatusId == (int)Statuses.Active

                             select ToDto(transaction, socialActivist, product, transactionState);

                return await result.ToListAsync();
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


        private static OrderDTO ToDto(Transaction transaction, SocialActivist socialActivist, Product product, TransactionState transactionState)
        {
            return new OrderDTO()
            {
                UserId = transaction.UserId,
                TwitterHandle = socialActivist.TwitterHandle,
                Email = socialActivist.Email,
                Address = socialActivist.Address,
                PhoneNumber = socialActivist.PhoneNumber,
                ProductId = transaction.ProductId,
                ProductTitle = product.Title,
                TransactionState = transactionState.Title

            };
        }

    }
}