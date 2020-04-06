using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalWebsite.GithubService
{
    public class GithubUpdateService : IHostedService, IDisposable
    {

        public class RepositoryData
        {
            public int Id { get; set; }
            public bool Fork { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Html_url { get; set; }
        }

        private Timer _timer;
        private readonly GithubClient _client;
        private readonly IServiceScopeFactory _scopeFactory;

        public GithubUpdateService(GithubClient client, IServiceScopeFactory scopeFactory)
        {
            _client = client;
            _scopeFactory = scopeFactory;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateGithubReposAsync, null, TimeSpan.Zero, TimeSpan.FromHours(24.0));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private async void UpdateGithubReposAsync(object state)
        {
            string json = await _client.GetRepositoriesAsync();

            if (json == null)
                return;

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GithubRepositoryContext>();

                IList<GithubRepository> repos = new List<GithubRepository>();
                JArray obj = JArray.Parse(json);
                foreach (JToken repo in obj)
                {
                    RepositoryData data = repo.ToObject<RepositoryData>();
                    if (!data.Fork)
                    {
                        string readme_html = await _client.GetReadmeAsync("/repos/Sarius587/" + data.Name + "/readme");
                        AdditionalRepositoryData additionalData = await context.AdditionalData.SingleOrDefaultAsync(a => a.RepositoryId == data.Id);
                        int id = -1;
                        if (additionalData == null)
                        {
                            AdditionalRepositoryData newAdditionalData = new AdditionalRepositoryData { RepositoryId = data.Id, CustomExperience = null, LastEdit = DateTime.UtcNow };
                            await context.AdditionalData.AddAsync(newAdditionalData);
                            await context.SaveChangesAsync();
                            id = newAdditionalData.Id;
                        }
                        else
                        {
                            id = additionalData.Id;
                        }

                        repos.Add(new GithubRepository
                        {
                            RepositoryId = data.Id,
                            Name = data.Name,
                            Description = data.Description,
                            Url = data.Html_url,
                            Readme = readme_html,
                            AdditionalRepositoryDataId = id
                        });
                    }
                }

                
            
                
                IList<GithubRepository> db_repos = await context.Repositories.ToListAsync();

                var add_list = repos.Except(db_repos);
                var remove_list = db_repos.Except(repos);

                context.Repositories.AddRange(add_list);
                context.Repositories.RemoveRange(remove_list);
                await context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
