using Microsoft.EntityFrameworkCore;

namespace WebApi.Models{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Student>? Students { get; set; }

        public DbSet<Show>? Shows { get; set;}
        // More DbSets go here for each table
        
    }
    
}