using Panda.Domain.Enums;

namespace Panda_Job.Models.Address
{
    public class AddNewAdressModel
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string Town { get; set; }
        public string StreetName { get; set; }

        public AddressType AddressType { get; set; }

        public PropertyType PropertyType { get; set; }
        public int Number { get; set; }

    }
}
