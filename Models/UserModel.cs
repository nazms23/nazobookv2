using System.ComponentModel.DataAnnotations;

namespace Nazobook.Models
{
    public class UserModel
    {
        [Required]
        public int UserId { get; set; }
        [Display(Name = "Kullanıcı Adı")]
        [Required]
        public string? KullaniciAdi { get; set; }
        [Display(Name = "E posta")]
        [EmailAddress]
        [Required]
        public string? Eposta { get; set; }  
    }
}