using API.Dtos.Quiz.QuizDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using WebApp.Helpers;
using API.Dtos.User;

namespace WebApp.Pages.Quiz
{
    public class QuizTakingModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public QuizTakingModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = APIUrlHelper.GetBaseUrl();
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public QuizDetailsDto QuizDetails { get; set; }

        [BindProperty]
        public UserDto? UserInfo { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/api/quiz/{Id}/questions");
            var accessToken = HttpContext.Session.GetString("accessToken");
            if (accessToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("/auth/login");
            }
            else if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching quiz details: {response.ReasonPhrase}");
                return StatusCode((int)response.StatusCode);
            }
            else
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                QuizDetails = JsonConvert.DeserializeObject<QuizDetailsDto>(jsonString) ?? new QuizDetailsDto();

                request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/api/authentication/me");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                response = await _httpClient.SendAsync(request);
                UserInfo = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());

                return Page();
            }
        }
    }
}
