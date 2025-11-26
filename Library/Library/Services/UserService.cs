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
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return false;

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

        public async Task<(object identity, UserRole role)> LoginAsync(string email, string password)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == email);
            if (admin != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                    return (admin, UserRole.Admin);
            }

            var librarian = await _context.Librarians.FirstOrDefaultAsync(l => l.Email == email);
            if (librarian != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, librarian.PasswordHash))
                    return (librarian, UserRole.Librarian);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                    return (user, UserRole.Reader);
            }

            return (null, UserRole.None);
        }
    }
}
