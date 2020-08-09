using System;

namespace Agency.Data.Infrastructure
{
    public abstract class BaseDeletableModel : IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
