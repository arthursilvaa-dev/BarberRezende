using BarberRezende.Domain.Entities;
using BarberRezende.Infrastructure.Data;
using BCrypt.Net;

namespace BarberRezende.Infrastructure.Seed;
public static class SeedService
{
    // Método para popular o banco de dados com um usuário administrador padrão.
    public static void SeedAdmin(BarberRezendeDbContext context)
    {
        // Verifica se já existe o admin padrão
        if (context.AdminUsers.Any(a => a.Email == "admin@barberrezende.com"))
            return;

        // Gera o hash da senha
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");

        // Cria o administrador
        var admin = new AdminUser(
            "admin@barberrezende.com",
            passwordHash
        );

        // Salva no banco
        context.AdminUsers.Add(admin);
        context.SaveChanges();
    }
}