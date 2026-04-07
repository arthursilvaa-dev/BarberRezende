using BarberRezende.Web.Models;
using BarberRezende.Web.Services;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// HttpContext
builder.Services.AddHttpContextAccessor();

// Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Token
builder.Services.AddScoped<TokenSessionService>();

// Config da API
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("Api")
);

// ✅ HTTP CLIENT CORRETO COM BYPASS DE SSL
builder.Services.AddHttpClient("Api", (sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;

    if (settings is null || string.IsNullOrWhiteSpace(settings.BaseUrl))
        throw new InvalidOperationException("Api:BaseUrl não configurado.");

    client.BaseAddress = new Uri(settings.BaseUrl);

    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    // Esta linha mágica ignora os erros de SSL do servidor gratuito do MonsterASP
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    };
});

// ApiClient usando o HttpClient correto
builder.Services.AddScoped<ApiClient>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = factory.CreateClient("Api");
    var tokenService = sp.GetRequiredService<TokenSessionService>();

    return new ApiClient(httpClient, tokenService);
});

// Filtro
builder.Services.AddScoped<AuthFilter>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}
else {
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
