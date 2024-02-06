using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Models.Requests;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class QrCodeService : IQrCodeService
    {
        private readonly IQrCodeRepository _qrCodeRepository;
        public QrCodeService(IQrCodeRepository qrCodeRepository)
        {
                _qrCodeRepository = qrCodeRepository;
        }

        public async Task<ApiResponse<List<StudentInSchoolResponse>>> GetTodaysQrCodeAsync(string email)
        {
            var data = await _qrCodeRepository.GetTodaysQrCodeAsync(email);

            return new ApiResponse<List<StudentInSchoolResponse>>()
            {
                Data = data
            };
        }
        
        public async Task<ApiResponse<GenerateQrCodeResponse>> CreateQrCodeAsync(GenerateQrCodeRequest request)
        {
            var response = new ApiResponse<GenerateQrCodeResponse>();

            var newQrCode = new QrCode()
            {
                AuthorizedUser = request.AuthorizedUser,
                AuthorizedUserFirstName = request.AuthorizedUserFirstName,
                AuthorizedUserLastName = request.AuthorizedUserLastName,
                AuthorizedUserRelationship = request.AuthorizedUserRelationship,
                StudentId = request.StudentId,
                UserEmail = request.UserEmail,
                Created = DateTime.UtcNow,
            };

            var result = await _qrCodeRepository.AddQrCode(newQrCode);
            if (!result.Status)
            {
                response.Status = result.Status;
                response.Message = result.Message;
                response.Code = result.Code;
                return response;
            }


            response.Data = new GenerateQrCodeResponse()
            {
                QrCodeData = $"mystar_{newQrCode.UserEmail}_{newQrCode.StudentId}_{newQrCode.Created}"
            };

            return response;
        }

    }
}
