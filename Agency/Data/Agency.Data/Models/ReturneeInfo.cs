using Agency.Data.Infrastructure;

namespace Agency.Data.Models
{
    public class ReturneeInfo : BaseModelWithId
    {
        public string WorkerId { get; set; }
        public Worker Worker { get; set; }        
    }
}