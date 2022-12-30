using Microsoft.AspNetCore.Mvc;
using SocialActivistAPI.AccessorsServices;
using SocialActivistAPI.Common;
using SocialActivistAPI.DTO;
using SocialActivistAPI.EngineServices;
using SocialActivistAPI.Models;
using SocialActivistAPI.Services;

namespace SocialActivistAPI.Controllers
{
    [ApiController]
    [Route($"{Const.BaseUrl}/[controller]")]
    public class SocialActivistController : ControllerBase
    {
        private readonly ILogger<SocialActivistController> _logger;
        private readonly SocialActivistService _socialActivistService;
        private readonly UserToCampaignBalanceService _userToCampaignService;
        private readonly TransactionService _transactionService;
        private readonly TransactionValidationService _transactionValidationService;
        public SocialActivistController(
            ILogger<SocialActivistController> logger, 
            SocialActivistService SAService, 
            UserToCampaignBalanceService UtCBService,
            TransactionService TService,
            TransactionValidationService TVService) 
        {
            _logger = logger;
            _socialActivistService = SAService;
            _userToCampaignService = UtCBService;
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
                var result = await _userToCampaignService.GetBalance(UserId);

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

            try
            {
                // steps:
                // 1. check if enough money to perform transaction
                // 2. create transaction
                // 3. change balance
                // 4. change N of products
                // 5. make sure 2-4 all done properly otherwise cancell all

                // 1. check if enough money to perform transaction
                bool result = await _transactionValidationService.IsTransactionPossible(transactionInfo);

                if (!result) 
                    return Forbid();
                _logger.LogInformation("1/5 Transaction: Validating transaction - OK");


                // 2. create transaction
                // TODO: need to check StatusId. If donated, flow will be different
                _logger.LogInformation("2/5 Transaction: Creating transaction...");
                var transaction = new Transaction()
                {
                    Id = Const.NonExistId,
                    UserId = transactionInfo.UserId,
                    ProductId = transactionInfo.ProductId,
                    StateId = transactionInfo.StateId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CreateUserId = transactionInfo.UserId,
                    UpdateUserId = transactionInfo.UserId,
                    StatusId = (int)Statuses.Active
                };

                var id = await _transactionService.CreateTransaction(transaction);

                if (id == null)
                {
                    return Problem("Failed to add transaction");
                }
                else
                {
                    _logger.LogInformation("2/5 Transaction: Creating transaction - OK");
                    return Ok(id);
                }

                // TODO: need to finish steps 3-5
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
