using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace PersonalWebsite
{
    public class GithubClient
    {

        static readonly HttpClient client = new HttpClient();


        public static void Initialize()
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri("https://api.github.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("product", "1"));
            }
        }

        public static async Task<string> GetRepositoriesAsync()
        {
            try
            {
                HttpResponseMessage msg = await client.GetAsync("/users/Sarius587/repos");
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
