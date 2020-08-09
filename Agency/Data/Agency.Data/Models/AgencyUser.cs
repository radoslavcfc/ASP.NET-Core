﻿using System;
using Agency.Data.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Agency.Data.Models
{
    public class AgencyUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public DateTime CreatedOn {get; set;}
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
