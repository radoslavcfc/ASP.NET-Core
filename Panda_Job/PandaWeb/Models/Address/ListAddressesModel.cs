using System.Collections.Generic;

namespace PandaWeb.Models.Address
{
    public class ListAddressesModel
    {
        public IEnumerable<ShortAddressDetailModel> ShortAddressDetailsModelsList { get; set; }
    }
}
