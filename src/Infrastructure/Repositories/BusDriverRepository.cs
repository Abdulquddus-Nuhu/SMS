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
    public class BusDriverRepository : IBusDriverRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<BusDriverRepository> _logger;
        public BusDriverRepository(AppDbContext dbContext, ILogger<BusDriverRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<Busdriver?> GetBusdriverByEmail(string email)
        {
            return await _dbContext.Busdrivers.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
