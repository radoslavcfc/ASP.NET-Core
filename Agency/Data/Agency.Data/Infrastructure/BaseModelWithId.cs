using System;
using System.ComponentModel.DataAnnotations;

namespace Agency.Data.Infrastructure
{
    public class BaseModelWithId : BaseModel
    {
        [Key]
        public string Id { get; set; }
        public BaseModelWithId()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
