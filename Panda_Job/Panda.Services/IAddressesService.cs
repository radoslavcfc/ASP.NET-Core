using Panda.Domain;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IAddressesService
    {
        void CreateAddress(Address address);

        int CountOfAddressesPerUser(PandaUser user);
        IEnumerable<Address> ListOfAddressesByUser(string userName);
    }
}
