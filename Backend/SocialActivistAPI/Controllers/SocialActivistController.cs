using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialActivistAPI.Common;
using SocialActivistAPI.DTO;
using SocialActivistAPI.Services;

namespace SocialActivistAPI.Controllers
{
    [ApiController]
    [Route($"{Const.BaseUrl}/[controller]")]
    public class SocialActivistController : ControllerBase
    {
        private readonly SocialActivistService _socialActivistService;
        public SocialActivistController(SocialActivistService service) 
        {
            _socialActivistService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<SocialActivistDTO>>> GetAll()
        {
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
}
