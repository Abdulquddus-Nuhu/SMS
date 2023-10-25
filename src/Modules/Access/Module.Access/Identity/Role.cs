using System;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace Module.Access.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
        {
        }
        public Role(string roleName)
        {
            Name = roleName;
        }
    }

}

