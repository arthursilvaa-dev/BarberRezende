using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Application.Mapping;
using BarberRezende.Application.Services;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;
using BarberRezende.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================= DB CONTEXT =================
builder.Services.AddDbContext<BarberRezendeDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// ================= REPOSITORIES =================
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IBarbeiroRepository, BarbeiroRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

// ================= SERVICES =================
builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IBarbeirosService, BarbeirosService>();
builder.Services.AddScoped<IFuncionariosService, FuncionariosService>();
builder.Services.AddScoped<IServicosService, ServicosService>();
builder.Services.AddScoped<IAgendamentosService, AgendamentosService>();

// ================= AUTOMAPPER =================
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ApplicationMappingProfile>();
});

// ================= CONTROLLERS & SWAGGER =================
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // <-- tem que ficar ANTES do Build

var app = builder.Build();

// ================= MIDDLEWARE PIPELINE =================
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment()) {
    app.UseHttpsRedirection();
}



app.UseAuthorization();
app.MapControllers();

app.Run();
