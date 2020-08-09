using System;
using System.ComponentModel.DataAnnotations;

namespace Agency.Data.Infrastructure
{
    public abstract class BaseModel : IAuditInfo
    {
        [Key]
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public BaseModel()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
