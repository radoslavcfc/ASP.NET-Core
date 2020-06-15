using Panda.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public interface IAddressesService
    {
        Task CreateAddress(Address address);

        int CountOfAddressesPerUser(PandaUser user);
        IQueryable<Address> ListOfAddressesByUser(string userName);
        Task<Address> GetAddressById(string addressId);

        string ShortenedAddressToString(Address fullAddress);
        Task MarkAsDeleted(string id);
        Task UpdateAddress(Address addressToUpdate);
    }
}
