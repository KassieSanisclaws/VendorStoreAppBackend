using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VendorStoreAppBackend.Entities_Models
{
    public class Vendors
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorId { get; set; }

        [Required]
        [StringLength(120)]
        public string? VendorName { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        [EmailAddress]
        public string? VendorEmail { get; set; }

        [Required]
        [StringLength(10)] // Adjust the length according 
        public string? VendorNumber { get; set; } 

        [Required]
        [StringLength(120)]
        public string? VendorAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        public string? VendorBussRegID { get; set; }

        [Required]
        [StringLength(120)]
        public string? VendorBussLicense { get; set; }

        [Required]
        [StringLength(400)] // Adjust the length according to your needs
        public string? VendorPasswordHash { get; set; }

        [Required]
        [StringLength(400)] // Adjust the length according to your needs
        public string? VendorImg { get; set; }

        [StringLength(255)]
        public string? VendorGreetingsIntro { get; set; } = string.Empty;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime VendorCreatedAt { get; set; } = default;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime VendorLastLoginAt { get; set; } = default;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime VendorUpdatedAt { get; set; } = default;
    }
}
