using Microsoft.EntityFrameworkCore;
using SavingsAPI.Models;

namespace SavingsAPI.Data
{
    public class SavingsDbContext : DbContext
    {
        public SavingsDbContext(DbContextOptions<SavingsDbContext> options)
            : base(options) { }

        public DbSet<SavingsAcc> SavingsAccounts { get; set; }
    }
}