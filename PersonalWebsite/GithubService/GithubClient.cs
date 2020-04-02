using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace PersonalWebsite.GithubService
{
    public class GithubClient
    {

        private readonly HttpClient _client = new HttpClient();

        public GithubClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.github.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3.html+json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("product", "1"));

            _client = client;
        }

        public async Task<string> GetRepositoriesAsync()
        {
            try
            {
                HttpResponseMessage msg = await _client.GetAsync("/users/Sarius587/repos");
                if (msg.IsSuccessStatusCode)
                {
                    return await msg.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public async Task<string> GetReadmeAsync(string path)
        {
            try
            {
                HttpResponseMessage msg = await _client.GetAsync(path);
                if (msg.IsSuccessStatusCode)
                {
                    return await msg.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
