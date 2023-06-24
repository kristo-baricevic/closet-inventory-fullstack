using Microsoft.EntityFrameworkCore;
using ClothingInventory.Models;



namespace ClothingInventory.Models
{
    public class ClothingInventoryContext : DbContext
    {
        public ClothingInventoryContext(DbContextOptions<ClothingInventoryContext> options)
            : base(options)
        {
        }

        public DbSet<ClothingItem> ClothingItems { get; set; }

        // Define your entity sets (DbSets) here

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity mappings and relationships here
        }
    }
}
