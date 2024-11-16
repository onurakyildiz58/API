using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using WnT.Web.Models.DTO;

namespace WnT.Web.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _regionsEndpoint;

        public RegionsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _regionsEndpoint = configuration.GetValue<string>("ApiSettings:RegionsEndpoint");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> regions = new List<RegionDTO>();

            try
            {
                var client = _httpClientFactory.CreateClient();

                // Send GET request to fetch regions
                using var responseClient = await client.GetAsync($"{_regionsEndpoint}/getAll");

                // Ensure success status code and handle the response content
                responseClient.EnsureSuccessStatusCode();

                var result = await responseClient.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>();

                if (result != null)
                {
                    regions.AddRange(result);
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Log the HTTP-specific error (e.g., network or API issues)
                Console.Error.WriteLine($"Request error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Log any other errors
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }

            return View(regions);
        }

        [HttpGet]
        public IActionResult add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> add(AddRegionDTO addRegionDTO)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, return the same view to show validation errors.
                return View(addRegionDTO);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(addRegionDTO), Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_regionsEndpoint}/saveRegion", content);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally, you could read the response body if you need to use the returned RegionDTO
                    var responseBody = await response.Content.ReadFromJsonAsync<RegionDTO>();

                    // Redirect to Index if the region was added successfully
                    return RedirectToAction("Index", "Regions");
                }
                else
                {
                    // Log the error response for debugging purposes
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.Error.WriteLine($"Error adding region: {response.StatusCode}, {errorContent}");

                    // Optionally, you can add a model error to inform the user about the issue
                    ModelState.AddModelError("", "Unable to add region. Please try again.");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log HTTP request exceptions
                Console.Error.WriteLine($"HTTP request error: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while connecting to the server. Please try again.");
            }
            catch (Exception ex)
            {
                // Log any other exceptions
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }

            // If we reach this point, there was an error; return the view with the existing data
            return View(addRegionDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                // Return a bad request if the ID is invalid
                return BadRequest("Invalid ID.");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();

                // Send GET request to fetch the region by ID
                var response = await client.GetFromJsonAsync<RegionDTO>($"{_regionsEndpoint}/getRegionById/{id}");

                if (response != null)
                {
                    return View(response);
                }
                else
                {
                    // Handle the case where the region was not found
                    // Optionally log this event for debugging
                    Console.Error.WriteLine($"Region with ID {id} not found.");
                    return NotFound($"Region with ID {id} not found.");
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Log HTTP request errors
                Console.Error.WriteLine($"HTTP request error while fetching region: {httpEx.Message}");
                // Optionally, you can add a model error to inform the user
                ModelState.AddModelError("", "An error occurred while fetching the region. Please try again.");
            }
            catch (Exception ex)
            {
                // Log any other exceptions
                Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }

            // If we reach this point, there was an error; return the view with a null model
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDTO regionDTO)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, return the same view with validation errors
                return View(regionDTO);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();

                // Send PUT request to update the region
                var content = new StringContent(JsonSerializer.Serialize(regionDTO), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{_regionsEndpoint}/updateRegion/{regionDTO.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally, you can read the response body if needed
                    var updatedRegion = await response.Content.ReadFromJsonAsync<RegionDTO>();

                    // Redirect to the Index page if the update is successful
                    return RedirectToAction("Index", "Regions");
                }
                else
                {
                    // Log the error and provide feedback to the user
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.Error.WriteLine($"Error updating region: {response.StatusCode}, {errorContent}");
                    ModelState.AddModelError("", "Unable to update the region. Please try again.");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception and inform the user about the issue
                Console.Error.WriteLine($"HTTP request error while updating region: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while communicating with the server. Please try again.");
            }
            catch (Exception ex)
            {
                // Catch any other exceptions and log them
                Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }

            // Return the view with the existing data in case of failure
            return View(regionDTO);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();

                // Send DELETE request to delete the region by ID   
                var response = await client.DeleteAsync($"{_regionsEndpoint}/deleteRegion/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to the Index page after successful deletion
                    return RedirectToAction("Index", "Regions");
                }
                else
                {
                    // Handle the case where the deletion failed
                    Console.Error.WriteLine($"Error deleting region: {response.StatusCode}");
                    ModelState.AddModelError("", "Unable to delete the region. Please try again.");
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.Error.WriteLine($"HTTP request error while deleting region: {httpEx.Message}");
                ModelState.AddModelError("", "An error occurred while communicating with the server. Please try again.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }

            // Return to the same view if there was an error
            return View();
        }

    }
}
