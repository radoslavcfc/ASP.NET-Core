using Agency.Data.Infrastructure;

namespace Agency.Data.Models
{
    public class Address : BaseDeletableModel<string>
    {
        public string Number { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string Region { get; set; }
        public string Couttry { get; set; }
        public int PostCode { get; set; }
        public int Flat { get; set; }
        public int Floor { get; set; }
        public string Entrance { get; set; }
    }
}
