namespace VendorStoreAppBackend.Entities_Models
{
    public class Orders
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int VendorId { get; set; }

        public string OrderAddress { get; set; } = string.Empty;

        public DateTime? ReservationDate { get; set; } = DateTime.UtcNow;

        public DateTime? ReservationTime { get; set; } = DateTime.UtcNow;

        public float TotalPrice { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? OrderDeletedAt { get; set; } = DateTime.UtcNow;

        public DateTime? OrderCreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? OrderUpdatedAt { get; set; } = DateTime.UtcNow;

        public enum OrderStatus
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed
        }

        // Navigation properties
           public Users? User { get; set; }

           public Vendors? Vendor { get; set; }

    }
}
