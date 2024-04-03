using Core.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IParentRepository
    {
        public Task<Parent?> GetByIdAsync(Guid id);
        public Task<Parent?> GetByEmailAsync(string email);
    }
}
