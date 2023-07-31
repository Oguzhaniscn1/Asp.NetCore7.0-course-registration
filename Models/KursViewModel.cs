using efcoreApp.Data;
using System.ComponentModel.DataAnnotations;

namespace efcoreApp.Models
{
    public class KursViewModel
    {
        [Key]
        public int KursId { get; set; }
        [Required]
        [StringLength(50)]
        public string? Baslık { get; set; }
        public int OgretmenId { get; set; }
        public ICollection<KursKayit> KursKayitlari { get; set; } = new List<KursKayit>();



    }
}
