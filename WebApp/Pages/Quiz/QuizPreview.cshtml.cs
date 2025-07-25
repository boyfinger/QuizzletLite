using API.Dtos.Quiz.QuizDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using WebApp.Helpers;

namespace WebApp.Pages.Quiz
{
    public class QuizPreviewModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        [BindProperty]
        public List<Flashcard> Flashcards { get; set; } = [];
        [BindProperty]
        public QuizDetailsDto QuizDetails { get; set; }

        public QuizPreviewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = APIUrlHelper.GetBaseUrl();
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
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
                Flashcards = QuizDetails.Questions.Select(q =>
                {
                    string question = q.Content + "<br /><br />" + string.Join("<br />", q.Options.Select(o => o.Content));
                    string answer = string.Join("<br />", q.Options.Where(o => o.IsCorrect).Select(o => o.Content));

                    var flashcard = new Flashcard
                    {
                        Question = question.Trim(),
                        Answer = answer.Trim()
                    };
                    return flashcard;
                }).ToList();
                return Page();
            }
        }
    }
}
