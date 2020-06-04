namespace PandaWeb.Models.Package
{
    public class PackageDeliveredViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public Panda.Domain.Address ShippingAddress { get; set; }

        public string Recipient { get; set; }

        public double Weight { get; set; }
    }
}
