using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Threading.Tasks;

namespace StayinAlive.Alive
{
    [Route("api/[controller]")]
    [ApiController]
    public class AliveController : ControllerBase
    {
        private readonly IRestClient restClient;

        public AliveController(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AliveModel model)
        {
            var restRequest = new RestRequest(model.ReturnUrl, Method.GET);

            var response = await restClient
                .ExecuteTaskAsync(restRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return Ok(model.ReturnUrl);
            else
                return BadRequest(response.ErrorMessage);
        }
    }
}
