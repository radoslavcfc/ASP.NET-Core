using Panda.Domain.Enums;

namespace PandaWeb.Models.Address
{
    public class ShortAddressDetailModel
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string Town { get; set; }
        public string StreetName { get; set; }

        public int Number { get; set; }

        public AddressType AddressType { get; set; }
    }
}
