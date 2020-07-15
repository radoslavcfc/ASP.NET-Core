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

//Currently in
//Type(new/R)
//Returnee Info
//New worker info
//Available Period
//Offer Status
//Notes

    }
}
