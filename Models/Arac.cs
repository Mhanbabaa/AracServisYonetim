using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AracServisYonetim.Models
{
    public class Arac
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MusteriId { get; set; }

        [ForeignKey("MusteriId")]
        public virtual Musteri Musteri { get; set; }

        [Required]
        [Display(Name = "Marka")]
        [StringLength(50)]
        public string Marka { get; set; }

        [Required]
        [Display(Name = "Model")]
        [StringLength(50)]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Model Yılı")]
        [Range(1900, 2100)]
        public int ModelYili { get; set; }

        [Display(Name = "Plaka")]
        [StringLength(20)]
        public string Plaka { get; set; }

        [Display(Name = "VIN / Şasi No")]
        [StringLength(50)]
        public string VIN { get; set; }

        [Display(Name = "Kilometre")]
        public int Kilometre { get; set; }

        [Required]
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Servis> Servisler { get; set; }

        public Arac()
        {
            Servisler = new HashSet<Servis>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        // Aracın tam bilgisini döndüren yardımcı metot
        [NotMapped]
        public string TamBilgi
        {
            get { return $"{Plaka} - {Marka} {Model} ({Musteri?.TamAd})"; }
        }
    }
}
