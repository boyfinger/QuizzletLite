using API.DAO;
using API.Dtos;
using API.Dtos.Question;
using API.Dtos.Quiz;
using API.Dtos.User;
using API.Dtos.QuizAttempt;
using API.Models;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<QuizletLiteContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuizletLite_DB"));
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "QuizzletLite API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var jwtSettings = builder.Configuration.GetSection("JWT");
var signingKey = jwtSettings["SigningKey"];
var googleConfig = builder.Configuration.GetSection("GoogleAuthentication");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SigningKey"]!)
            ),
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = googleConfig["client_id"];
        options.ClientSecret = googleConfig["client_secret"];
        options.SaveTokens = true;
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Events = new OAuthEvents
        {
            OnCreatingTicket = async context =>
            {
                var accessToken = context.AccessToken;

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
                var content = await response.Content.ReadAsStringAsync();

                var userInfo = System.Text.Json.JsonDocument.Parse(content).RootElement;
                var picture = userInfo.GetProperty("picture").GetString();
                var name = userInfo.GetProperty("name").GetString();
                var email = userInfo.GetProperty("email").GetString();

                context.Identity.AddClaim(new Claim("picture", picture));
                context.Identity.AddClaim(new Claim(ClaimTypes.Name, name));
                context.Identity.AddClaim(new Claim(ClaimTypes.Email, email));
            },

            OnRemoteFailure = context =>
            {
                context.Response.Redirect("https://localhost:7113/Auth/Login");
                context.HandleResponse();
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IQuizAttemptService, QuizAttemptService>();
builder.Services.AddScoped<IQuizAttemptRepository, QuizAttemptRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<Admin_IQuizRepository, Admin_QuizRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<Admin_IQuizService, Admin_QuizService>();
builder.Services.AddScoped<Admin_IQuestionRepository, Admin_QuestionRepository>();
builder.Services.AddScoped<Admin_IQuestionService, Admin_QuestionService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionService, QuestionService>();


builder.Services.AddControllers().AddOData(options => options
    .Filter()
    .OrderBy()
    .Select()
    .Count()
    .Expand()
    .SetMaxTop(100)
    .AddRouteComponents("odata", getEdmModel())
);
IEdmModel getEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Question>("Questions");
    builder.EntitySet<Quiz>("Quizzes");
    builder.EntitySet<QuizAttempt>("QuizAttempts");
    builder.EntitySet<QuizzesDto>("QuizzesDto");
    builder.EntitySet<QuestionDto>("QuestionDtos");
    builder.EntitySet<UserDto>("Users");
    builder.EntitySet<Admin_QuizDto>("Admin_Quizzes");
    builder.EntitySet<QuizAttemptDto>("QuizAttemptDtos");
    return builder.GetEdmModel();
}

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(
//    opt =>
//    {
//        opt.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,

//            ValidIssuer = builder.Configuration["JWT:Issuer"],
//            ValidAudience = builder.Configuration["JWT:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))

//        };
//    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
