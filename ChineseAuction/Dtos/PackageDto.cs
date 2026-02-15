using System.ComponentModel.DataAnnotations;

namespace ChineseAuction.Dtos
{
    public class CreatePackageDto
    {
        [Required, MaxLength(30)]
        public string Name { get; set; }=string.Empty;
        [Required, MaxLength(1000)]
        public string Description { get; set; }=string.Empty;
        [Required]
        public int NumOfCards { get; set; }
        [Required]
        public int Price { get; set; } = 10;
    }
    public class GetPackageDto
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int NumOfCards { get; set; }
        [Required]
        public int Price { get; set; } = 10;
    }
}
