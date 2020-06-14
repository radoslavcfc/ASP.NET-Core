namespace PandaWeb.Models.Package
{
    public class PackageDeliveredViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string ShippingAddress { get; set; }

        public string RecipientId { get; set; }
        public string RecipientFullName { get; set; }

        public double Weight { get; set; }
    }
}
