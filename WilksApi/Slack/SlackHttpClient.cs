using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WilksApi.Slack
{
    public class SlackHttpClient
    {
        private string SlackUrl { get; set; } = "https://hooks.slack.com/services/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";

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
