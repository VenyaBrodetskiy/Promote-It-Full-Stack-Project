using Microsoft.AspNetCore.Mvc;
using dotNetBackend.AccessorsServices;
using dotNetBackend.Common;
using dotNetBackend.DTO;
using dotNetBackend.EngineServices;
using dotNetBackend.Models;
using dotNetBackend.Services;
using System.ComponentModel.DataAnnotations;

namespace dotNetBackend.Controllers
{
    [ApiController]
    [Route($"{Const.BaseUrl}/[controller]")]
    public class SocialActivistController : ControllerBase
    {
        private readonly ILogger<SocialActivistController> _logger;
        private readonly MasaProjectDbContext _db;
        private readonly SocialActivistService _socialActivistService;
        private readonly UserToCampaignBalanceService _userToCampaignService;
        private readonly ProductToCampaignQtyService _productToCampaignService;
        private readonly TransactionService _transactionService;
        private readonly TransactionValidationService _transactionValidationService;
        public SocialActivistController(
            ILogger<SocialActivistController> logger,
            MasaProjectDbContext db,
            SocialActivistService SAService, 
            UserToCampaignBalanceService UtCBService,
            ProductToCampaignQtyService PtCQtyService,
            TransactionService TService,
            TransactionValidationService TVService) 
        {
            _logger = logger;
            _db = db;
            _socialActivistService = SAService;
            _userToCampaignService = UtCBService;
            _productToCampaignService = PtCQtyService;
            _transactionService = TService;
            _transactionValidationService = TVService;
        }

        // this route is just for example and testing
        [HttpGet]
        public async Task<ActionResult<List<SocialActivistDTO>>> GetAll()
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {
                var result = await _socialActivistService.GetAll();

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        // this route is just for example and testing
        [HttpGet("{id}")]
        public async Task<ActionResult<SocialActivistDTO>> Get(int id)
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {
                var result = await _socialActivistService.Get(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);

                }
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }

        }

        [HttpGet("[action]/{UserId}")]
        public async Task<ActionResult<UserToCampaignBalanceDTO>> GetBalance(int UserId)
        {
            // no validation of UserId here, it should do middleware (here on in Node.js server)
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {
                var result = await _userToCampaignService.GetUserBalances(UserId);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserToCampaignBalanceDTO>> CreateTransaction(TransactionDTO transactionInfo)
        {
            // no validation of UserId here, it should do middleware (here on in Node.js server)

            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            // steps:
            // 1. check if enough money to perform transaction
            // 2. create transaction
            // 3. change balance
            // 4. change N of products
            // 5. make sure 2-4 all done properly otherwise cancell all

            // 1. check if enough money to perform transaction
            try
            {
                var validationResult = _transactionValidationService.IsTransactionPossible(transactionInfo);

                if (validationResult)
                {
                    _logger.LogInformation("1/4 Transaction: Validating transaction - OK");
                }
                else
                {
                    _logger.LogInformation("1/4 Transaction: Validating transaction - Failed");
                    return BadRequest("Transaction State is not valid");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            _logger.LogInformation("Creating transaction...");
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // 2. create transaction
                    var id = await _transactionService.CreateTransaction(transactionInfo);
                    _logger.LogInformation("2/4 Transaction: New transaction added - OK");

                    // 3. change balance
                    var newBalance = await _userToCampaignService.DecreaseBalance(transactionInfo);
                    _logger.LogInformation("3/4 Transaction: Balance decreased - OK");

                    // 4. change N of products (if user donates product, don't need to change N of products)
                    if (transactionInfo.StateId == (int)TransactionStates.Ordered)
                    {
                        var newNumOfProducts = await _productToCampaignService.DecreaseNumOfProducts(transactionInfo);
                        _logger.LogInformation("4/4 Transaction: Number of products decreased - OK");
                    }
                    else
                    {
                        _logger.LogInformation("4/4 Transaction: Number of products didn't change, because product was donated - OK");
                    }

                    dbContextTransaction.Commit();
                    return Ok(new
                    {
                        TransactionId = id,
                        NewBalance = newBalance
                    });
                }
                catch (ValidationException ex)
                {
                    _logger.LogError("Transaction failed. Rolling back all changes...");
                    dbContextTransaction.Rollback();

                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Transaction failed. Rolling back all changes...");
                    dbContextTransaction.Rollback();

                    return Problem(ex.Message);
                }
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserToCampaignBalanceDTO>> UpdateUserBalance(UserToCampaignTwitterInfo userToCampaign)
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {
                var balance = await _userToCampaignService.GetBalance(userToCampaign.UserId, userToCampaign.CampaignId);
                if (balance.Count == 0)
                {
                    _logger.LogInformation("User {userToCampaign.UserId} doesn't have balance for Campaign {userToCampaign.CampaignId}", userToCampaign.UserId, userToCampaign.CampaignId);
                    
                    var result = await _userToCampaignService.AddNewBalance(userToCampaign);
                    _logger.LogInformation("Created new line in UserToCampaignBalance: {userTwitter}", result.TwitterHandle);

                    return Ok(result);
                }
                else
                {
                    _logger.LogInformation("User {userToCampaign.UserId} already has balance for Campaign {userToCampaign.CampaignId}", userToCampaign.UserId, userToCampaign.CampaignId);

                    var result = await _userToCampaignService.ChangeBalance(userToCampaign);
                    _logger.LogInformation("Updated user balance: {userTwitter}", result.TwitterHandle);

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
