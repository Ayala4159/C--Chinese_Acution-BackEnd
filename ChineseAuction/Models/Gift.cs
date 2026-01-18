using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChineseAuction.Models
{
    public class Gift
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }=string.Empty;
        [Required, MaxLength(100)]
        public string Description { get; set; }=string.Empty;
        public string? Details { get; set; }
        public string Picture { get; set; }=string.Empty;
        [Required]
        public int Value { get; set; }
        [Required]
        public int DonorId { get; set; }
        [Required, ForeignKey("DonorId")]
        public Donor? Donor { get; set; }=null;
        [Required]
        public int CategoryId { get; set; }
        [Required, ForeignKey("CategoryId")]
        public Category? Category { get; set; }=null;
        public int Purchases_quantity { get; set; }
        public ICollection<Purchase> Purchase { get; set; } = new List<Purchase>();
        [Required]
        public bool Is_lottery { get; set; } = false;
        [Required]
        public bool Is_approved { get; set; } = false;
    }
}
