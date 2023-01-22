using System.Timers;
using Timer = System.Timers.Timer;

namespace MainService.EngineServices
{
    public class TimerService
    {
        private readonly Timer _timer;
        private readonly TwitterEngineService _twitterEngine;
        private readonly ILogger<TimerService> _logger;

        public TimerService(TwitterEngineService twitterEngine, ILogger<TimerService> logger)
        {
            _timer = new Timer(100000);
            _twitterEngine = twitterEngine;
            _logger = logger;
        }

        public void StartPeriodicalCheck(int seconds)
        {
            _logger.LogInformation("Starting periodical twitter check each {minutes}", seconds/60);

            double interval = TimeSpan.FromSeconds(seconds).TotalMilliseconds;
            _timer.Interval = interval;
            _timer.Enabled = true;
            _timer.Elapsed += OnTimerEvent;
        }

        public void StopPeriodicalCheck()
        {
            _timer.Enabled = false;
            _logger.LogInformation("Stopped periodical twitter check");
        }

        public async void OnTimerEvent(object? source, ElapsedEventArgs e)
        {
            try
            {
                _logger.LogInformation("Regular check of twitter invoked at {0:HH:mm:ss.fff}", e.SignalTime);

                var response = await _twitterEngine.ChangeUserBalancesForAllCampaigns();

                _logger.LogInformation("twitter Engine returns: {message}", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                StopPeriodicalCheck();

            }
        }
    }
}
