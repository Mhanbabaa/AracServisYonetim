using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AracServisYonetim.Models
{
    public enum ServisDurumu
    {
        [Display(Name = "Bekliyor")]
        Bekliyor = 0,
        
        [Display(Name = "İşlemde")]
        Islemde = 1,
        
        [Display(Name = "Tamamlandı")]
        Tamamlandı = 2,
        
        [Display(Name = "İptal Edildi")]
        IptalEdildi = 3
    }

    public class Servis
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AracId { get; set; }

        [ForeignKey("AracId")]
        public virtual Arac Arac { get; set; }

        [Required]
        [Display(Name = "Servis Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime ServisTarihi { get; set; }

        [Display(Name = "Teslim Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime? TeslimTarihi { get; set; }

        [Display(Name = "Açıklama")]
        [StringLength(500)]
        public string Açıklama { get; set; }

        [Display(Name = "Servis Bedeli")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal")]
        [Range(0, 9999999.99, ErrorMessage = "Servis bedeli 0 ile 9,999,999.99 arasında olmalıdır.")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal ServisBedeli { get; set; }

        [Required]
        [Display(Name = "Durumu")]
        public ServisDurumu Durumu { get; set; }

        [Required]
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime UpdatedAt { get; set; }

        public Servis()
        {
            ServisTarihi = DateTime.Now;
            Durumu = ServisDurumu.Bekliyor;
            ServisBedeli = 0.00m;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
