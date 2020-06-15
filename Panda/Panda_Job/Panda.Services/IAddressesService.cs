﻿using Panda.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public interface IAddressesService
    {
        Task CreateAddressAsync(Address address);
        int CountOfAddressesPerUser(PandaUser user);
        IQueryable<Address> ListOfAddressesByUser(string userName);
        Task<Address> GetAddressByIdAsync(string addressId);
        string ShortenedAddressToString(Address fullAddress);
        Task MarkAsDeletedAsync(string id);
        Task UpdateAddressAsync(Address addressToUpdate);
    }
}
