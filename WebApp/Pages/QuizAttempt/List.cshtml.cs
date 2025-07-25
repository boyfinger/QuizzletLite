using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using WebApp.Helpers;
using API.Dtos.Quiz.QuizDetails;

namespace WebApp.Pages.QuizAttempt
{
    public class ListModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ListModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = APIUrlHelper.GetBaseUrl();
        }

        [BindProperty]
        public List<QuizResultDto> ResultList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/api/quizAttempt/user");
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
                ResultList = JsonConvert.DeserializeObject<List<QuizResultDto>>(jsonString) ?? [];

                return Page();
            }
        }
    }
}
