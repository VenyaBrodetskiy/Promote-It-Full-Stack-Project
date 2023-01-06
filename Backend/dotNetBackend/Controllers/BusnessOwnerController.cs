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
    public class BusinessOwnerController : ControllerBase
    {
        private readonly ILogger<BusinessOwnerController> _logger;
        private readonly MasaProjectDbContext _db;
        private readonly BusinessOwnerService _businessOwnerService;
        private readonly TransactionService _transactionService;
        private readonly ShippingValidationService _shippingValidationService;
        private readonly ProductService _productService;
        private readonly ProductToCampaignQtyService _productToCampaignQtyService;
        //private readonly UserToCampaignBalanceService _userToCampaignService;

        public BusinessOwnerController(
            ILogger<BusinessOwnerController> logger,
            MasaProjectDbContext db,
            BusinessOwnerService BOService,
            TransactionService TService,
            ShippingValidationService SVService,
            ProductService PService,
            ProductToCampaignQtyService PTCService
            //UserToCampaignBalanceService UtCBService,
            )
        {
            _logger = logger;
            _db = db;
            _businessOwnerService = BOService;
            _transactionService = TService;
            _shippingValidationService = SVService;
            _productService = PService;
            _productToCampaignQtyService = PTCService;
            //_userToCampaignService = UtCBService;


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
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessOwnerDTO>> Get(int id)
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {
                var result = await _businessOwnerService.Get(id);

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

        [HttpGet("[action]")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders(int businessOwnerId)
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

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

        [HttpPut("[action]")]

        public async Task<ActionResult<OrderDTO>> ChangeTransactionState(int businessOwnerId, int transactionId)
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {
                var result = await _transactionService.ChangeTransactionState(businessOwnerId, transactionId);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (ValidationException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }


        [HttpPost("[action]")]
        public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO productInfo)
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {

                var id = await _productService.CreateProduct(productInfo);
                _logger.LogInformation("New product added - OK");

                
                return Ok(new
                {
                    ProductId = id,
                    productInfo = productInfo
                });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ProductDTO>> CreateProductToCampaignQty(ProductToCampaignQtyDTO productToCampaignQtyDTO)
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {

                var id = await _productToCampaignQtyService.CreateProductToCampaignQty(productToCampaignQtyDTO);
                _logger.LogInformation("Qty of products to campaign added - OK");


                return Ok(new
                {
                    Id = id,
                    Info = productToCampaignQtyDTO
                });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }


    }
}
