using System;
using Agency.Data.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Agency.Data.Models
{
    public class AgencyUserRole : IdentityRole, IDeletableEntity, IAuditInfo
    {
        public DateTime CreatedOn {get; set;}
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
