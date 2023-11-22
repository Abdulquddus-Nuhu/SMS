using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Access.Data.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public bool IsDeleted { get; set; }

        public Role()
        {
        }

        public Role(string roleName)
        {
            Name = roleName;
        }

    }
}
