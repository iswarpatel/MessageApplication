using MessageApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageApplication.Models
{
    public class MessagesContext : DbContext
    {
        public MessagesContext(DbContextOptions<MessagesContext> options)
            : base(options)
        {
        }

        public DbSet<MessagesItem> MessagesItems { get; set; }
    }
}