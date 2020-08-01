using Agency.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agency.Data.Models
{
    public class Names : BaseDeletableModel<string>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        public Worker Worker { get; set; }

        public string WorkerId { get; set; }
    }
}
