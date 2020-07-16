using Agency.Data.Infrastructure;

namespace Agency.Data.Models
{
    public class ContactInfo : BaseDeletableModel<string>
    {
        public string Mobile { get; set; }

        public string Email { get; set; }

        public string FacebookProfile { get; set; }

        public string MobileSecond { get; set; }

        public string AddressId { get; set; }
        public Address Address { get; set; }
    }
}
