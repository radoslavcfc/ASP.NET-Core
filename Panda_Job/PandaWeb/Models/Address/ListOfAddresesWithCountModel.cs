using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace PandaWeb.Models.Address
{
    public class ListOfAddresesWithCountModel
    {
        public ListOfAddresesWithCountModel()
        {
            this.ListOfAdresses = new List<ApiAddressModel>();
        }   
        public int CountOfAddresses { get; set; }
        public IList<ApiAddressModel> ListOfAdresses {get;set;}
    }
}
