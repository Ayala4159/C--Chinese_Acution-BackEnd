using System.ComponentModel.DataAnnotations;

namespace ChineseAuction.Models
{
    public enum Role{manager=0,customer=1}
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required,EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }=string.Empty;
        [Required,MinLength(8),MaxLength(20)]
        public string Password { get; set; }=string.Empty;
        [Required,MaxLength(30)]
        public string First_name { get; set; }=string.Empty;
        [Required,MaxLength(30)]
        public string Last_name { get; set; }=string.Empty;
        public string? Phone { get; set; }
        [Required]
        public Role Role { get; set; }= Role.customer;
        public ICollection<Purchase> Purchase { get; set; } = new List<Purchase>();
    }
}
