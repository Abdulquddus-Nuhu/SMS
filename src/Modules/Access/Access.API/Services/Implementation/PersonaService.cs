using Access.API.Models.Responses;
using Access.API.Services.Interfaces;
using Access.Data.Identity;
using Access.Models.Requests;
using Access.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Shared.Constants;
using Shared.Enums;
using Shared.Models.Responses;
using static Shared.Constants.StringConstants;
using System;
using Access.Data;
using Microsoft.EntityFrameworkCore;
using Access.API.Models.Requests;
using MediatR;

namespace Access.API.Services.Implementation
{
    public class PersonaService : IPersonaService
    {
        private readonly UserManager<Persona> _userManager;
        private readonly ILogger<PersonaService> _logger;
        private readonly HttpContext _httpContext;
        private readonly AccessDbContext _dbContext;
        private readonly IWebHostEnvironment _webHost;

        public PersonaService(UserManager<Persona> userManager,
            ILogger<PersonaService> logger,
            IHttpContextAccessor httpContextAccessor,
            AccessDbContext dbContext,
            IWebHostEnvironment webHost)
        {
            _userManager = userManager;
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext!;
            _dbContext = dbContext;
            _webHost = webHost;
        }


        private static string GetRoleToAdd(PersonaType type)
        {
            var role = type switch
            {
                PersonaType.Parent => AuthConstants.Roles.PARENT,
                PersonaType.Student => AuthConstants.Roles.STUDENT,
                PersonaType.Staff => AuthConstants.Roles.STAFF,
                PersonaType.BusDriver => AuthConstants.Roles.BUS_DRIVER,
                _ => default,
            };
            return role;
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

            _logger.LogInformation("Creating a new Parent");
            _logger.LogInformation("Trying to upload photo of new Parent");

            string photoUrl = UploadPhoto(request.Photo, "parent-images", "parent-");
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                response.Status = false;
                response.Message = "Unable to upload parent's photo. Please try again.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }
            _logger.LogInformation("Photo uploaded sucessfuly");


            var user = new Persona() { UserName = request.Email, Email = request.Email, PhoneNumber = request.PhoneNumber, FirstName = request.FirstName, LastName = request.LastName, EmailConfirmed = true, PhotoUrl = photoUrl, PesonaType = PersonaType.Parent };
            var creationResult = await _userManager.CreateAsync(user, request.Password);
            if (!creationResult.Succeeded)
            {
                response.Code = ResponseCodes.Status400BadRequest;
                response.Status = false;
                response.Message = string.Join(',', creationResult.Errors.Select(a => a.Description));
                _logger.LogInformation("Parent Creation is not successful with the following error {0}", response.Message);
                return response;
            }
            _logger.LogInformation("Parent Creation is successful");

            var roleResult = await _userManager.AddToRoleAsync(user, AuthConstants.Roles.PARENT);
            if (!roleResult.Succeeded)
            {
                _logger.LogInformation("Unable add user to Parent role.");
                response.Status = false;
                response.Message = "Unable add user to Parent role.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }

            response.Data = new ParentResponse() { PhotoUrl = user.PhotoUrl, Email = user.Email, FirstName = user.FirstName, ParentId = user.Id, LastName = user.LastName, PhoneNumber = user.PhoneNumber, UserName = user.UserName, Role = AuthConstants.Roles.PARENT };

            //if (roleResult.Succeeded)
            //{
            //    await _auditTrailService.AddAsync(createTrail(AuditActions.Create, null, user.ToJson(), $"Created new user: {user.Email}"));
            //}
            return response;
        }

        public async Task<BaseResponse> EditParentAsync(Guid parentId, EditParentRequest request, string editor)
        {
            var response = new BaseResponse();

            var parent = await _userManager.FindByIdAsync(parentId.ToString());
            if (parent is null)
            {
                response.Code = ResponseCodes.Status404NotFound;
                response.Status = false;
                response.Message = "Parent not found";
                return response;
            }

            parent.FirstName = request.FirstName ?? parent.FirstName;
            parent.LastName = request.LastName ?? parent.LastName;
            parent.Edit(editor);

            var updateResult = await _userManager.UpdateAsync(parent);
            if (!updateResult.Succeeded)
            {
                response.Code = ResponseCodes.Status500InternalServerError;
                response.Status = false;
                response.Message = string.Join(',', updateResult.Errors.Select(a => a.Description));
                return response;
            }

            return response;
        }



        public async Task<ApiResponse<StudentResponse>> CreateStudentAsync(CreateStudentRequest request, string host)
        {
            var response = new ApiResponse<StudentResponse>() { Code = ResponseCodes.Status201Created };

            var parent = await _userManager.FindByIdAsync(request.ParentId.ToString());
            if (parent is null)
            {
                response.Code = ResponseCodes.Status400BadRequest;
                response.Status = false;
                response.Message = "Invaild ParentId";
                return response;
            }

            _logger.LogInformation("Creating a new Student");
            _logger.LogInformation("Trying to upload photo of new Student");

            string photoUrl = UploadPhoto(request.Photo, "student-images", "student-");
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                response.Status = false;
                response.Message = "Unable to upload student's photo. Please try again.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }
            _logger.LogInformation("Photo uploaded sucessfuly");

            var user = new Persona() { FirstName = request.FirstName, LastName = request.LastName,Email = string.Concat(request.FirstName, "@", request.LastName, ".com"), UserName = string.Concat(request.FirstName, request.LastName), PhotoUrl = photoUrl, PesonaType = PersonaType.Student, ParentId = request.ParentId, BusServiceRequired = request.BusServiceRequired, GradeId = request.GradeId, EmailConfirmed = true };
            var creationResult = await _userManager.CreateAsync(user);
            if (!creationResult.Succeeded)
            {
                response.Code = ResponseCodes.Status400BadRequest;
                response.Status = false;
                response.Message = string.Join(',', creationResult.Errors.Select(a => a.Description));
                _logger.LogInformation("Student Creation is not successful with the following error {0}", response.Message);
                return response;
            }
            _logger.LogInformation("Student Creation is successful");

            var roleResult = await _userManager.AddToRoleAsync(user, AuthConstants.Roles.STUDENT);
            if (!roleResult.Succeeded)
            {
                _logger.LogInformation("Unable add user to Student role.");
                response.Status = false;
                response.Message = "Unable add user to Student role.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }

            //response.Data = new StudentResponse() {StudentId = user.Id, PhotoUrl = user.PhotoUrl, FirstName = user.FirstName, BusServiceRequired = request.BusServiceRequired, Grade = request.Grade, LastName = user.LastName, Role = AuthConstants.Roles.STUDENT };
            response.Data = new StudentResponse() {StudentId = user.Id, PhotoUrl = user.PhotoUrl, FirstName = user.FirstName, BusServiceRequired = request.BusServiceRequired, LastName = user.LastName, Role = AuthConstants.Roles.STUDENT };

            //if (roleResult.Succeeded)
            //{
            //    await _auditTrailService.AddAsync(createTrail(AuditActions.Create, null, user.ToJson(), $"Created new user: {user.Email}"));
            //}
            return response;
        }
       
        
        public async Task<ApiResponse<StudentResponse>> EditStudentAsync(Guid studentId, EditStudentRequest request, string editor)
        {
            var response = new ApiResponse<StudentResponse>();

            // Find the student to edit
            var student = await _userManager.FindByIdAsync(studentId.ToString());
            if (student is null)
            {
                response.Code = ResponseCodes.Status404NotFound;
                response.Status = false;
                response.Message = "Student not found";
                return response;
            }

            // Update student properties based on the request
            student.FirstName = request.FirstName ?? student.FirstName;
            student.LastName = request.LastName ??  student.LastName;
            student.GradeId = request.GradeId ?? student.GradeId;
            student.BusServiceRequired = request.BusServiceRequired;
            student.Edit(editor);

            // Check if a new photo is provided and update PhotoUrl accordingly
          

            // Update the student in the database
            var updateResult = await _userManager.UpdateAsync(student);
            if (!updateResult.Succeeded)
            {
                response.Code = ResponseCodes.Status500InternalServerError;
                response.Status = false;
                response.Message = string.Join(',', updateResult.Errors.Select(a => a.Description));
                return response;
            }
     
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

            _logger.LogInformation("Creating a new user");
            _logger.LogInformation("Trying to upload photo of new Bus driver");

            string photoUrl = UploadPhoto(request.Photo, "busdriver-images", "busdriver-");
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                _logger.LogInformation("Unable to upload bus driver photo. Please try again.");
                response.Status = false;
                response.Message = "Unable to upload bus driver photo. Please try again.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }
            _logger.LogInformation("Photo uploaded sucessfuly");


            var user = new Persona() { UserName = request.Email, Email = request.Email, PhoneNumber = request.PhoneNumber, FirstName = request.FirstName, LastName = request.LastName, EmailConfirmed = true, PhotoUrl = photoUrl, BusId = request.BusId, PesonaType = PersonaType.BusDriver };
            var creationResult = await _userManager.CreateAsync(user, request.Password);
            if (!creationResult.Succeeded)
            {
                response.Code = ResponseCodes.Status400BadRequest;
                response.Status = false;
                response.Message = string.Join(',', creationResult.Errors.Select(a => a.Description));
                _logger.LogInformation("Bus driver Creation is not successful with the following error {0}", response.Message);
                return response;
            }
            _logger.LogInformation("Bus driver Creation is successful");


            var roleResult = await _userManager.AddToRoleAsync(user, AuthConstants.Roles.BUS_DRIVER);
            if (!roleResult.Succeeded)
            {
                _logger.LogInformation("Unable add user to Bus driver role.");
                response.Message = "Unable add user to Bus driver role.";
                response.Code = ResponseCodes.Status500InternalServerError;
                response.Status = false;
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

            _logger.LogInformation("Creating a new staff");
            _logger.LogInformation("Trying to upload photo of new Staff");

            string photoUrl = UploadPhoto(request.Photo, "staff-images", "staff-");
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                _logger.LogInformation("Unable to upload staff photo. Please try again.");
                response.Status = false;
                response.Message = "Unable to upload staff photo. Please try again.";
                response.Code = ResponseCodes.Status500InternalServerError;
                return response;
            }
            _logger.LogInformation("Photo uploaded sucessfuly");


            var user = new Persona() { UserName = request.Email, Email = request.Email, PhoneNumber = request.PhoneNumber, FirstName = request.FirstName, LastName = request.LastName, EmailConfirmed = true, PhotoUrl = photoUrl, PesonaType = PersonaType.Staff, DepartmentId = request.DepartmentId, JobTitleId = request.JobTitleId };
            var creationResult = await _userManager.CreateAsync(user, request.Password);
            if (!creationResult.Succeeded)
            {
                response.Code = ResponseCodes.Status400BadRequest;
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
                response.Status = false;
                return response;
            }

            return response;
        }


        public async Task<ApiResponse<List<ParentResponse>>> ParentListAsync()
        {
            var response = new ApiResponse<List<ParentResponse>>();

            var parents = await _dbContext.Users
                .Where(x => x.PesonaType == PersonaType.Parent)
                .Select(x => new ParentResponse()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ParentId = x.Id,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    PhotoUrl = x.PhotoUrl,
                })
                .AsNoTracking()
                .ToListAsync();

            foreach (var parent in parents)
            {
                parent.NumberOfStudent = _dbContext.Users.Where(x => x.ParentId == parent.ParentId && x.IsDeleted == false).Count();

            }

            response.Data = parents;
            return response;
        }

        public async Task<ApiResponse<List<StudentResponse>>> StudentListAsync()
        {
            var response = new ApiResponse<List<StudentResponse>>();

            var students = _dbContext.Users
                .Include(x => x.Grade)
                .Where(x => x.PesonaType == PersonaType.Student)
                .Select(x => new StudentResponse()
                {
                    StudentId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Grade = x.Grade.Name,
                    BusServiceRequired = x.BusServiceRequired.Value,
                    PhotoUrl = x.PhotoUrl,
                })
                .AsNoTracking();

            response.Data = await students.ToListAsync();
            return response;
        }

        public async Task<ApiResponse<List<StudentResponse>>> ParentStudentsListAsync(Guid parentId)
        {
            var response = new ApiResponse<List<StudentResponse>>();

            var students = _dbContext.Users
                .Include(x => x.Grade)
                .Where(x => x.PesonaType == PersonaType.Student && x.ParentId == parentId)
                .Select(x => new StudentResponse()
                {
                    StudentId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Grade = x.Grade!.Name,
                    BusServiceRequired = x.BusServiceRequired.Value,
                    PhotoUrl = x.PhotoUrl,
                })
                .AsNoTracking();

            response.Data = await students.ToListAsync();
            return response;
        }
        public async Task<ApiResponse<List<StaffResponse>>> StaffListAsync()
        {
            var response = new ApiResponse<List<StaffResponse>>();

            var staff = _dbContext.Users
                .Where(x => x.PesonaType == PersonaType.Staff)
                .Select(x => new StaffResponse()
                {
                    StaffId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhotoUrl = x.PhotoUrl,
                })
                .AsNoTracking();

            response.Data = await staff.ToListAsync();
            return response;
        }
        public async Task<ApiResponse<List<BusDriverResponse>>> BusDriverListAsync()
        {
            var response = new ApiResponse<List<BusDriverResponse>>();

            var busdrivers = _dbContext.Users
                .Include(x => x.Bus)
                .Where(x => x.PesonaType == PersonaType.BusDriver)
                .Select(x => new BusDriverResponse()
                {
                    BusDriverId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhotoUrl = x.PhotoUrl,
                    BusNumber = x.Bus.Number,

                })
                .AsNoTracking();

            response.Data = await busdrivers.ToListAsync();
            return response;
        }

        public async Task<BaseResponse> DeleteParentAsync(Guid parentId, string deletor)
        {
            _logger.LogInformation("Trying to delete a user with id: {0}", parentId);
            var response = new BaseResponse();

            var parent = await _userManager.FindByIdAsync(parentId.ToString());
            if (parent is null)
            {
                _logger.LogInformation("Parent with id: {0} not found", parentId);
                response.Code = ResponseCodes.Status404NotFound;
                response.Message = "Parent not found";
                response.Status = false;
                return response;
            }

            parent.Delete(deletor);

            if (!(await _userManager.UpdateAsync(parent)).Succeeded)
            {
                response.Code = ResponseCodes.Status500InternalServerError;
                response.Status = false;
                response.Message = "Unable to delete parent! Please try again";
                return response;
            }

            return response;
        }

        public async Task<ApiResponse<ParentResponse>> GetParentAsync(Guid parentId)
        {
            _logger.LogInformation("Trying to get a parent with id: {0}", parentId);
            var response = new ApiResponse<ParentResponse>() { Code = ResponseCodes.Status200OK };

            var parent = await _userManager.FindByIdAsync(parentId.ToString());
            if (parent is null)
            {
                _logger.LogInformation("Parent with id: {0} not found", parentId);
                response.Code = ResponseCodes.Status404NotFound;
                response.Message = "Parent not found";
                response.Status = false;
                return response;
            }

            response.Data = new ParentResponse()
            {
                Email = parent.Email ?? string.Empty,
                ParentId = parent.Id,
                PhoneNumber = parent.PhoneNumber ?? string.Empty,
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                PhotoUrl = parent.PhotoUrl ?? string.Empty,
            };

            return response;
        }


        public async Task<BaseResponse> DeleteStudnetAsync(Guid studentId, string deletor)
        {
            _logger.LogInformation("Trying to delete a user with id: {0}", studentId);
            var response = new BaseResponse();

            var student = await _userManager.FindByIdAsync(studentId.ToString());
            if (student is null)
            {
                _logger.LogInformation("Parent with id: {0} not found", studentId);
                response.Code = ResponseCodes.Status404NotFound;
                response.Message = "Parent not found";
                response.Status = false;
                return response;
            }

            student.Delete(deletor);

            if (!(await _userManager.UpdateAsync(student)).Succeeded)
            {
                response.Code = ResponseCodes.Status500InternalServerError;
                response.Status = false;
                response.Message = "Unable to delete student! Please try again";
                return response;
            }

            return response;
        }

        public async Task<ApiResponse<StudentResponse>> GetStudentAsync(Guid studentId)
        {
            _logger.LogInformation("Trying to get a student with id: {0}", studentId);
            var response = new ApiResponse<StudentResponse>() { Code = ResponseCodes.Status200OK };

            //var student = await _userManager.FindByIdAsync(studentId.ToString());
            var student = await _dbContext.Users.Include(x => x.Grade).FirstOrDefaultAsync(x => x.Id == studentId);
            if (student is null)
            {
                _logger.LogInformation("Student with id: {0} not found", studentId);
                response.Code = ResponseCodes.Status404NotFound;
                response.Message = "Student not found";
                response.Status = false;
                return response;
            }

            response.Data = new StudentResponse()
            {
                StudentId = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                PhotoUrl = student.PhotoUrl ?? string.Empty,
                Grade = student.Grade!.Name
            };

            return response;
        }


        private string UploadPhoto(IFormFile? photo, string folder, string fileNameAlias)
        {
            string folderPath = "static";

            string path = Path.Combine(_webHost.ContentRootPath, folderPath, folder);
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
                var newFileName = String.Concat(fileNameAlias, myUniqueFileName, fileExtension);

                // Combines two strings into a path.
                string filepath = string.Empty;
                try
                {
                    filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), folderPath, folder)).Root + $"{newFileName}";
                    using (FileStream fs = File.Create(filepath))
                    {
                        photo?.CopyTo(fs);
                        fs.Flush();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Error while uploading photo");
                    _logger.LogError(ex.Message);
                }

                string prefixToRemove = Path.Combine(Directory.GetCurrentDirectory());
                if (filepath.StartsWith(prefixToRemove))
                {
                    filepath = filepath.Substring(prefixToRemove.Length);
                    Console.WriteLine(filepath);
                }

                return filepath;
            }

            return string.Empty;
        }

    }

}
