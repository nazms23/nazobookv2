using System.ComponentModel.DataAnnotations;
using Nazobook.Data;

namespace Nazobook.Models
{
    public class Usergirismodel
    {
        [EmailAddress]
        [Required]
        public string Eposta { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Sifre { get; set; }
    }
}