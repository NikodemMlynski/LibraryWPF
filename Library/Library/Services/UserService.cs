using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Library.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService()
        {
            _context = new AppDbContext();
        }

        public async Task<bool> RegisterUserAsync(string name, string email, string password)
        {
            // Sprawdzenie czy użytkownik już istnieje
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return false;

            // Hashowanie hasła (proste, możesz później dodać lepsze)
            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> LoginUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            // Weryfikacja hasła
            bool verified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return verified ? user : null;
        }
    }
}
