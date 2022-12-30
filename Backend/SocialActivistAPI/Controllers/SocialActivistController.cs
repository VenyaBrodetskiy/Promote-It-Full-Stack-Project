using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialActivistAPI.Common;
using SocialActivistAPI.DTO;
using SocialActivistAPI.Models;
using SocialActivistAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace SocialActivistAPI.Controllers
{
    [ApiController]
    [Route($"{Const.BaseUrl}/[controller]")]
    public class SocialActivistController : ControllerBase
    {
        private readonly ILogger<SocialActivistController> _logger;
        private readonly SocialActivistService _socialActivistService;
        private readonly UserToCampaignBalanceService _userToCampaignService;
        public SocialActivistController(
            ILogger<SocialActivistController> logger, 
            SocialActivistService SAService, 
            UserToCampaignBalanceService UtCBService) 
        {
            _logger = logger;
            _socialActivistService = SAService;
            _userToCampaignService = UtCBService;
        }

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

        [HttpGet("balance/{UserId}")]
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
    }
}
