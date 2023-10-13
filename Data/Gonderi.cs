namespace Nazobook.Data
{
    public class Gonderi
    {
        public int GonderiId { get; set; }
        public string? Baslik { get; set; }
        public string? Mesaj { get; set; }
        public string? ResimYol { get; set; }
        public bool isDuyuru { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int GonderiNo { get; set; }
        public DateTime GonderiTarih { get; set; }
        
    }
}