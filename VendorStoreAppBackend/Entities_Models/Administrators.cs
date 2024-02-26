using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace VendorStoreAppBackend.Entities_Models
{
    public class Administrators
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }

        [Required]
        [StringLength(50)]
        public string AdminFirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string AdminLastName { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        [EmailAddress]
        public string? AdminEmail { get; set; }

        [Required]
        [StringLength(400)] // Adjust the length according to your needs
        public string? AdminPasswordHash { get; set; }

        [Required]
        public Gender AdminGender { get; set; }

        [StringLength(500)]
        public string AdminImg { get; set; } = string.Empty;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime AdminCreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime AdminLastLoginAt { get; set; } = DateTime.UtcNow;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime AdminUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

