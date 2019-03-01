using Microsoft.Extensions.Configuration;
using RestSharp;

namespace StayinAlive.Alive
{
    public class StayAliveJob
    {
        private readonly IRestClient restClient;
        private readonly IConfiguration configuration;

        public StayAliveJob(
            IRestClient restClient,
            IConfiguration configuration)
        {
            this.restClient = restClient;
            this.configuration = configuration;
        }

        public void Run()
        {
            var url = configuration["Services:Url"];
            var restRequest = new RestRequest(url, Method.GET);

            var response = restClient
                .Execute(restRequest);
        }
    }
}
