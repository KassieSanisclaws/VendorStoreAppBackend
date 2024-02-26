using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace VendorStoreAppBackend.Entities_Models
{
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }

        [Required]
        [StringLength(50)]
        public string PermissionName { get; set; } = string.Empty;

        [Required]
        public int PermissionResourceId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public bool PermissionCanAdd { get; set; }
        public bool PermissionCanEdit { get; set; }
        public bool PermissionCanView { get; set; }
        public bool PermissionCanDelete { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PermissionCreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PermissionUpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Permissions_Resource? PermissionsResource { get; set; }
        public Roles? Role { get; set; }
    }
}
