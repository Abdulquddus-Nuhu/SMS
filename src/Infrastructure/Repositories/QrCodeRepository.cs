using Core.Entities;
using Core.Entities.Users;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Responses;
using Shared.Models.Responses;
using static Shared.Constants.StringConstants;

namespace Infrastructure.Repositories
{
    public class QrCodeRepository : IQrCodeRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<QrCodeRepository> _logger;
        public QrCodeRepository(AppDbContext dbContext, ILogger<QrCodeRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<BaseResponse> AddQrCode(QrCode qrCode)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status201Created };

            _dbContext.QrCodes.Add(qrCode);

            var result = await _dbContext.TrySaveChangesAsync();
            if (result)
            {
                return response;
            }

            response.Message = "Unable to generate qrcode! Please try again";
            response.Status = false;
            response.Code = ResponseCodes.Status500InternalServerError;

            return response;

        }
        public async Task<BaseResponse> AddQrCodes(List<QrCode> qrCodes)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status201Created };

            await _dbContext.QrCodes.AddRangeAsync(qrCodes);

            var result = await _dbContext.TrySaveChangesAsync();
            if (result)
            {
                return response;
            }

            response.Message = "Unable to generate qrcodes! Please try again";
            response.Status = false;
            response.Code = ResponseCodes.Status500InternalServerError;

            return response;
        }

        public async Task<IQueryable<QrCode>> GetAllAsync()
        {
            return _dbContext.QrCodes.AsQueryable().AsNoTracking();
        }
        
        public async Task<List<StudentInSchoolResponse>> GetTodaysQrCodeAsync(string email)
        {
            var parent = await _dbContext.Parents.FirstOrDefaultAsync(x => x.Email == email);
            if (parent is null)
            {
                _logger.LogInformation("User with email {0} not found in parents table", email);
            }

            var studentIds = await _dbContext.ParentStudent
                .Where(x => x.ParentsId == parent.Id)
                .Select(x => x.StudentsId)
                .ToListAsync();

            var students = await _dbContext.Students
                .Include(x => x.Grade)
                .Where(x => studentIds.Contains(x.Id))
                .Select(x => new StudentInSchoolResponse()
                {
                    StudentId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhotoUrl = x.PhotoUrl,
                    Grade = x.Grade.Name,
                })
                .ToListAsync();

            foreach ( var student in students )
            {
                // Get the current date without the time component
                DateTime currentDate = DateTime.Today.ToUniversalTime();
                // Get the QR code for the student that matches the current date
                var qrCode = await _dbContext.QrCodes.Where(x => x.StudentId == student.StudentId && x.Created.Date.ToUniversalTime() == currentDate).FirstOrDefaultAsync();
                if (qrCode != null)
                {
                    student.IsInSchool = true;
                }
                else
                {
                    student.IsInSchool = false;
                }
            }

            return students;
        }


        public async Task<List<StudentWithQrCodeResponse>> GetParentStudentsAsync(string email)
        {
            var parent = await _dbContext.Parents.FirstOrDefaultAsync(x => x.Email == email);
            if (parent is null)
            {
                _logger.LogInformation("User with email {0} not found in parents table", email);
                return new List<StudentWithQrCodeResponse>();
            }

            var studentIds = await _dbContext.ParentStudent
                .Where(x => x.ParentsId == parent.Id)
                .Select(x => x.StudentsId)
                .ToListAsync();

            var students = await _dbContext.Students
                .Include(x => x.Grade)
                .Where(x => studentIds.Contains(x.Id))
                .Select(x => new StudentWithQrCodeResponse()
                {
                    StudentId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhotoUrl = x.PhotoUrl,
                    Grade = x.Grade.Name,
                })
                .ToListAsync();

            foreach (var student in students)
            {
                // Get the current date without the time component
                DateTime currentDate = DateTime.Today.ToUniversalTime();
                // Get the QR code for the student that matches the current date
                var qrCode = await _dbContext.QrCodes.FirstOrDefaultAsync(x => x.StudentId == student.StudentId && x.Created.Date.ToUniversalTime() == currentDate);
                if (qrCode != null)
                {
                    student.HasQrCode = true;
                }
                else
                {
                    student.HasQrCode = false;
                }
            }

            return students;
        }

        public async Task<BaseResponse> EditQrCode(QrCode qrCode)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status200OK };

            // Save the changes
            var result = await _dbContext.TrySaveChangesAsync();
            if (result)
            {
                return response;
            }

            response.Message = "Unable to update/edit qrcode! Please try again";
            response.Status = false;
            response.Code = ResponseCodes.Status500InternalServerError;

            return response;
        }

        public async Task<QrCode?> GetQrCodeById(Guid id)
        {
            return await _dbContext.QrCodes.FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task<BaseResponse> QrCodeExist(Guid studentId, string parentEmail)
        {
            var response = new BaseResponse();

            var qrCode = await _dbContext.QrCodes.FirstOrDefaultAsync(x => x.StudentId == studentId && x.UserEmail == parentEmail && x.Created.Date.ToUniversalTime() == DateTime.Today.ToUniversalTime());
            if (qrCode != null)
            {
                response.Code = ResponseCodes.Status200OK;
                response.Status = true;
                response.Message = "Qr Code record exist";
                return response;
            }
            else
            {
                response.Code = ResponseCodes.Status404NotFound;
                response.Status = false;
                response.Message = "Qr Code record doesn't exist";
                return response;
            }
        }
    }
}
