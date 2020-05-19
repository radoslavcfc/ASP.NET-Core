using Panda.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PandaWeb.Models.Address
{
    public class AddNewAddressModel
    {   [Required]
        public string Country { get; set; }
        [Required]
        public string Region { get; set; }
        public string Town { get; set; }
        public string StreetName { get; set; }

        public AddressType AddressType { get; set; }

        public PropertyType PropertyType { get; set; }
        public int Number { get; set; }
        public int Floor { get; set; }
        public int Flat { get; set; }
        public int Apartment { get; set; }


    }
}
