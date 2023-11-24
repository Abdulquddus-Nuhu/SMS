using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Data.Identity
{
    public interface IBaseEntity : ICloneable
    {
        public bool IsDeleted { get; set; }
    }

}
