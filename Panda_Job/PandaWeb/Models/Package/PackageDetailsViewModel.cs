
namespace PandaWeb.Models.Package
{
    public class PackageDetailsViewModel
    {
        public Panda.Domain.Address ShippingAddress { get; set; }

        public string Status { get; set; }

        public string EstimatedDeliveryDate { get; set; }

        public double Weight { get; set; }

        public string Recipient { get; set; }

        public string Description { get; set; }
    }
}
