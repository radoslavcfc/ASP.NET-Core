using System.Linq;
using System.Threading.Tasks;

using Panda.Domain;

namespace Panda.Services
{
    public interface IAddressesService
    {
        Task CreateAddressAsync(Address address);
        int CountOfAddressesPerUser(PandaUser user);
        IQueryable<Address> ListOfAddressesByUser(string userName);      
        string ShortenedAddressToString(Address fullAddress);
        Task MarkAsDeletedAsync(string id);
        Task UpdateAddressAsync(Address addressToUpdate);
        Task<Address> GetAddressByIdAsync(string addressId);
    }
}
