using System;
using Agency.Data.Enums;

namespace Agency.Data.Models
{
    public class Worker
    {
        public Names Names { get; set; }
        public DateTime DOB { get; set; }
        public Gender Gender { get; set; }
        public Nationality Nationality { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public string [] Relatives { get; set; }
        public string CurrentlyIn { get; set; }
        public WorkerType WorkerType { get; set; }
        
        public DateTime AvailableFrom { get; set; }

        public DateTime AvailableTo { get; set; }
        //Returnee Info
        //New worker info
        
        //Offer Status
        //Notes
    }
}
