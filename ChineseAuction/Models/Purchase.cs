using ChineseAuction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChineseAuction.Models
{
    public class Purchase
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int GiftId { get; set; }
        [Required, ForeignKey("GiftId")]
        public Gift? Gift { get; set; } = null;
        [Required]
        public int UserId { get; set; }
        [Required, ForeignKey("UserId")]
        public User? User { get; set; } = null;
        [Required]
        public int PackageId { get; set; }
        [Required, ForeignKey("PackageId")]
        public Package? Package { get; set; }
        [Required]
        public DateTime Pruchase_date { get; set; } = DateTime.Now;
        [Required]
        public bool IsWon { get; set; } = false;
        [Required]
        public string UniquePackageId { get; set; } = string.Empty;
    }
}