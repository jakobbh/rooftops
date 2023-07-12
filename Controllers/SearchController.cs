using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication8.Models;
using static System.Net.WebRequestMethods;

namespace WebApplication8.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //Get JSON response from Google Places API
        [HttpPost]
        public IActionResult Search(SearchViewModel model)
        {
            var client = new HttpClient();
            var api_key = "key";
            var query = model.Rooftop + "+" + model.Country;
            var endpoint = new Uri($"https://maps.googleapis.com/maps/api/place/textsearch/json?query={query}&key={api_key}");

            var result = client.GetAsync(endpoint).Result.Content.ReadAsStringAsync().Result;
            var deserializedData = JsonSerializer.Deserialize<JsonElement>(result);

            var search_result = deserializedData.GetProperty("results");
            var address = search_result[0].GetProperty("formatted_address").GetString();
            var name = search_result[0].GetProperty("name").GetString();

            var rooftop = new Rooftop(){
                Address = address,
                Name = name
            };
            var viewModel = new RooftopViewModel()
            {
                Rooftop = rooftop,
                anotherProp = model.Country
            };

            return View(viewModel);
        }
    }
}
