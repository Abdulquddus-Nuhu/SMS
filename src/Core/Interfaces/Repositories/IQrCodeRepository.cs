using Core.Entities;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IQrCodeRepository
    {
        public Task<BaseResponse> AddQrCode(QrCode qrCode);
        public Task<IQueryable<QrCode>> GetAllAsync();
        public Task<List<StudentInSchoolResponse>> GetTodaysQrCodeAsync(string email);
        public Task<QrCode?> GetQrCodeById(Guid id);
        public Task<BaseResponse> EditQrCode(QrCode qrCode);
        public Task<List<StudentWithQrCodeResponse>> GetParentStudentsAsync(string email);
        public Task<BaseResponse> QrCodeExist(Guid studentId, string parentEmail);
        public Task<BaseResponse> AddQrCodes(List<QrCode> qrCodes);
    }
}
