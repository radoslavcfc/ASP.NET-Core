using Agency.Data.Enums;
using System;

namespace Agency.Web.Models.Workers
{
    public class CompleteWorkerDataModel
    {
        public DateTime DOB { get; set; }
        public Gender Gender { get; set; }
        public string Relatives { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public string CurrentlyIn { get; set; }
        public WorkerType WorkerType { get; set; }
    }
}
