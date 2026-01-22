using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Application.Mapping;
using BarberRezende.Application.Services;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;
using BarberRezende.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================= DB CONTEXT =================
// Registra o DbContext no container de DI.
// Ele será usado pelos Repositories (Infrastructure) para acessar o SQL Server.
builder.Services.AddDbContext<BarberRezendeDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// ================= REPOSITORIES =================
// Domain define os contratos (interfaces) e a Infrastructure implementa.
// Aqui dizemos ao .NET: "quando alguém pedir IClienteRepository, entregue ClienteRepository".
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IBarbeiroRepository, BarbeiroRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

// ================= SERVICES =================
// Application Services: orquestram regras e chamam repositórios.
// Controller não fala com repositório direto: fala com Service.
builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IBarbeirosService, BarbeirosService>();
builder.Services.AddScoped<IFuncionariosService, FuncionariosService>();
builder.Services.AddScoped<IServicosService, ServicosService>();
builder.Services.AddScoped<IAgendamentosService, AgendamentosService>();

// ================= AUTOMAPPER =================
// Mapeia Entity <-> DTO
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ApplicationMappingProfile>();
});

// ================= CONTROLLERS =================
// Aqui ativamos Controllers + padronização de validações (ModelState).
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Quando o DTO falhar na validação (DataAnnotations),
        // o ASP.NET retorna 400 automaticamente.
        // Aqui só padronizamos o formato do retorno.
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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ================= PIPELINE (MIDDLEWARES) =================
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// OBS: se você usa HTTPS local e não configurou porta no launchSettings,
// pode aparecer warning do redirect (não é erro).
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
