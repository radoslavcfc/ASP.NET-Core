﻿using Agency.Data.Enums;
using Agency.Data.Infrastructure;
using System;
using System.Collections.Generic;

namespace Agency.Data.Models
{
    public class Worker : BaseModelWithId
    {
        public Worker() : base()
        {
            this.WorkerNationalities = new HashSet<WorkerNationality>();
            this.OfferStatus = OfferStatus.Waiting;
        }

        //Worker based properties
        public DateTime DOB { get; set; }
        public Gender Gender { get; set; }         
        public string ConnectionSource { get; set; }
        public string Relatives { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public string CurrentlyIn { get; set; }
        public WorkerType WorkerType { get; set; }        

        //Navigation properties 
        public ICollection<WorkerNationality> WorkerNationalities { get; set; }
        public Names Names { get; set; }
        public ContactInfo ContactInfo { get; set; }                 
        public ReturneeInfo ReturneeInfo { get; set; }
        public NewWorkerInfo NewWorkerInfo { get; set; }          
        public AgencyUser AgencyUser { get; set; }

        //Properties to be used by the HR Manager
        public string Notes { get; set; }
        public OfferStatus OfferStatus { get; set; }
    }
}
