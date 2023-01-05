using Microsoft.EntityFrameworkCore;
using dotNetBackend.Common;
using dotNetBackend.DTO;
using dotNetBackend.Models;
using System.Net;
using System.ComponentModel.DataAnnotations;


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

        public async Task<List<OrderDTO>> GetOrderList(int businessOwnerId)
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

                             select ToOrderDto(transaction, socialActivist, product, transactionState);

                return await result.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<OrderDTO> ChangeTransactionState(int businessOwnerId, int transactionId)
        {
            try
            {
                var transaction = await _db.Transactions
                    .Where(row => row.Id == transactionId 
                        && row.Product.UserId == businessOwnerId 
                        && row.StateId == (int)TransactionStates.Ordered)
                    .FirstAsync();


                transaction.StateId = (int)TransactionStates.Shipped;
                transaction.UpdateUserId = businessOwnerId;
                transaction.UpdateDate = DateTime.Now;

                await _db.SaveChangesAsync();

                var socialActivist = await _db.SocialActivists
                    .Where(sa => sa.UserId == transaction.UserId)
                    .FirstAsync();

                var product = await _db.Products
                    .Where(p => p.Id == transaction.ProductId)
                    .FirstAsync();

                var transactionState = await _db.TransactionStates
                    .Where(ts => ts.Id == transaction.StateId)
                    .FirstAsync();

                var result = ToOrderDto(transaction, socialActivist, product, transactionState);

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


        private static OrderDTO ToOrderDto(Transaction transaction, SocialActivist socialActivist, Product product, TransactionState transactionState)
        {
            return new OrderDTO()
            {
                Id = transaction.Id,
                UserId = transaction.UserId,
                TwitterHandle = socialActivist.TwitterHandle,
                Email = socialActivist.Email,
                Address = socialActivist.Address,
                PhoneNumber = socialActivist.PhoneNumber,
                ProductId = transaction.ProductId,
                ProductTitle = product.Title,
                ProductOwnerId = product.UserId,
                TransactionState = transactionState.Title

            };
        }

    }
}