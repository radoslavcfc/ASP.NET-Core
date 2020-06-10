using System;

namespace Panda.Domain
{
    public class Flat
    {
        public Flat()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public int? Floor { get; set; }
        public string Entrance { get; set; }
        public int? Apartment { get; set; }
        public Address Address { get; set; }
        public string AddressId { get; set; }
    }
}
