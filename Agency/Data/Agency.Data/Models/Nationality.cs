using Agency.Data.Infrastructure;
using System.Collections;
using System.Collections.Generic;

namespace Agency.Data.Models
{
    public class Nationality : BaseModelWithId
    {
        public Nationality()
        {
            this.WorkerNationalities = new HashSet<WorkerNationality>();
        }
        public string NationalityCountry { get; set; }

        public ICollection<WorkerNationality> WorkerNationalities { get; set; }
    }
}
