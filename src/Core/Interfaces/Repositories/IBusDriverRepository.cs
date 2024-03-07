using Core.Entities.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IBusDriverRepository
    {
        public Task<Busdriver?> GetBusdriverByEmail(string email);
    }
}
