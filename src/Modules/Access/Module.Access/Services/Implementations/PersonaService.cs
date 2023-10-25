using System;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Module.Access.Identity;
using Module.Access.Models.Request;
using Module.Access.Models.Response;
using Module.Access.Services.Interfaces;
using Shared.Constants;
using static Shared.Constants.StringConstants;

namespace Module.Access.Services.Implementations
{
    public class PersonaService : IPersonaService
    {
        private readonly UserManager<Persona> _userManager;
        private readonly ILogger<PersonaService> _logger;
        private readonly HttpContext _httpContext;
        //private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHost;

        public PersonaService(UserManager<Persona> userManager,
            ILogger<PersonaService> logger,
            IHttpContextAccessor httpContextAccessor,
            //AppDbContext dbContext,
            IWebHostEnvironment webHost)
        {
            _userManager = userManager;
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext!;
            //_dbContext = dbContext;
            _webHost = webHost;
        }


        public async Task<ApiResponse<ParentResponse>> CreateParentAsync(CreateParentRequest request, string host)
        {
            var response = new ApiResponse<ParentResponse>() { Code = ResponseCodes.Status201Created };
            if (!string.Equals(request.Password, request.ConfirmPassword))
            {
                response.Status = false;
                response.Code = ResponseCodes.Status400BadRequest;
                response.Message = "Password Field not the same as that of ConfirmPassword field";
                return response;
            }

            string UploadParentPhoto(IFormFile photo)
            {
                string path = Path.Combine(_webHost.ContentRootPath, "static", "parent-images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (photo != null && photo.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(photo.FileName);
                    //Assigning Unique Filename (Guid)
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat("parent-", myUniqueFileName, fileExtension);

                    // Combines two strings into a path.
                    string filepath = string.Empty;
                    try
                    {
                        filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "static", "parent-images")).Root + $"{newFileName}";
                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            request.Photo.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation("Error while uploading photo");
                        _logger.LogError(ex.Message);
                    }

                    return filepath;
                }

                return string.Empty;
            }

            _logger.LogInformation("Creating a new user");
            var photoUrl = UploadParentPhoto(request.Photo);
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                response.Status = false;
                response.Message = "Unable to upload parent's photo. Please try again.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }


            //var user = new Persona() { UserName = request.Email, Email = request.Email, PhoneNumber = request.PhoneNumber, FirstName = request.FirstName, LastName = request.LastName, EmailConfirmed = true, PhotoUrl = string.Concat(host, photoUrl) };
            var user = new Persona() { UserName = request.Email, Email = request.Email, PhoneNumber = request.PhoneNumber, FirstName = request.FirstName, LastName = request.LastName, EmailConfirmed = true, PhotoUrl = photoUrl };
            var creationResult = await _userManager.CreateAsync(user, request.Password);
            if (!creationResult.Succeeded)
            {
                response.Status = false;
                response.Message = string.Join(',', creationResult.Errors.Select(a => a.Description));
                _logger.LogInformation("Parent Creation is not successful with the following error {0}", response.Message);
                return response;
            }
            _logger.LogInformation("Parent Creation is successful");

            var roleResult = await _userManager.AddToRoleAsync(user, AuthConstants.Roles.PARENT);
            if (!roleResult.Succeeded)
            {
                response.Message = "Unable add user to Parent role.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }

            response.Data = new ParentResponse() { PhotoUrl = user.PhotoUrl, Email = user.Email, FirstName = user.FirstName, ParentId = user.Id.ToString(), LastName = user.LastName, PhoneNumber = user.PhoneNumber, UserName = user.UserName, Role = AuthConstants.Roles.PARENT };

            //if (roleResult.Succeeded)
            //{
            //    await _auditTrailService.AddAsync(createTrail(AuditActions.Create, null, user.ToJson(), $"Created new user: {user.Email}"));
            //}
            return response;
        }


        public async Task<ApiResponse<StudentResponse>> CreateStudentAsync(CreateStudentRequest request, string host)
        {
            var response = new ApiResponse<StudentResponse>() { Code = ResponseCodes.Status201Created };

            var isSuccess = Guid.TryParse(request.ParentId, out Guid parentId);
            if (!isSuccess)
            {
                response.Code = ResponseCodes.Status400BadRequest;
                response.Status = false;
                response.Message = "Invaild ParentId";
                return response;
            }

            string UploadStudentPhoto(IFormFile photo)
            {
                string path = Path.Combine(_webHost.ContentRootPath, "static", "student-images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (photo != null && photo.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(photo.FileName);
                    //Assigning Unique Filename (Guid)
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat("student-", myUniqueFileName, fileExtension);

                    // Combines two strings into a path.
                    string filepath = string.Empty;
                    try
                    {
                        filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "static", "student-images")).Root + $"{newFileName}";
                        using (FileStream fs = File.Create(filepath))
                        {
                            request.Photo.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation("Error while uploading photo");
                        _logger.LogError(ex.Message);
                    }

                    return filepath;
                }

                return string.Empty;
            }

            _logger.LogInformation("Creating a new Student");
            var photoUrl = UploadStudentPhoto(request.Photo);
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                response.Status = false;
                response.Message = "Unable to upload student's photo. Please try again.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }

            var user = new Persona() { FirstName = request.FirstName, LastName = request.LastName, EmailConfirmed = true, PhotoUrl = photoUrl, PesonaType = 2, ParentId = parentId, BusServiceRequired = request.BusServiceRequired, Grade = request.Grade };
            var creationResult = await _userManager.CreateAsync(user);
            if (!creationResult.Succeeded)
            {
                response.Status = false;
                response.Message = string.Join(',', creationResult.Errors.Select(a => a.Description));
                _logger.LogInformation("Student Creation is not successful with the following error {0}", response.Message);
                return response;
            }
            _logger.LogInformation("Student Creation is successful");

