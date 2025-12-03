using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class ReaderService
    {
        private readonly AppDbContext _context;
        public ReaderService()
        {
            _context = new AppDbContext();
        }
        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.AsNoTracking().ToListAsync();

        }

        public async Task<string> AddRentalAsync(Book book, DateTime expectedReturnDate)
        {
            var currentUser = AuthService.Instance.CurrentUser;
            if (currentUser == null) return "Błąd: Użytkownik nie jest zalogowany.";

            try
            {
                var bookToUpdate = await _context.Books.FindAsync(book.Id);
                if (bookToUpdate == null) return "Błąd: Książka nie istnieje.";

                RentalStatus status;
                DateTime? reservationDate;
                DateTime? startDate;
                string message;

                // Ustawienie daty w UTC dla Postgresa
                DateTime utcExpectedDate = DateTime.SpecifyKind(expectedReturnDate, DateTimeKind.Utc);

                if (bookToUpdate.Quantity > 0)
                {
                    status = RentalStatus.Active;
                    reservationDate = null;
                    startDate = DateTime.UtcNow;

                    bookToUpdate.Quantity--;
                    _context.Books.Update(bookToUpdate);
                    message = "Sukces: Książka została wypożyczona.";
                }
                else
                {
                    status = RentalStatus.Reservated;
                    reservationDate = DateTime.UtcNow;
                    startDate = null;
                    message = "Sukces: Książka została zarezerwowana.";
                }

                var rental = new Rental
                {
                    BookId = bookToUpdate.Id,
                    UserID = currentUser.Id,
                    RentalStatus = status,
                    StartDate = startDate,
                    ReservationDate = reservationDate,
                    ExpectedReturnDate = utcExpectedDate, // <-- ZAPISUJEMY DATĘ
                    EndDate = null
                };

                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync();

                return message;
            }
            catch (Exception ex)
            {
                return $"Błąd bazy danych: {ex.Message} {ex.InnerException?.Message}";
            }
        
        }
    }
}
