using CosmosDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDB.Controllers
{
    [ApiController]
    [Route("api/locations")]
    public class CosmosController : Controller
    {
        CosmosClient cosmosClient;
        Container cosmosContainer;

        public CosmosController(CosmosClient client)
        {
            cosmosClient = client;
            cosmosContainer = client.GetContainer("step-cosmos-db", "locations");
        }

        [HttpPost]
        public async Task<IActionResult> Add(Location location)
        {
            if (ModelState.IsValid)
            {
                location.Id = Guid.NewGuid().ToString();

                var response = await cosmosContainer.CreateItemAsync(
                    location, new PartitionKey(location.Country));

                return StatusCode(((int)response.StatusCode));
            }
            return Problem();
        }

        [HttpGet]
        public async Task<List<Location>> GetAll()
        {
            var results = new List<Location>();

            //Sql way
            var iterator = cosmosContainer.GetItemQueryIterator<Location>();

            //Linq Way
            iterator = cosmosContainer.GetItemLinqQueryable<Location>().Where(x => x.Country == "Bob").ToFeedIterator();

            while (iterator.HasMoreResults)
            {
                results.AddRange((await iterator.ReadNextAsync()).ToList());
            }

            return results;
        }

    }
}
