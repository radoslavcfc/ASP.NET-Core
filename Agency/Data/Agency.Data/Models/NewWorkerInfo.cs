using Agency.Data.Infrastructure;

namespace Agency.Data.Models
{
    public class NewWorkerInfo : BaseModelWithId
    {
        public Worker Worker { get; set; }

        public string WorkerId { get; set; }
    }
}