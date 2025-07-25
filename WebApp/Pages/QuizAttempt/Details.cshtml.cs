using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net;
using WebApp.Helpers;
using API.Dtos.QuizAttempt;
using Newtonsoft.Json;

namespace WebApp.Pages.QuizAttempt
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public DetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = APIUrlHelper.GetBaseUrl();
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public QuizAttemptDto QuizAttempt { get; set; } = new QuizAttemptDto();

        public async Task<IActionResult> OnGetAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/api/quizAttempt/{Id}");
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
                QuizAttempt = JsonConvert.DeserializeObject<QuizAttemptDto>(jsonString) ?? new QuizAttemptDto();

                return Page();
            }
        }
    }
}
