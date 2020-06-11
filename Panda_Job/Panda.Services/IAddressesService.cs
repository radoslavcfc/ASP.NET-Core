using Panda.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public interface IAddressesService
    {
        void CreateAddress(Address address);

        int CountOfAddressesPerUser(PandaUser user);
        IQueryable<Address> ListOfAddressesByUser(string userName);
        Address GetAddressById(string addressId);

        string ShortenedAddressToString(Address fullAddress);
        void MarkAsDeleted(string id);
    }
}
