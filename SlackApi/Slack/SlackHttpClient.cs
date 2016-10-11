using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SlackApi.Slack
{
    public class SlackHttpClient
    {
        public string SlackUrl { get; set; }

        public async Task<HttpStatusCode> Post(string text)
        {
            var payload = JsonConvert.SerializeObject(new SlackPostModel { text = text });
            var data = new FormUrlEncodedContent(new Dictionary<string, string>() { { "payload", payload } });

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(SlackUrl, data);
                return response.StatusCode;
            }
        }
    }
}
