using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;
using static Shared.Constants.StringConstants;

namespace Infrastructure.Repositories
{
    public class QrCodeRepository : IQrCodeRepository
    {
        private readonly AppDbContext _dbContext;
        public QrCodeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task<IQueryable<QrCode>> GetAllAsync()
        {
            return _dbContext.QrCodes.AsQueryable().AsNoTracking();
        }
        
        public async Task<IQueryable<QrCode>> GetTodaysQrCodeAsync()
        {
            return _dbContext.QrCodes.Include(x => x.Student).AsQueryable().AsNoTracking();
        }
    }
}
