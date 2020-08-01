using Agency.Data.Infrastructure;

namespace Agency.Data.Models
{
    public class NewWorkerInfo :BaseDeletableModel<string>
    {
        public Worker Worker { get; set; }

        public string WorkerId { get; set; }
    }
}