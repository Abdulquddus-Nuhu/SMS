using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Models.Requests;
using Shared.Models.Requests;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Constants.StringConstants;

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
        
        public async Task<ApiResponse<List<StudentWithQrCodeResponse>>> GetParentStudentsAsync(string email)
        {
            var data = await _qrCodeRepository.GetParentStudentsAsync(email);

            return new ApiResponse<List<StudentWithQrCodeResponse>>()
            {
                Data = data
            };
        }
        
        public async Task<ApiResponse<GenerateQrCodeResponse>> CreateQrCodeAsync(GenerateQrCodeRequest request)
        {
            var response = new ApiResponse<GenerateQrCodeResponse>();

            var qrCodeExist = await _qrCodeRepository.QrCodeExist(request.StudentId, request.UserEmail);
            if (qrCodeExist.Status)
            {
                response.Status = qrCodeExist.Status;
                response.Message = qrCodeExist.Message;
                response.Code = ResponseCodes.Status400BadRequest;
                return response;
            }

            var newQrCode = new QrCode()
            {
                Id = Guid.NewGuid(),
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
                QrCodeId = newQrCode.Id,
                QrCodeData = $"mystar_{newQrCode.UserEmail}_{newQrCode.StudentId}_{newQrCode.Created}"
            };

            return response;
        }

        public async Task<BaseResponse> AuthorizeQrCode(AuthorizeQrCodeRequest request)
        {
            var response = new BaseResponse();

            var qrCode = await _qrCodeRepository.GetQrCodeById(request.QrCodeId);
            if (qrCode is null)
            {
                response.Status = false;
                response.Code = ResponseCodes.Status404NotFound;
                response.Message = "QrCode doesnt exist";
                return response;
            }

            // Update the QrCode properties
            qrCode.AuthorizedUser = request.AuthorizedUser;
            qrCode.AuthorizedUserFullName = request.AuthorizedUserFullName;
            qrCode.AuthorizedUserRelationship = request.AuthorizedUserRelationship;
            qrCode.AuthorizedUserPhoneNumber = request.AuthorizedUserPhoneNumber;

            var result = await _qrCodeRepository.EditQrCode(qrCode);
            if (!result.Status)
            {
                response.Status = false;
                response.Code = ResponseCodes.Status500InternalServerError;
                response.Message = result.Message;
                return response;
            }

            return response;
        }

    }
}
