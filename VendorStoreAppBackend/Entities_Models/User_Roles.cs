using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VendorStoreAppBackend.Entities_Models
{
    public class User_Roles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRolesId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UserRolesCreatedAt { get; set; }

        public DateTime? UserRolesDeletedAt { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UserRolesUpdatedAt { get; set; }

        // Navigation properties
        public Roles? Role { get; set; }
        public Users? User { get; set; }
    }
}
