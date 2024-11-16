using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using WnT.Web.Models.DTO;

namespace WnT.Web.Controllers
{
    public class WalksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _walkEndpoint;
        private readonly string _regionsEndpoint;

        public WalksController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _walkEndpoint = configuration.GetValue<string>("ApiSettings:WalksEndpoint");
            _regionsEndpoint = configuration.GetValue<string>("ApiSettings:RegionsEndpoint");
        }

        public async Task<IActionResult> Index(int pageParam = 1)
        {
            List<WalkDTO> walks = new List<WalkDTO>();

            try
            {
                var client = _httpClientFactory.CreateClient();

                // Construct the API URL with query parameters
                var url = $"{_walkEndpoint}/getAll?pageParam={pageParam}&pageSize=10";

                using var responseClient = await client.GetAsync(url);

                responseClient.EnsureSuccessStatusCode();

                var result = await responseClient.Content.ReadFromJsonAsync<IEnumerable<WalkDTO>>();

                if (result != null)
                {
                    walks.AddRange(result);
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP errors
                Console.Error.WriteLine($"Request error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other errors
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
            ViewBag.CurrentPage = pageParam;

            return View(walks);
        }

        [HttpGet]
        public async Task<IActionResult> add()
        {
            var viewModel = new AddWalkViewModel();

            try
            {
                var client = _httpClientFactory.CreateClient();

                // Fetch regions
                var regionsResponse = await client.GetAsync($"{_regionsEndpoint}/getAll");
                regionsResponse.EnsureSuccessStatusCode();
                viewModel.AvailableRegions = await regionsResponse.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>();


            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP errors
                Console.Error.WriteLine($"Request error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other errors
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }



            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> add(AddWalkViewModel addWalkViewModel)
        {
            if (!ModelState.IsValid)
            {
                addWalkViewModel.AvailableRegions = await FetchRegionsAsync();
                return View(addWalkViewModel);
            }
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Serialize only the `Walk` property as this is the data needed by the API
                var content = new StringContent(JsonSerializer.Serialize(addWalkViewModel.Walk), Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_walkEndpoint}/saveWalk", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Success"); // Redirect on successful submission
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.Error.WriteLine($"Failed to save walk data. Status Code: {response.StatusCode}, Error: {errorContent}");
                    ModelState.AddModelError(string.Empty, "Failed to save walk data.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
            }

            addWalkViewModel.AvailableRegions = await FetchRegionsAsync();
            return View(addWalkViewModel);
        }
        // Helper methods to fetch regions and difficulties
        private async Task<IEnumerable<RegionDTO>> FetchRegionsAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_regionsEndpoint}/getAll");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>();
        }
    }
}
