using Panda.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Panda.Service
{
    public interface IAddressesService
    {
        void CreateAddress(Address address);

        int CountOfAddressesPerUser(PandaUser user);
    }
}
