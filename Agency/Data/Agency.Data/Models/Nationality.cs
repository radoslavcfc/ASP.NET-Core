﻿using Agency.Data.Infrastructure;

namespace Agency.Data.Models
{
    public class Nationality : BaseDeletableModel<string>
    {
        public string NationalityCountry { get; set; }

        public Worker Worker { get; set; }

        public string WorkerId { get; set; }
    }
}
