using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Data.Identity
{
    public abstract class BaseIdentity : IdentityUser<Guid>, IBaseEntity
    {
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; protected set; } = string.Empty;
        public virtual DateTime? Deleted { get; protected set; }
        public string? CreatedBy { get; protected set; } = string.Empty;
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Modified { get; protected set; }
        public virtual string? LastModifiedBy { get; protected set; }
        protected BaseIdentity()
        {
            IsDeleted = false;
            Created = DateTime.UtcNow;
        }


        public object Clone()
        {
            return MemberwiseClone();
        }
    }

}
