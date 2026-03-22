using BarberRezende.API.Middlewares;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Application.Mapping;
using BarberRezende.Application.Services;
using BarberRezende.Application.Settings;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;
using BarberRezende.Infrastructure.Repositories;
using BarberRezende.Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);



// ================= DB CONTEXT =================
// Registra o DbContext no container de DI.
// Ele será usado pelos Repositories (Infrastructure) para acessar o SQL Server.
builder.Services.AddDbContext<BarberRezendeDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});



// ================= REPOSITORIES =================
// Domain define os contratos (interfaces)
// Infrastructure implementa.
// Aqui dizemos ao .NET qual implementação entregar.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IBarbeiroRepository, BarbeiroRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();



// ================= APPLICATION SERVICES =================
// Services orquestram regras de negócio.
// Controller NÃO fala com Repository direto.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IBarbeirosService, BarbeirosService>();
builder.Services.AddScoped<IFuncionariosService, FuncionariosService>();
builder.Services.AddScoped<IServicosService, ServicosService>();
builder.Services.AddScoped<IAgendamentosService, AgendamentosService>();



// ================= MIDDLEWARE CUSTOM =================
builder.Services.AddTransient<ApiExceptionMiddleware>();



// ================= AUTOMAPPER =================
// Mapeia Entity <-> DTO
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ApplicationMappingProfile>();
});



// ================= CORS =================
// Permite que o FrontEnd (porta diferente) consuma a API
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsDev", policy =>
    {
        policy
            .AllowAnyOrigin()   // DEV apenas
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});



// ================= CONTROLLERS =================
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState) {
                Title = "Falha de validação",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Verifique os campos enviados e tente novamente.",
                Instance = context.HttpContext.Request.Path
            };

            return new BadRequestObjectResult(problemDetails);
        };
    });



// ================= SWAGGER =================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token}"
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("Bearer"),
                new List<string>()
            }
        });
});




/// ================= JWT CONFIGURATION =================
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

// ================= AUTENTICAÇÃO JWT =================
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

var key = Encoding.UTF8.GetBytes(jwtSettings!.SecretKey);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // DEV
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

// Habilita autorização
builder.Services.AddAuthorization();




// ================= BUILD APP =================
var app = builder.Build();

// ================= SEED ADMIN =================
using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<BarberRezendeDbContext>();
    SeedService.SeedAdmin(context);
}



// ================= PIPELINE (ORDEM IMPORTA) =================
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/", () => Results.Redirect("/swagger"));
}

// CORS
app.UseCors("CorsDev");

// HTTPS
app.UseHttpsRedirection();

// Middleware global de exceções
app.UseMiddleware<ApiExceptionMiddleware>();

// 🔐 AUTENTICAÇÃO (vem antes da autorização)
app.UseAuthentication();

// 🛡 AUTORIZAÇÃO
app.UseAuthorization();

// Controllers
app.MapControllers();

var senha = "Admin123!";
var hash = BCrypt.Net.BCrypt.HashPassword(senha);

Console.WriteLine("HASH GERADA:");
Console.WriteLine(hash);

app.Run();