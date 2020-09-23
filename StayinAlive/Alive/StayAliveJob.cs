using System.Linq;
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
            var urls = configuration
                .GetSection("Services:Urls")
                .AsEnumerable()
                .Where(x=> x.Value != null)
                .Select(x=> x.Value)
                .ToList();
            
            foreach (var url in urls)
            {
                var restRequest = new RestRequest(url, Method.GET);

                var response = restClient
                    .Execute(restRequest);
            }
        }
    }
}
