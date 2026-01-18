using ChineseAuction.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChineseAuction.Dtos
{
    public class CreatePurchaseDto
    {
        [Required]
        public int GiftId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PackageId { get; set; }
    }
    public class GetPurchaseDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int GiftId { get; set; }
        [Required]
        public int PackageId { get; set; }
        [Required]
        public string UniquePackageId { get; set; }=string.Empty;
    }
    public class UpdatePurchaseDto
    {
        [Required]
        public bool IsWon { get; set; }
    }
}
