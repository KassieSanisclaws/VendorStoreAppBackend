using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VendorStoreAppBackend.Entities_Models
{
    public class Roles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [StringLength(120)]
        public string RoleName { get; set; } = string.Empty;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RoleCreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RoleDeletedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RoleUpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property if needed
        public ICollection<Permission>? Permissions { get; set; }
    }
}
