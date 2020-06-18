using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace Panda.Domain
{
    public class PandaUser : IdentityUser
    {
        public PandaUser()
        {
            this.Packages = new List<Package>();
            this.Receipts = new List<Receipt>();
            this.Addresses = new List<Address>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondContactNumber { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public PandaUserRole UserRole { get; set; }

        public ICollection<Package> Packages { get; set; }

        public ICollection<Receipt> Receipts { get; set; }

        public DateTime RegisteredOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