            var roleResult = await _userManager.AddToRoleAsync(user, AuthConstants.Roles.STUDENT);
            if (!roleResult.Succeeded)
            {
                response.Message = "Unable add user to Student role.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }

            response.Data = new StudentResponse() { PhotoUrl = user.PhotoUrl, FirstName = user.FirstName, BusServiceRequired = request.BusServiceRequired, Grade = request.Grade, LastName = user.LastName, Role = AuthConstants.Roles.STUDENT };

            //if (roleResult.Succeeded)
            //{
            //    await _auditTrailService.AddAsync(createTrail(AuditActions.Create, null, user.ToJson(), $"Created new user: {user.Email}"));
            //}
            return response;
        }

        public async Task<BaseResponse> CreateBusDriverAsync(CreateBusDriverRequest request, string host)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status201Created };
            if (!string.Equals(request.Password, request.ConfirmPassword))
            {
                _logger.LogInformation("Password Field not the same as that of ConfirmPassword field");
                response.Status = false;
                response.Code = ResponseCodes.Status400BadRequest;
                response.Message = "Password Field not the same as that of ConfirmPassword field";
                return response;
            }

            string UploadBusDriverPhoto(IFormFile photo)
            {
                string path = Path.Combine(_webHost.ContentRootPath, "static", "busdriver-images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (photo != null && photo.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(photo.FileName);
                    //Assigning Unique Filename (Guid)
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat("busdriver-", myUniqueFileName, fileExtension);

                    // Combines two strings into a path.
                    string filepath = string.Empty;
                    try
                    {
                        filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "static", "busdriver-images")).Root + $"{newFileName}";
                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            request.Photo.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation("Error while uploading photo");
                        _logger.LogError(ex.Message);
                    }

                    return filepath;
                }

                return string.Empty;
            }

            _logger.LogInformation("Creating a new user");

            var photoUrl = UploadBusDriverPhoto(request.Photo);
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                _logger.LogInformation("Unable to upload bus driver photo. Please try again.");
                response.Status = false;
                response.Message = "Unable to upload bus driver photo. Please try again.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }

            var user = new Persona() { UserName = request.Email, Email = request.Email, PhoneNumber = request.PhoneNumber, FirstName = request.FirstName, LastName = request.LastName, EmailConfirmed = true, PhotoUrl = photoUrl, BusNumber = request.BusNumber, PesonaType = 3 };
            var creationResult = await _userManager.CreateAsync(user, request.Password);
            if (!creationResult.Succeeded)
            {
                response.Status = false;
                response.Message = string.Join(',', creationResult.Errors.Select(a => a.Description));
                _logger.LogInformation("Bus driver Creation is not successful with the following error {0}", response.Message);
                return response;
            }
            _logger.LogInformation("Bus driver Creation is successful");

            var roleResult = await _userManager.AddToRoleAsync(user, AuthConstants.Roles.BUS_DRIVER);
            if (!roleResult.Succeeded)
            {
                response.Message = "Unable add user to Bus driver role.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }

            return response;
        }

        public async Task<BaseResponse> CreateStaffAsync(CreateStaffRequest request, string host)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status201Created };
            if (!string.Equals(request.Password, request.ConfirmPassword))
            {
                _logger.LogInformation("Password Field not the same as that of ConfirmPassword field");
                response.Status = false;
                response.Code = ResponseCodes.Status400BadRequest;
                response.Message = "Password Field not the same as that of ConfirmPassword field";
                return response;
            }

            string UploadStaffPhoto(IFormFile photo)
            {
                string path = Path.Combine(_webHost.ContentRootPath, "static", "staff-images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (photo != null && photo.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(photo.FileName);
                    //Assigning Unique Filename (Guid)
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat("staff-", myUniqueFileName, fileExtension);

                    // Combines two strings into a path.
                    string filepath = string.Empty;
                    try
                    {
                        filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "static", "staff-images")).Root + $"{newFileName}";
                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            request.Photo.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation("Error while uploading photo");
                        _logger.LogError(ex.Message);
                    }

                    return filepath;
                }

                return string.Empty;
            }

            _logger.LogInformation("Creating a new staff");

            var photoUrl = UploadStaffPhoto(request.Photo);
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                _logger.LogInformation("Unable to upload staff photo. Please try again.");
                response.Status = false;
                response.Message = "Unable to upload staff photo. Please try again.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }

            var user = new Persona() { UserName = request.Email, Email = request.Email, PhoneNumber = request.PhoneNumber, FirstName = request.FirstName, LastName = request.LastName, EmailConfirmed = true, PhotoUrl = photoUrl, PesonaType = 4, Department = request.Department, JobTitle = request.JobTitle };
            var creationResult = await _userManager.CreateAsync(user, request.Password);
            if (!creationResult.Succeeded)
            {
                response.Status = false;
                response.Message = string.Join(',', creationResult.Errors.Select(a => a.Description));
                _logger.LogInformation("Staff Creation is not successful with the following error {0}", response.Message);
                return response;
            }
            _logger.LogInformation("Staff Creation is successful");

            var roleResult = await _userManager.AddToRoleAsync(user, AuthConstants.Roles.STAFF);
            if (!roleResult.Succeeded)
            {
                _logger.LogInformation("Unable add user to Staff role.");
                response.Message = "Unable add user to Staff role.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }

            return response;
        }

    }

}

