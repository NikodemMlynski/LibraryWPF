using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Services
{
    public class LibrarianService
    {
        private readonly AppDbContext _context;

        public LibrarianService()
        {
            _context = new AppDbContext();
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.AsNoTracking().ToListAsync();
        }

        public async Task<bool> AddBookAsync(string title, string author, int quantity)
        {
            try
            {
                var book = new Book { Title = title, Author = author, Quantity = quantity };
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        // NOWE: Usuwanie książki
        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        // NOWE: Aktualizacja książki
        public async Task<bool> UpdateBookAsync(int id, string title, string author, int quantity)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book != null)
                {
                    book.Title = title;
                    book.Author = author;
                    book.Quantity = quantity;

                    _context.Books.Update(book);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch { return false; }
        }
    }
}