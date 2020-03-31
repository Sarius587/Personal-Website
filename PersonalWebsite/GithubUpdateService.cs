using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalWebsite
{
    public class GithubUpdateService : IHostedService, IDisposable
    {

        private Timer _timer;


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateGithubReposAsync, null, TimeSpan.Zero, TimeSpan.FromHours(1.0));
            GithubClient.Initialize();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private async void UpdateGithubReposAsync(object state)
        {
            string json = await GithubClient.GetRepositoriesAsync();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
