

using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace InfrastrctureLayer.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
