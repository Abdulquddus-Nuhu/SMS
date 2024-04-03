using Core.Entities;
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
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<StudentRepository> _logger;
        public StudentRepository(AppDbContext dbContext, ILogger<StudentRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IQueryable<Student> GetStudentsWithBusServiceAsync(Guid busId)
        {
            return _dbContext.Students
                .Include(x => x.Grade)
                .Where(x => x.BusId == busId)
                .AsNoTracking();
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Students.Include(t => t.Grade).FirstOrDefaultAsync(t => t.Id == id);
        }


    }
}
