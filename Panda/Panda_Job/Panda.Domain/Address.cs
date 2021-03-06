﻿using System;

using Panda.Domain.Enums;

namespace Panda.Domain
{
    public class Address
    {
        public Address()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string  Id { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Town { get; set; }
        public string StreetName { get; set; }
        public AddressType? AddressType { get; set; }
        public PropertyType PropertyType { get; set; }
        public int Number { get; set; }
        public string UserId { get; set; }
        public PandaUser User { get; set; }
        public Flat Flat { get; set; }

        public bool IsDeleted { get; set; }
    }
}
