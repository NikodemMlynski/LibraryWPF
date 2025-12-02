using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class AdminService: LibrarianService
    {
        public AdminService(): base()
        {

        }
        public async Task<bool> GenerateAutidReportAsync()
        {
            // later
            return true;
        }
    }
}
