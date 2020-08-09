using Agency.Data.Infrastructure;

namespace Agency.Data.Models
{
    public class NewWorkerInfo : BaseDeletableModel
    {
        public Worker Worker { get; set; }

        public string WorkerId { get; set; }
    }
}