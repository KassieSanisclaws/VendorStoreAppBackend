using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace VendorStoreAppBackend.Entities_Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UsersId { get; set; }

        [Required]
        [StringLength(120)]
        public string? UsersFirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        public string? UsersLastName { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        [EmailAddress]
        public string? UsersEmail { get; set; }

        [Required]
        [StringLength(400)] // Adjust the length according to your needs
        public string? UsersPasswordHash { get; set; }

        [Required]
        [StringLength(10)] // Adjust the length according to your needs
        public string? UsersMobile { get; set; }

        [Required]
        public Gender UsersGender { get; set; }

        [StringLength(500)]
        public string UsersImg { get; set; } = string.Empty;

        [Required]
        public DateTime UsersDateOfBirth { get; set; } = DateTime.MinValue;

        [StringLength(255)]
        public string UsersGreetingIntro { get; set; } = string.Empty;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UsersCreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UsersLastLogin { get; set; } = DateTime.UtcNow;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UsersUpdatedAt { get; set; } = DateTime.UtcNow;

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        // Navigation property
        public ICollection<User_Roles>? UserRoles { get; set; }
    }
}
