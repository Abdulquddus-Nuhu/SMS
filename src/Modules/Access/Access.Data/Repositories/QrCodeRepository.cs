using Access.Core.Entities;
using Access.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Constants.StringConstants;

namespace Access.Data.Repositories
{
    internal class QrCodeRepository : IQrCodeRepository
    {
        private readonly AccessDbContext _dbContext;
        public QrCodeRepository(AccessDbContext dbContext)
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
