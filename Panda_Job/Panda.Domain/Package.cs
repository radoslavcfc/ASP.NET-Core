using Panda.Domain.Enums;
using System;

namespace Panda.Domain
{
    public class Package
    {
        public Package()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }

        public string Description { get; set; }

        public double Weight { get; set; }

        public Address ShippingAddress { get; set; }

        public PackageStatus Status { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public string RecipientId { get; set; }

        public PandaUser Recipient { get; set; }
    }
}
