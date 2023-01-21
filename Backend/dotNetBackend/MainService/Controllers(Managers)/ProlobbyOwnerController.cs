using dotNetBackend.Common;
using dotNetBackend.Controllers;
using dotNetBackend.DTO;
using dotNetBackend.Services;
using MainService.EngineServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Controllers
{
    [ApiController]
    [Route($"{Const.BaseUrl}/[controller]/[action]")]
    public class ProlobbyOwnerController : ControllerBase
    {
        private readonly ILogger<ProlobbyOwnerController> _logger;
        private readonly HttpClient _httpClient;
        private readonly TwitterEngineService _twitterEngine;

        public ProlobbyOwnerController(
            ILogger<ProlobbyOwnerController> logger,
            HttpClient httpClient,
            TwitterEngineService twitterEngine)
        {
            _logger = logger;
            _httpClient = httpClient;
            _twitterEngine = twitterEngine;
        }

        [HttpGet]
        [Authorize(Policy = Policies.ProlobbyOwner)]
        public async Task<ActionResult<List<TweetDTO>>> GetAllTweets()
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {
                _logger.LogInformation("Asking twitterEngine to handle request");
                var result = await _twitterEngine.GetAllTweets();

                if (result == null)
                {
                    return Ok();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
