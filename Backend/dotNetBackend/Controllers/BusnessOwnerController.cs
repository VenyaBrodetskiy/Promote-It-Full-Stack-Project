using Microsoft.AspNetCore.Mvc;
using dotNetBackend.AccessorsServices;
using dotNetBackend.Common;
using dotNetBackend.DTO;
using dotNetBackend.EngineServices;
using dotNetBackend.Services;
using Microsoft.AspNetCore.Authorization;
using dotNetBackend.Helpers;

namespace dotNetBackend.Controllers
{
    [ApiController]
    [Route($"{Const.BaseUrl}/[controller]/[action]")]
    [Authorize(Policy = Policies.BusinessOwner)]
    public class BusinessOwnerController : ControllerBase
    {
        private readonly ILogger<BusinessOwnerController> _logger;
        private readonly BusinessOwnerService _businessOwnerService;
        private readonly TransactionService _transactionService;
        private readonly ProductService _productService;
        private readonly ProductToCampaignQtyService _productToCampaignQtyService;
        private readonly DonationValidationService _donationValidationService;

        public BusinessOwnerController(
            ILogger<BusinessOwnerController> logger,
            BusinessOwnerService BOService,
            TransactionService TService,
            ProductService PService,
            ProductToCampaignQtyService PTCService,
            DonationValidationService DVService
            )
        {
            _logger = logger;
            _businessOwnerService = BOService;
            _transactionService = TService;
            _productService = PService;
            _productToCampaignQtyService = PTCService;
            _donationValidationService = DVService;
        }

        // this route is just for example and testing
        [HttpGet]
        public async Task<ActionResult<List<BusinessOwnerDTO>>> GetAll()
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {
                var result = await _businessOwnerService.GetAll();

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
        [HttpGet]
        public async Task<ActionResult<BusinessOwnerDTO>> Get()
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            int businessOwnerId = HttpHelper.GetUserId(HttpContext);

            _logger.LogInformation("User id from token: {UserId}", businessOwnerId);

            try
            {
                var result = await _businessOwnerService.Get(businessOwnerId);

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

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            int businessOwnerId = HttpHelper.GetUserId(HttpContext);

            _logger.LogInformation("User id from token: {UserId}", businessOwnerId);

            try
            {
                var result = await _transactionService.GetOrderList(businessOwnerId);

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

        [HttpPut]
        public async Task<ActionResult<OrderDTO>> ChangeTransactionState(TransactionChangeState transactionInfo)
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            int businessOwnerId = HttpHelper.GetUserId(HttpContext);

            _logger.LogInformation("User id from token: {UserId}", businessOwnerId);

            try
            {
                var result = await _transactionService.ChangeTransactionState(businessOwnerId, transactionInfo);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (InvalidOperationException)
            {
                return NotFound("Invalid input data");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct(ProductDTO productInfo)
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            int businessOwnerId = HttpHelper.GetUserId(HttpContext);

            _logger.LogInformation("User id from token: {UserId}", businessOwnerId);

            try
            {
                var id = await _productService.AddProduct(businessOwnerId, productInfo);
                _logger.LogInformation("New product added - OK");

                // TODO: probably in future better to return ProductDTO type with id
                return Ok(id);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> DonateProductsToCampaign(ProductToCampaignQtyDTO productToCampaignQtyDTO)
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            int businessOwnerId = HttpHelper.GetUserId(HttpContext);

            _logger.LogInformation("User id from token: {UserId}", businessOwnerId);

            try
            {
                var validationResult = await _donationValidationService.IsDonationPossible(businessOwnerId, productToCampaignQtyDTO);
                if (validationResult)
                {
                    _logger.LogInformation("Validating donation - OK");
                }
                else
                {
                    _logger.LogInformation("Validating donation - Failed");
                    return BadRequest("Qty of products to campaign wasn`t added. This product doesn`t belong to this user");
                }

                var result = await _productToCampaignQtyService.ChangeProductToCampaignQty(businessOwnerId, productToCampaignQtyDTO);
                _logger.LogInformation("Qty of products to campaign added - OK");

                return Ok(result);

            }
            catch (InvalidOperationException)
            {
                return NotFound("Invalid input data");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
