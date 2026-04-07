using System;

namespace BarberRezende.Domain.Entities
{
    /// <summary>
    /// Representa o usuário administrador do sistema.
    /// 
    /// Decisão arquitetural:
    /// - Pertence ao Domain pois é uma entidade de negócio.
    /// - Não depende de EF.
    /// - Não possui atributos de infraestrutura.
    /// - Usa private setters para proteger invariantes. Invariantes são regras que devem sempre ser verdadeiras, como "um usuário deve ter um email". Ao usar private setters, garantimos que essas regras só possam ser violadas dentro da própria classe, onde podemos controlar melhor as mudanças.
    /// </summary>
    public class AdminUser
    {
        /// <summary>
        /// Identificador único do administrador.
        /// </summary>
        public Guid Id { get; private set; } // Usamos Guid para garantir unicidade global, mesmo que tenhamos múltiplas instâncias do sistema.

        /// <summary>
        /// Email utilizado para login.
        /// Deve ser único no sistema.
        /// </summary>
        public string Email { get; private set; } //private setter para garantir que o email só possa ser definido na criação ou por um método específico, evitando mudanças acidentais.

        /// <summary>
        /// Hash da senha armazenado com BCrypt.
        /// Nunca armazenamos senha em texto puro.
        /// </summary>
        public string PasswordHash { get;  set; } //passwordhash é o hash da senha, e não a senha em si.
                                                         //Usamos private setter para garantir que a senha só
                                                         //possa ser alterada por um método específico, como
                                                         //UpdatePassword, onde podemos aplicar regras
                                                         //adicionais se necessário.

        /// <summary>
        /// Data de criação do usuário.
        /// Útil para auditoria futura.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Construtor privado exigido pelo EF Core.
        /// </summary>
        private AdminUser() { } // O construtor privado é necessário para o EF Core criar instâncias da entidade
                                // quando estiver lendo do banco de dados. Ele não deve ser usado diretamente no
                                // código, por isso é privado.

        /// <summary>
        /// Construtor público para criação de um novo Admin.
        /// 
        /// Decisão:
        /// - Forçamos criação com Email e PasswordHash.
        /// - Não permitimos criação sem senha.
        /// </summary>
        public AdminUser(string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Permite alterar o hash da senha.
        /// Não expomos setter público.
        /// </summary>
        public void UpdatePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }
    }
}