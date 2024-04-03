using Core.Entities.Users;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ParentRepository : IParentRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ParentRepository> _logger;
        public ParentRepository(AppDbContext dbContext, ILogger<ParentRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Parent?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Parents.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Parent?> GetByEmailAsync(string email)
        {
            return await _dbContext.Parents.FirstOrDefaultAsync(t => t.Email == email);
        }

    }
}
