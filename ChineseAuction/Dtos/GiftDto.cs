using ChineseAuction.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChineseAuction.Dtos
{
    public class GetGiftDto
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string Description { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string Picture { get; set; } = string.Empty;
        [Required]
        public int Value { get; set; }
        [Required]
        public int DonorId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public bool Is_lottery { get; set; }
        [Required]
        public bool Is_approved { get; set; } = false;

    }
    public class CreateGiftDto
    {
        [Required, MaxLength(30)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string Description { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string Picture { get; set; } = string.Empty;
        [Required]
        public int Value { get; set; }
        [Required]
        public int DonorId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public bool Is_lottery { get; set; }
        [Required]
        public bool Is_approved { get; set; } = false;
    }
    public class UserUpdateGiftDto
    {
        [Required]
        public int Purchases_quantity { get; set; }
    }
    public class ApproveGiftDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool Is_approved { get; set; } = false;
    }
}
