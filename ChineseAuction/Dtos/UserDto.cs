using ChineseAuction.Models;
using System.ComponentModel.DataAnnotations;

namespace ChineseAuction.Dtos
{
    public class CreateUserDto
    {
        [Required, EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }=string.Empty;
        [Required, MinLength(8), MaxLength(20)]
        public string Password { get; set; }=string.Empty;
        [Required, MaxLength(30)]
        public string First_name { get; set; }=string.Empty;
        [Required, MaxLength(30)]
        public string Last_name { get; set; }=string.Empty;
        public string? Phone { get; set; }
    }
    public class GetUserDto
    {
        [Required]
        public int Id { get; set; }
        public string Email { get; set; }= string.Empty;
        [Required, MaxLength(30)]
        public string First_name { get; set; }= string.Empty;
        [Required, MaxLength(30)]
        public string Last_name { get; set; }=string.Empty;
        public string? Phone { get; set; }
        [Required]
        public Role Role { get; set; } = Role.customer;
        public ICollection<Purchase> Purchase { get; set; } = new List<Purchase>();

    }
}
