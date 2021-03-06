﻿using Agency.Data.Infrastructure;

namespace Agency.Data.Models
{
    public class ContactInfo : BaseModelWithId
    {
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string FacebookProfile { get; set; }
        public string MobileSecond { get; set; }
        public string AddressId { get; set; }
        public Address Address { get; set; }
        public Worker Worker { get; set; }
        public string WorkerId { get; set; }
    }
}
