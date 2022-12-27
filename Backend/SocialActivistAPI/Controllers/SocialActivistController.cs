using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialActivistAPI.Models;
using SocialActivistAPI.Services;

namespace SocialActivistAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SocialActivistController : ControllerBase
    {
        private readonly SocialActivistService _socialActivistService;
        public SocialActivistController(SocialActivistService service) 
        {
            _socialActivistService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<SocialActivistLocal>>> GetAll()
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

        [HttpGet("{id}")]
        public async Task<ActionResult<SocialActivistLocal>> Get(int id)
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
    }

    public class SocialActivistLocal
    {
        public int UserId { get; set; }
        public string? TwitterHandle { get; set; }
        public string? Email {get; set;}
        public string? Address { get; set;}
        public string? PhoneNumber { get; set;}
    }
}
