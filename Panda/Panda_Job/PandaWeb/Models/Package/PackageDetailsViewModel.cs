
namespace PandaWeb.Models.Package
{
    public class PackageDetailsViewModel
    {
        public string Id { get; set; }
        public string Status { get; set; }

        public string EstimatedDeliveryDate { get; set; }

        public double Weight { get; set; }

        public string RecipientId { get; set; }
        public string RecipientFullName { get; set; }

        public string Description { get; set; }

        public string ShippingAddress { get; set; }
    }
}
