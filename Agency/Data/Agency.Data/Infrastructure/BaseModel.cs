using System;
using System.ComponentModel.DataAnnotations;

namespace Agency.Data.Infrastructure
{
    public abstract class BaseModel : BaseDeletableModel, IAuditInfo
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        
    }
}
