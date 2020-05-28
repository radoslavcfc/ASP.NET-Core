using Panda.Domain.Enums;
using PandaWeb.Models.Flat;
using System.ComponentModel.DataAnnotations;

namespace PandaWeb.Models.Address
{
    public class AddNewAddressModel
    {
        [Required]
        public string Country { get; set; }
        [Required]
        public string Region { get; set; }
        public string Town { get; set; }
        public string StreetName { get; set; }

        public AddressType? AddressType { get; set; }

        public PropertyType PropertyType { get; set; }
        public int Number { get; set; }
        public AddFlatModel FlatModel { get; set; } 

    }
}
