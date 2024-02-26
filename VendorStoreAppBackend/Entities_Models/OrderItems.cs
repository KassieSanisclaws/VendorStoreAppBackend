namespace VendorStoreAppBackend.Entities_Models
{
    public class OrderItems
    {
        public int OrderItemsId { get; set; }

        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public DateTime? OrderItemsCreatedAt { get; set; }

        public DateTime? OrderItemsUpdatedAt { get; set;}

        // Navigation properties
        public Orders? Order { get; set; }

    }
}
