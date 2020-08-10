using Agency.Data.Enums;
using Agency.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Agency.Web.Models.Workers
{
    public class CompleteWorkerDataModel
    {
        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public string Relatives { get; set; }

        [Required]
        public DateTime AvailableFrom { get; set; }

        [Required]
        public DateTime AvailableTo { get; set; }

        [Required]
        public string CurrentlyIn { get; set; }

        [Required]
        public WorkerType WorkerType { get; set; }

        [Required]
        public Nationality Nationality { get; set; }
    }
}
