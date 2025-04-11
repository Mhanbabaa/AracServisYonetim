using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AracServisYonetim.Models
{
    public class Musteri
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ad")]
        [StringLength(50)]
        public string Ad { get; set; }

        [Required]
        [Display(Name = "Soyad")]
        [StringLength(50)]
        public string Soyad { get; set; }

        [Display(Name = "E-posta")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Display(Name = "Telefon")]
        [Phone]
        [StringLength(20)]
        public string Telefon { get; set; }

        [Display(Name = "Adres")]
        [StringLength(250)]
        public string Adres { get; set; }

        [Required]
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Arac> Araclar { get; set; }

        [Display(Name = "Tam Ad")]
        public string TamAd
        {
            get { return Ad + " " + Soyad; }
        }

        public Musteri()
        {
            Araclar = new HashSet<Arac>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
