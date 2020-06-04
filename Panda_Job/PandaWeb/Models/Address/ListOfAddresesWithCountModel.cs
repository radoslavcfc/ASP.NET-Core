using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace PandaWeb.Models.Address
{
    public class ListOfAddresesWithCountModel
    {
        public ListOfAddresesWithCountModel()
        {
            this.ListOfAdresses = new List<ShortAddressDetailModel>();
        }   
        public int CountOfAddresses { get; set; }
        public IList<ShortAddressDetailModel> ListOfAdresses {get;set;}
    }
}
