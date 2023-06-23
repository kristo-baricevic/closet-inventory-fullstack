using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingInventory.Models
{
    public class ClothingItem
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Category { get; set; }
    }
}
