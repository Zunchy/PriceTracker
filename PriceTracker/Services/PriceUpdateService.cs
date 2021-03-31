using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PriceTracker.Services
{
    public class PriceUpdateService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public PriceUpdateService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        //Callback for timer, initiates checking and updating all prices for tracked items
        private async Task UpdateAllTrackedItemPrices(object state)
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var trackingService = scope.ServiceProvider.GetRequiredService<TrackingService>();

                await trackingService.UpdateTrackedItemPrices();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //Time interval for the task to run
            TimeSpan interval = TimeSpan.FromHours(24);

            //Calculate the first interval to run price update
            var nextRunTime = DateTime.Today.AddDays(1);
            var currentTime = DateTime.Now;
            var firstInterval = nextRunTime - currentTime;

            //Delay initialization of timer so that the task only run at midnight
            Action action = async () =>
            {
                var t1 = Task.Delay(firstInterval);
                t1.Wait();
                await UpdateAllTrackedItemPrices(null);

                // timer repeates call to UpdateAllTrackedItemPrices every 24 hours.
                _timer = new Timer(
                    async o => await UpdateAllTrackedItemPrices(o),
                    null,
                    TimeSpan.Zero,
                    interval
                );
            };

            Task.Run(action);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
