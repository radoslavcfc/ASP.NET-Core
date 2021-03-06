﻿using Panda.Domain.Enums;

namespace PandaWeb.Models.Address
{
    public class ShortAddressDetailModel
    {
        public string Id { get; set; }
        public string ShotenedContent { get; set; }

        public AddressType AddressType { get; set; }
    }
}
