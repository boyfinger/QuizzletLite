using API.Dtos.Quiz;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Helpers;

namespace WebApp.Pages.Quiz
{
    public class QuizListModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        [BindProperty]
        public List<QuizDto> QuizList { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; } = string.Empty;

        [BindProperty]
        public int TotalPages { get; set; }

        public QuizListModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = APIUrlHelper.GetBaseUrl();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            int pageSize = 6;

            try
            {
                List<QuizDto> fullList;

                if (string.IsNullOrEmpty(SearchString))
                {
                    fullList = await _httpClient.GetFromJsonAsync<List<QuizDto>>($"{_baseUrl}/api/quiz/quizzes");
                    QuizList = await _httpClient.GetFromJsonAsync<List<QuizDto>>($"{_baseUrl}/api/quiz/quizzes?page={CurrentPage}&pageSize={pageSize}");
                }
                else
                {
                    fullList = await _httpClient.GetFromJsonAsync<List<QuizDto>>($"{_baseUrl}/api/quiz/quizzes?name={SearchString}");
                    QuizList = await _httpClient.GetFromJsonAsync<List<QuizDto>>($"{_baseUrl}/api/quiz/quizzes?name={SearchString}&page={CurrentPage}&pageSize={pageSize}");
                }

                TotalPages = (int)Math.Ceiling((double)fullList.Count / pageSize);
                if (CurrentPage < 1)
                {
                    CurrentPage = 1;
                }
                else if (CurrentPage > TotalPages)
                {
                    CurrentPage = TotalPages;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching quizzes: {ex.Message}");
                Console.WriteLine(ex.ToString());
            }

            return Page();
        }
    }
}
