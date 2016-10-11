using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlackApi.Managers;
using SlackApi.Slack;

namespace SlackApi.Controllers
{
    public class WilksController : Controller
    {
        private WilksManager WilksManager;
        private SlackHttpClient SlackHttpClient;

        public WilksController(WilksManager wilksManager, SlackHttpClient slackHttpClient)
        {
            WilksManager = wilksManager;
            SlackHttpClient = slackHttpClient;
            SlackHttpClient.SlackUrl = "FUCKING SECRET";
        }

        /// <summary>
        /// Gets the wilks based on what the weakpot types in slack and posts to slack
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="team_id">The team_id.</param>
        /// <param name="team_domain">The team_domain.</param>
        /// <param name="channel_id">The channel_id.</param>
        /// <param name="channel_name">The channel_name.</param>
        /// <param name="user_id">The user_id.</param>
        /// <param name="user_name">The user_name.</param>
        /// <param name="command">The command.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task GetWilks(string token, string team_id, string team_domain, string channel_id, string channel_name, string user_id, string user_name, string command, string text)
        {
            var wilksMessage = WilksManager.GetWilksMessage(text);
            await SlackHttpClient.Post(wilksMessage);
        }
    }
}
