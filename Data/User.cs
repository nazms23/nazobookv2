using System.ComponentModel.DataAnnotations;

namespace Nazobook.Data
{
    public class User
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
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [Required]
        public string? Sifre { get; set; }
        public bool isBanned { get; set; }
        public int GonderiSayi { get; set; } //Toplam gönderi sayısı
        public int GonderiSayi2 { get; set; } //Güncel gönderi sayısı (silinenlerin eksilceği falan)
        public bool isAdmin { get; set; }
        public DateTime Tarih { get; set; }
        public int UyePp { get; set; } 
        public string? ResimYol { get; set; }
        public List<Gonderi>? Gonderiler { get; set; }
    }
}