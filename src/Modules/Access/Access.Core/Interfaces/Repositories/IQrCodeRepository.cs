using Access.Core.Entities;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Core.Interfaces.Repositories
{
    public interface IQrCodeRepository
    {
        public Task<BaseResponse> AddQrCode(QrCode qrCode);
        public Task<IQueryable<QrCode>> GetAllAsync();
    }
}
