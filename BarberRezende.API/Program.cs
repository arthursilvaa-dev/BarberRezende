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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; 
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================= 1. BANCO DE DADOS =================
builder.Services.AddDbContext<BarberRezendeDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// ================= 2. REPOSITÓRIOS =================
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IBarbeiroRepository, BarbeiroRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();

// ================= 3. SERVIÇOS DA APLICAÇÃO =================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IBarbeirosService, BarbeirosService>();
builder.Services.AddScoped<IFuncionariosService, FuncionariosService>();
builder.Services.AddScoped<IServicosService, ServicosService>();
builder.Services.AddScoped<IAgendamentosService, AgendamentosService>();

// ================= 4. CONFIGURAÇÕES EXTRAS =================
builder.Services.AddTransient<ApiExceptionMiddleware>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ApplicationMappingProfile>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsDev", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var problemDetails = new ValidationProblemDetails(context.ModelState)
        {
            Title = "Falha de validação",
            Status = StatusCodes.Status400BadRequest,
            Detail = "Verifique os campos enviados.",
            Instance = context.HttpContext.Request.Path
        };
        return new BadRequestObjectResult(problemDetails);
    };
});

// ================= 5. SWAGGER =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ================= 6. JWT & AUTENTICAÇÃO =================
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings!.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
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

builder.Services.AddAuthorization();

var app = builder.Build();

// ================= 7. PIPELINE (MIDDLEWARES) =================
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseCors("CorsDev");
app.UseHttpsRedirection();
app.UseMiddleware<ApiExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ================= 8. AUTO-MIGRAÇÃO E SEED =================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<BarberRezendeDbContext>();
        
        // Aplica as Migrations (Cria as tabelas no MonsterASP)
        context.Database.Migrate();
        
        // Cria o usuário Admin inicial
        SeedService.SeedAdmin(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao iniciar o banco de dados.");
    }
}

app.Run();
