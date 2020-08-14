using Agency.Data.Infrastructure;
using System.Collections;
using System.Collections.Generic;

namespace Agency.Data.Models
{
    public class Nationality : BaseModelWithId
    {
        public Nationality()
        {
            this.Workers = new HashSet<Worker>();
        }
        public string NationalityCountry { get; set; }

        public ICollection<Worker> Workers { get; set; }
    }
}
