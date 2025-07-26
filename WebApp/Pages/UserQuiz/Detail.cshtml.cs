using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebApp.Pages.UserQuiz
{
    public class DetailModel : PageModel
    {
        public int QuizId { get; set; }
        public string? AccessToken { get; set; }

        public void OnGet(int id)
        {
            QuizId = id;
            AccessToken = HttpContext.Session.GetString("accessToken");
        }
    }

}
