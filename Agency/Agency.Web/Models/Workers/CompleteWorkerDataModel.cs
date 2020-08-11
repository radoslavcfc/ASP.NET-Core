using Agency.Data.Enums;
using Agency.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Agency.Web.Models.Workers
{
    public class CompleteWorkerDataModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name ="Date of birth")]
        public DateTime DOB { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Display(Name = "Name/s of person/people who you want to travel and work with:")]
        public string Relatives { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Available from")]
        public DateTime AvailableFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Available to")]
        public DateTime AvailableTo { get; set; }

        [Required]
        [Display(Name = "Where are you currently?")]
        public string CurrentlyIn { get; set; }

        [Required]
        [Display(Name = "How did you find about us. Please fill in the names of your friend/s or relative/s who told you about us?")]
        
        public string ConnectionSource { get; set; }

        [Required]
        public Nationality Nationality { get; set; }
    }
}
