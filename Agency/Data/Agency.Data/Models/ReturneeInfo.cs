using Agency.Data.Infrastructure;

namespace Agency.Data.Models
{
    public class ReturneeInfo : BaseDeletableModel<string>
    {
        public string WorkerId { get; set; }
        public Worker Worker { get; set; }        
    }
}