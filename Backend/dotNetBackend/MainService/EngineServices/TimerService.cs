using System.Timers;
using Timer = System.Timers.Timer;

namespace MainService.EngineServices
{
    public class TimerService
    {
        public readonly Timer timer;
        private readonly TwitterEngineService _twitterEngine;
        private readonly ILogger<TimerService> _logger;

        public TimerService(TwitterEngineService twitterEngine, ILogger<TimerService> logger)
        {
            timer = new Timer(100000);
            _twitterEngine = twitterEngine;
            _logger = logger;
        }

        public void StartPeriodicalCheck(int seconds)
        {
            _logger.LogInformation("Starting periodical twitter check each {minutes} minute(s)", (double)seconds/60);

            double interval = TimeSpan.FromSeconds(seconds).TotalMilliseconds;
            timer.Interval = interval;
            timer.Enabled = true;
            timer.Elapsed += OnTimerEvent;
        }

        public void StopPeriodicalCheck()
        {
            timer.Enabled = false;
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
