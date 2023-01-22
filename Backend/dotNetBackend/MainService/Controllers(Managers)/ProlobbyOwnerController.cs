using dotNetBackend.Common;
using dotNetBackend.Controllers;
using dotNetBackend.DTO;
using dotNetBackend.Services;
using MainService.EngineServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace MainService.Controllers
{
    [ApiController]
    [Route($"{Const.BaseUrl}/[controller]/[action]")]
    public class ProlobbyOwnerController : ControllerBase
    {
        private readonly ILogger<ProlobbyOwnerController> _logger;
        private readonly TwitterEngineService _twitterEngine;
        private readonly TimerService _timerService;

        public ProlobbyOwnerController(
            ILogger<ProlobbyOwnerController> logger,
            TwitterEngineService twitterEngine,
            TimerService timerService)
        {
            _logger = logger;
            _twitterEngine = twitterEngine;
            _timerService = timerService;
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

        [HttpPut]
        [Authorize(Policy = Policies.ProlobbyOwner)]
        public async Task<ActionResult<string>> UpdateBalancesAllCampaigns()
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {
                _logger.LogInformation("Asking twitterEngine to handle request");
                var response = await _twitterEngine.ChangeUserBalancesForAllCampaigns();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpPut("{seconds}")]
        [Authorize(Policy = Policies.ProlobbyOwner)]
        public ActionResult<string> StartTwitterChecking(int seconds)
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            if (seconds <= 10) return BadRequest("Please input positive number >10");

            try
            {
                _timerService.StartPeriodicalCheck(seconds);

                return Ok("Started periodical checking twitter");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = Policies.ProlobbyOwner)]
        public ActionResult<string> StopTwitterChecking()
        {
            _logger.LogInformation("{Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

            try
            {
                _timerService.StopPeriodicalCheck();

                return Ok("Stopped periodical check of twitter");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
