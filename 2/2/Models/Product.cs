using System.ComponentModel.DataAnnotations.Schema;

namespace _2.Models
{

    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public string? AlternativeNames { get; set; }
        public string? ScreenDiagonal { get; set; }
        public string? Color { get; set; }
        public string? Processor { get; set; }
        public int? RamSize { get; set; }
        public int? StorageSize { get; set; }
        public string? ScreenResolution { get; set; }
        public string? GraphicsType { get; set; } 
        public double? BatteryCapacity { get; set; } 


        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}


