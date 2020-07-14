using Agency.Data.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agency.Data.Models
{
    public class AgencyUserRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public DateTime CreatedOn {get; set;}
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
