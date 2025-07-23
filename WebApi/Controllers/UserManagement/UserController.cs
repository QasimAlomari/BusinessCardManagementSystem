using Domain.AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using static Domain.Common.CommonClass;
using WebApi.Authorization;
using Domain.ViewModel.ApplicationUserViewModel;
using Services.UnitOfWork;
using Domain.ViewModel.BusinessCardViewModel;
using Domain.ViewModel.CommonViewModel;
using Services.UnitWork;
using static Dapper.SqlMapper;

namespace WebApi.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IAuthentication<ApplicationUserModel> _jwt;

        public UserController(IUnitOfWorkFactory unitOfWorkFactory,
            IAuthentication<ApplicationUserModel> jwt)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _jwt = jwt;
        }

        [HttpPost(nameof(Login))]
        public async Task<ResponseStandardJsonApi> Login(ApplicationUserModelLogin userLogin)
        {
            var apiResponse = new ResponseStandardJsonApi();
            try
            {
                var mapper = AutoMapperConfiguration.CreateMapper();
                var data = mapper.Map<ApplicationUser>(userLogin);
                var unitOfWork = _unitOfWorkFactory.Create();
                var user = await unitOfWork.ApplicationUser.GetUserNamePasswordAsync(data);
                if (user != null)
                {
                    var userModel = mapper.Map<ApplicationUserModel>(user);
                    userModel.ApplicationUserToken = _jwt.GetJsonWebToken(userModel);

                    apiResponse.Message = "Login Successfully";
                    apiResponse.Code = Ok().StatusCode;
                    apiResponse.Success = true;
                    apiResponse.Result = userModel;
                }
                else
                {
                    apiResponse.Message = "Login Failed";
                    apiResponse.Code = Ok().StatusCode;
                    apiResponse.Success = false;
                    apiResponse.Result = new NullColumns[] { };
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                apiResponse.Code = BadRequest().StatusCode;
                apiResponse.Result = new NullColumns[] { };
            }
            return apiResponse;
        }

        [HttpPost(nameof(Register))]
        public async Task<ResponseStandardJsonApi> Register(ApplicationUserModelRegister userRegister)
        {
            var apiResponse = new ResponseStandardJsonApi();
            try
            {
                if (ModelState.IsValid)
                {
                    var email = userRegister.Email?.Trim();
                    var role = (int)userRegister.UserRole;
                    var mapper = AutoMapperConfiguration.CreateMapper();
                    var data = mapper.Map<ApplicationUser>(userRegister);
                    var unitOfWork = _unitOfWorkFactory.Create();
                    if (await unitOfWork.ApplicationUser.IsUserExistsByEmailAsync<bool>(data))
                    {
                        apiResponse.Success = false;
                        apiResponse.Message = "Email already exists.";
                        apiResponse.Code = BadRequest().StatusCode;
                        apiResponse.Result = new NullColumns[] { };
                    }
                    else
                    {
                        await unitOfWork.ApplicationUser.Add(data);
                        apiResponse.Message = "Register Successfully";
                        apiResponse.Code = Ok().StatusCode;
                        apiResponse.Success = true;
                        apiResponse.Result = new NullColumns[] { };
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                apiResponse.Code = BadRequest().StatusCode;
                apiResponse.Result = new NullColumns[] { };
            }
            return apiResponse;
        }

        [HttpPost(nameof(GetListForDataTable))]
        public async Task<ResponseStandardJsonApi> GetListForDataTable([FromBody] DataTablesRequest request)
        {
            var apiResponse = new ResponseStandardJsonApi();

            try
            {
                int pageNumber = (request.Start / request.Length) + 1;
                int pageSize = request.Length > 0 ? request.Length : 10;
                string searchValue = request.Search?.Value?.Trim() ?? "";
                string sortColumn = "ApplicationUserUsername";
                string sortDirection = "ASC";

                if (request.Order != null && request.Order.Count > 0)
                {
                    var order = request.Order[0];
                    int sortColIndex = order.Column;
                    sortDirection = order.Dir?.ToUpper() == "DESC" ? "DESC" : "ASC";

                    if (request.Columns != null && request.Columns.Count > sortColIndex)
                    {
                        sortColumn = request.Columns[sortColIndex].Data;
                    }
                }

                var unitOfWork = _unitOfWorkFactory.Create();
                var mapper = AutoMapperConfiguration.CreateMapper();

                // Fetch paged user list from DB
                var userList = await unitOfWork.ApplicationUser.GetListDataTableAsync(
                    searchValue, pageNumber, pageSize, sortColumn, sortDirection);

                var mappedData = mapper.Map<IList<ApplicationUserToViewFrontModel>>(userList)
                 ?? new List<ApplicationUserToViewFrontModel>();

                int filteredCount = await unitOfWork.ApplicationUser.GetFilteredCountAsync<int>(searchValue);
                int totalCount = await unitOfWork.ApplicationUser.GetTotalCount<int>();

                var userStatusCount = await
                    unitOfWork.ApplicationUser.GetGetCountDeActiveAndDeleted<CountDeActiveAndDeleted>();

                apiResponse.Success = true;
                apiResponse.Code = Ok().StatusCode;
                apiResponse.Message = "Show Rows";
                apiResponse.Result = new
                {
                    draw = request.Draw,
                    recordsTotal = totalCount,
                    recordsFiltered = filteredCount,
                    data = mappedData,
                    pagination = new PaginationInfo
                    {
                        PageNumber = request.Start / request.Length + 1,
                        PageSize = request.Length,
                        TotalCount = totalCount,
                        TotalPages = (int)Math.Ceiling((double)totalCount / request.Length)
                    },
                    countDeActiveAndDeleted = new CountDeActiveAndDeleted
                    {
                        DeactiveCount = userStatusCount?.DeactiveCount ?? 0,
                        DeletedCount = userStatusCount?.DeletedCount ?? 0
                    }
                };
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                apiResponse.Code = BadRequest().StatusCode;
                apiResponse.Result = new
                {
                    draw = request.Draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<ApplicationUserToViewFrontModel>()
                };
            }

            return apiResponse;
        }

        [HttpPost(nameof(GetById))]
        public async Task<ResponseStandardJsonApi> GetById(ApplicationUserModelId User)
        {
            var apiResponse = new ResponseStandardJsonApi();
            try
            {
                var Mapper = AutoMapperConfiguration.CreateMapper();
                var Data = Mapper.Map<ApplicationUser>(User);
                var unitOfWork = _unitOfWorkFactory.Create();
                var obj = await unitOfWork.ApplicationUser.GetSpecificRows(Data);
                var NewData = Mapper.Map<ApplicationUserUpdateInfoModel>(obj);

                if (NewData != null)
                {
                    apiResponse.Message = "Show Rows";
                    apiResponse.Code = Ok().StatusCode;
                    apiResponse.Success = true;
                    apiResponse.Result = NewData;
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "No Data";
                    apiResponse.Code = NotFound().StatusCode;
                    apiResponse.Result = new NullColumns[] { };
                }
            }
            catch (Exception ex)
            {

                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                apiResponse.Code = BadRequest().StatusCode;
                apiResponse.Result = new NullColumns[] { };
            }
            return apiResponse;
        }

        [HttpPost(nameof(SoftDelete))]
        public async Task<ResponseStandardJsonApi> SoftDelete(ApplicationUserModelId User)
        {
            var apiResponse = new ResponseStandardJsonApi();
            var token = Request.Headers["Authorization"].ToString();
            var UserInfo = InformationToken.GetInfoUsers(token);

            try
            {
                var Mapper = AutoMapperConfiguration.CreateMapper();
                var Data = Mapper.Map<ApplicationUser>(User);
                Data.UpdateId = UserInfo.nameid;
                var unitOfWork = _unitOfWorkFactory.Create();
                await unitOfWork.ApplicationUser.Delete(Data);
                apiResponse.Message = "Delete Successfully";
                apiResponse.Code = Ok().StatusCode;
                apiResponse.Success = true;
                apiResponse.Result = new NullColumns[] { };
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                apiResponse.Code = BadRequest().StatusCode;
                apiResponse.Result = new NullColumns[] { };
            }
            return apiResponse;
        }

        [HttpPost(nameof(HardDelete))]
        public async Task<ResponseStandardJsonApi> HardDelete(ApplicationUserModelId User)
        {
            var apiResponse = new ResponseStandardJsonApi();
            var token = Request.Headers["Authorization"].ToString();
            var UserInfo = InformationToken.GetInfoUsers(token);

            try
            {
                var Mapper = AutoMapperConfiguration.CreateMapper();
                var Data = Mapper.Map<ApplicationUser>(User);
                var unitOfWork = _unitOfWorkFactory.Create();
                await unitOfWork.ApplicationUser.HardDelete(Data);
                apiResponse.Message = "Delete Successfully";
                apiResponse.Code = Ok().StatusCode;
                apiResponse.Success = true;
                apiResponse.Result = new NullColumns[] { };
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                apiResponse.Code = BadRequest().StatusCode;
                apiResponse.Result = new NullColumns[] { };
            }
            return apiResponse;
        }

        [HttpPost(nameof(Active))]
        public async Task<ResponseStandardJsonApi> Active(ApplicationUserModelId User)
        {
            var apiResponse = new ResponseStandardJsonApi();
            var token = Request.Headers["Authorization"].ToString();
            var UserInfo = InformationToken.GetInfoUsers(token);

            try
            {
                var Mapper = AutoMapperConfiguration.CreateMapper();
                var Data = Mapper.Map<ApplicationUser>(User);
                var unitOfWork = _unitOfWorkFactory.Create();
                var Obj = await unitOfWork.ApplicationUser.GetSpecificRows(Data);
                if (Obj != null)
                {
                    Obj.IsActive = !Obj.IsActive;
                    Obj.UpdateId = UserInfo.nameid;
                    await unitOfWork.ApplicationUser.Active(Obj);
                    apiResponse.Message = "Active / InActive Successfully";
                    apiResponse.Code = Ok().StatusCode;
                    apiResponse.Success = true;
                    apiResponse.Result = new NullColumns[] { };
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "No Data";
                    apiResponse.Code = NotFound().StatusCode;
                    apiResponse.Result = new NullColumns[] { };
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                apiResponse.Code = BadRequest().StatusCode;
                apiResponse.Result = new NullColumns[] { };
            }
            return apiResponse;
        }

        [HttpPost(nameof(ResetPassword))]
        public async Task<ResponseStandardJsonApi> ResetPassword(ApplicationUserResetPasswordModel resetPassword)
        {
            var apiResponse = new ResponseStandardJsonApi();

            try
            {
                if (!ModelState.IsValid)
                {
                    apiResponse.Success = false;
                    apiResponse.Code = BadRequest().StatusCode;
                    apiResponse.Message = "Invalid model data.";
                    apiResponse.Result = new NullColumns[] { };
                    return apiResponse;
                }

                if (string.IsNullOrWhiteSpace(resetPassword.ApplicationUserId) ||
                    string.IsNullOrWhiteSpace(resetPassword.OldPasswordHash) ||
                    string.IsNullOrWhiteSpace(resetPassword.NewPasswordHash) ||
                    string.IsNullOrWhiteSpace(resetPassword.ConfirmNewPasswordHash))
                {
                    apiResponse.Success = false;
                    apiResponse.Code = BadRequest().StatusCode;
                    apiResponse.Message = "User ID, Old Password, New Password, and Confirm New Password are required.";
                    apiResponse.Result = new NullColumns[] { };
                    return apiResponse;
                }

                if (resetPassword.NewPasswordHash != resetPassword.ConfirmNewPasswordHash)
                {
                    apiResponse.Success = false;
                    apiResponse.Code = BadRequest().StatusCode;
                    apiResponse.Message = "New password and confirm new password do not match.";
                    apiResponse.Result = new NullColumns[] { };
                    return apiResponse;
                }

                if (resetPassword.OldPasswordHash == resetPassword.NewPasswordHash)
                {
                    apiResponse.Success = false;
                    apiResponse.Code = BadRequest().StatusCode;
                    apiResponse.Message = "New password cannot be the same as the old password.";
                    apiResponse.Result = new NullColumns[] { };
                    return apiResponse;
                }

                var entity = new ApplicationUser
                {
                    ApplicationUserId = resetPassword.ApplicationUserId,
                    ApplicationUserPasswordHash = resetPassword.NewPasswordHash
                };

                var unitOfWork = _unitOfWorkFactory.Create();
                var result = await unitOfWork.ApplicationUser.
                    ResetPasswordAsync<bool>(entity, resetPassword.OldPasswordHash);

                if (result)
                {
                    apiResponse.Message = "Password reset successfully.";
                    apiResponse.Code = Ok().StatusCode;
                    apiResponse.Success = true;
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Code = BadRequest().StatusCode;
                    apiResponse.Message = "Password reset failed. User may not exist or old password incorrect.";
                }

                apiResponse.Result = new NullColumns[] { };
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                apiResponse.Code = BadRequest().StatusCode;
                apiResponse.Result = new NullColumns[] { };
            }
            return apiResponse;
        }

        [HttpPost(nameof(Update))]
        public async Task<ResponseStandardJsonApi> Update(ApplicationUserUpdateInfoModel User)
        {
            var apiResponse = new ResponseStandardJsonApi();
            try
            {
                if (ModelState.IsValid)
                {
                    var mapper = AutoMapperConfiguration.CreateMapper();
                    var data = mapper.Map<ApplicationUser>(User);
                    var unitOfWork = _unitOfWorkFactory.Create();
                    await unitOfWork.ApplicationUser.Update(data);
                    apiResponse.Message = "Update Successfully";
                    apiResponse.Code = Ok().StatusCode;
                    apiResponse.Success = true;
                    apiResponse.Result = new NullColumns[] { };
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Message = "Invalid Model State";
                    apiResponse.Code = BadRequest().StatusCode;
                    apiResponse.Result = new NullColumns[] { };
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                apiResponse.Code = BadRequest().StatusCode;
                apiResponse.Result = new NullColumns[] { };
            }
            return apiResponse;
        }
    }
}
