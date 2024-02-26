using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VendorStoreAppBackend.Entities_Models
{
    public class Permissions_Resource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionResourceId { get; set; }

        [Required]
        [StringLength(120)]
        public string PermissionResourceName { get; set; } = string.Empty;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PermissionResourceCreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PermissionResourceUpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property if needed
        public ICollection<Permission>? Permissions { get; set; }
    }
}
