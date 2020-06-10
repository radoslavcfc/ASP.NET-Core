﻿using Panda.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Panda.Services
{
    public interface IPackagesService
    {
        void CreatePackage(Package package);

        Package GetPackage(string id);

        ICollection<Package> GetAllPackages();

        void UpdatePackage(Package package);

        IQueryable<Package> GetPackagesWithRecipientAndStatus();
    }
}
