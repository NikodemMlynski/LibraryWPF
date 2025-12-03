using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserID { get; set; }
        public DateTime? ReservationDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public DateTime? EndDate { get; set; }
        public RentalStatus RentalStatus { get; set; }
    }
}
