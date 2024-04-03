using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Models.Requests;
using Models.Responses;
using Shared.Models.Requests;
using Shared.Models.Responses;
using static Shared.Constants.StringConstants;

namespace Core.Services
{
    public class QrCodeService : IQrCodeService
    {
        private readonly IQrCodeRepository _qrCodeRepository;
        private readonly ITripRepository _tripRepository;
        private readonly ITripService _tripService;
        private readonly ILogger<QrCodeService> _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly IParentRepository _parentRepository;
        public QrCodeService(IQrCodeRepository qrCodeRepository, ITripRepository tripRepository, ITripService tripService,
            ILogger<QrCodeService> logger, IStudentRepository studentRepository, IParentRepository parentRepository)
        {
            _qrCodeRepository = qrCodeRepository;
            _tripRepository = tripRepository;
            _tripService = tripService;
            _logger = logger;
            _studentRepository = studentRepository;
            _parentRepository = parentRepository;
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
                QrCodeData = $"mystar:{newQrCode.Id}"
                //QrCodeData = $"mystar_{newQrCode.UserEmail}_{newQrCode.StudentId}_{newQrCode.Created}"
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


        public async Task<ApiResponse<List<GenerateQrCodeResponse>>> GenerateQrCodesForTripAsync(Guid tripId, string busDriverEmail)
        {
            //TOdo: check if busDriver has already generate qrcode for the day before proceding
            var response = new ApiResponse<List<GenerateQrCodeResponse>>();

            List<GenerateQrCodeResponse> qrCodesResponse = new();

            List<StudentResponse> onboardedStudents = (await _tripService.GetOnboardedStudentAsync(tripId, busDriverEmail)).Data.ToList();

            List<QrCode> newQrCodes = new();

            //FOREACH LOOP TO ADD QRCODE IN DB
            //foreach (var student in onboardedStudents)
            //{
            //    var newQrCode = new QrCode()
            //    {
            //        Id = Guid.NewGuid(),
            //        StudentId = student.StudentId,
            //        UserEmail = busDriverEmail,
            //        Created = DateTime.UtcNow,
            //    };

            //    //todo: roundtrip to database here
            //    //await _qrCodeRepository.AddQrCode(newQrCode);

            //    var result = await _qrCodeRepository.AddQrCode(newQrCode);
            //    if (!result.Status)
            //    {
            //        _logger.LogInformation($"Student {student.StudentId} qrcode wasn't generated on trip {tripId}. Code {result.Code}. Message {result.Message}");
            //        continue;
            //    }

            //    qrCodesResponse.Add(new()
            //    {
            //        QrCodeId = newQrCode.Id,
            //        QrCodeData = $"mystar_{newQrCode.UserEmail}_{newQrCode.StudentId}_{newQrCode.Created}"
            //    });

            //}


            //Create qrcode for all students
            foreach (var student in onboardedStudents)
            {
                var newQrCode = new QrCode()
                {
                    Id = Guid.NewGuid(),
                    StudentId = student.StudentId,
                    UserEmail = busDriverEmail,
                    Created = DateTime.UtcNow,
                };

                newQrCodes.Add(newQrCode);
            }

            //Save all created qrcodes to database
            var result = await _qrCodeRepository.AddQrCodes(newQrCodes);
            if (!result.Status)
            {
                _logger.LogInformation("{0}", result.Message);
            }

            //Transform all created qrcodes to response
            foreach (var newQrCode in newQrCodes)
            {
                qrCodesResponse.Add(new()
                {
                    QrCodeId = newQrCode.Id,
                    QrCodeData = $"mystar:{newQrCode.Id}"
                    //QrCodeData = $"mystar_{newQrCode.UserEmail}_{newQrCode.StudentId}_{newQrCode.Created}"
                });
            }

            response.Data = qrCodesResponse;
            return response;
        }

        public async Task<ApiResponse<ScanQrCodeResponse>> ScanQrCodeAsync(string qrCodeData, string user)
        {
            var response = new ApiResponse<ScanQrCodeResponse>();

            string[] parts = qrCodeData.Split(':');

            if (!(parts.Length == 2 && Guid.TryParse(parts[1], out Guid qrCodeId)))
            {
                response.Status = false;
                response.Code = ResponseCodes.Status400BadRequest;
                response.Message = "Invalid QrCode data";
                return response;
            }

            var qrCode = await _qrCodeRepository.GetQrCodeById(qrCodeId);
            if (qrCode is null)
            {
                response.Status = false;
                response.Code = ResponseCodes.Status404NotFound;
                response.Message = "QrCode doesn't exist";
                return response;
            }

            if (qrCode.ScannedBy != null)
            {
                response.Status = false;
                response.Code = ResponseCodes.Status400BadRequest;
                response.Message = "QrCode has already been scanned";
                return response;
            }

            qrCode.ScannedBy = user;
            qrCode.ScannedTime = DateTime.UtcNow;

            var result = await _qrCodeRepository.EditQrCode(qrCode);
            if (!result.Status)
            {
                response.Status = false;
                response.Code = ResponseCodes.Status500InternalServerError;
                response.Message = result.Message;
                return response;
            }

            var student = await _studentRepository.GetByIdAsync(qrCode.StudentId);
            if (student is null)
            {
                //do something
            }

            response.Data = new ScanQrCodeResponse
            {
                Grade = student.Grade.Name,
                Photo = student.PhotoUrl,
                FullName = student.FullName,
                InTimer = qrCode.PickUpTime,
                OutTimer = qrCode.DropOffTime,
                IsAuthorizedUserParent = qrCode.AuthorizedUser == Shared.Enums.AuthorizedUserType.Self ? true : false,
            };

            if (!response.Data.IsAuthorizedUserParent)
            {
                response.Data.AuthorizedUser = qrCode.AuthorizedUserFullName;
            }
            else
            {
                var parent = await _parentRepository.GetByEmailAsync(qrCode.UserEmail ?? string.Empty);
                if (parent is null)
                {
                    //do something
                }

                response.Data.AuthorizedUser = parent.FullName;
            }

            return response;

        }
    }
}
