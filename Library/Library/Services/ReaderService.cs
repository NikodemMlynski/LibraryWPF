using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<List<Rental>> GetAllRentalsAsync()
        {
            var currentUser = AuthService.Instance.CurrentUser;
            if (currentUser == null)
            {
                return new List<Rental>();

            }
            return await _context.Rentals.Where(r => r.UserID == currentUser.Id).Include(r => r.Book).AsNoTracking().ToListAsync();

        }

        public async Task<string> ReturnBookAsync(Rental rentalToReturn)
        {
            var currentUser = AuthService.Instance.CurrentUser;
            if (currentUser == null) return "Error: User is not authorized";

            try
            {
                var rental = await _context.Rentals
                    .Include(r => r.Book)
                    .FirstOrDefaultAsync(r => r.Id == rentalToReturn.Id);

                if (rental == null) return "Error: Rental not found";
                if (rental.UserID != currentUser.Id) return "Error: You do not have permission for this rental";
                if (rental.RentalStatus == RentalStatus.Returned) return "Error: Book is already returned";

                rental.RentalStatus = RentalStatus.Returned;
                rental.EndDate = DateTime.UtcNow; // Ustaw aktualną datę zwrotu (UTC)

                if (rental.Book != null)
                {
                    rental.Book.Quantity++;
                    _context.Books.Update(rental.Book);
                }

                _context.Rentals.Update(rental);
                await _context.SaveChangesAsync();

                return "Success: The book has been successfully returned.";
            }
            catch (Exception ex)
            {
                return $"Database error: {ex.Message} {ex.InnerException?.Message}";
            }
        }
        public async Task<string> UpdateRentalReturnDate(Rental rental, DateTime selectedReturnDate)
        {
            var currentUser = AuthService.Instance.CurrentUser;
            if (currentUser == null) return "Error: User is not authorized";

            try
            {
                var rentalToUpdate = await _context.Rentals.FindAsync(rental.Id);

                if (rentalToUpdate == null) return "Error: Rental not found";

                if (rentalToUpdate.UserID != currentUser.Id) return "Error: You do not have permission do this rental";

                if (selectedReturnDate.Date < DateTime.Now.Date) return "Error: New return date cannot be in past";
                DateTime utcDate = DateTime.SpecifyKind(selectedReturnDate, DateTimeKind.Utc);

                rentalToUpdate.ExpectedReturnDate = utcDate;
                _context.Rentals.Update(rentalToUpdate);
                await _context.SaveChangesAsync();
                return "Success: rental expected return date has been updated";
            } catch (Exception ex)
            {
                return "Database error: {ex.Message}";
            }
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
