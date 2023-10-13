using Nazobook.Data;

namespace Nazobook.Models
{
    public class HomeIndexModel
    {
        public User? User { get; set; }
        public List<Gonderi> Gonderi { get; set; }
    }
}