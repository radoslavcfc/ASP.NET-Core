using System;
using Agency.Data.Enums;

namespace Agency.Data.Models
{
    public class Worker
    {
        public string NamesId { get; set; }
        public Names Names { get; set; }
        public DateTime DOB { get; set; }
        public Gender Gender { get; set; }
        public string NationalityId { get; set; }
        public Nationality Nationality { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public string [] Relatives { get; set; }
        public string CurrentlyIn { get; set; }
        public WorkerType WorkerType { get; set; }        
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public OfferStatus OfferStatus { get; set; }
        public string Notes { get; set; }
        public string ReturneeInfoId { get; set; }
        public ReturneeInfo ReturneeInfo { get; set; }

        public string NeWorkerInfoId { get; set; }
        public NewWorkerInfo NewWorkerInfo { get; set; }
            
    }
}
