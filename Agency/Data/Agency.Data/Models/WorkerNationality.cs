using System;
using System.Collections.Generic;
using System.Text;

namespace Agency.Data.Models
{
    public class WorkerNationality
    {
        public string WorkerId { get; set; }
        public Worker Worker { get; set; }
        public string NationalityId { get; set; }

        public Nationality Nationality { get; set; }
    }
}
