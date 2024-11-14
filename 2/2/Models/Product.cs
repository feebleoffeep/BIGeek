using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва товару є обов'язковою")]
        [StringLength(100, ErrorMessage = "Назва не повинна перевищувати 100 символів")]
        public string? Name { get; set; }

        [StringLength(1000, ErrorMessage = "Опис не повинен перевищувати 1000 символів")]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Ціна є обов'язковою")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ціна повинна бути більшою за 0")]
        public double Price { get; set; }

        [StringLength(200, ErrorMessage = "Альтернативні назви не повинні перевищувати 200 символів")]
        public string? AlternativeNames { get; set; }

        [StringLength(10, ErrorMessage = "Діагональ екрану не повинна перевищувати 10 символів")]
        public string? ScreenDiagonal { get; set; }

        [StringLength(30, ErrorMessage = "Колір не повинен перевищувати 30 символів")]
        public string? Color { get; set; }

        [StringLength(50, ErrorMessage = "Процесор не повинен перевищувати 50 символів")]
        public string? Processor { get; set; }

        [Range(1, 128, ErrorMessage = "Розмір оперативної пам'яті має бути від 1 до 128 ГБ")]
        public int? RamSize { get; set; }

        [Range(1, 2000, ErrorMessage = "Розмір сховища має бути від 1 до 2000 ГБ")]
        public int? StorageSize { get; set; }

        [StringLength(20, ErrorMessage = "Роздільна здатність екрану не повинна перевищувати 20 символів")]
        public string? ScreenResolution { get; set; }

        [StringLength(50, ErrorMessage = "Тип графіки не повинен перевищувати 50 символів")]
        public string? GraphicsType { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Ємність батареї повинна бути більшою за 0")]
        public double? BatteryCapacity { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}



