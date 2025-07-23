using Domain.AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.UnitOfWork;
using Services.UnitWork;
using static Domain.Common.CommonClass;
using WebApi.Authorization;
using Domain.Entities;
using Domain.ViewModel.BusinessCardViewModel;
using Domain.ViewModel.CommonViewModel;
using System.Data;
using Services.Interfaces.BulkExcelUpload;
using System.Text.RegularExpressions;
using Domain.Common;

namespace WebApi.Controllers.BusinessCardManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCardController : ControllerBase
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IExcelHelper _excelHelper;
        private readonly IBulkExcelUpload<BusinessCardExcelModelCreate> _bulkExcelUpload;

        public BusinessCardController(
            IUnitOfWorkFactory unitOfWorkFactory,
            IExcelHelper excelHelper,
            IBulkExcelUpload<BusinessCardExcelModelCreate> bulkExcelUpload)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _excelHelper = excelHelper;
            _bulkExcelUpload = bulkExcelUpload;
        }

        [HttpPost(nameof(GetList))]
        public async Task<ResponseStandardJsonApi> GetList()
        {
            var apiResponse = new ResponseStandardJsonApi();
            try
            {
                var Token = Request.Headers["Authorization"].ToString();
                var UserInfo = InformationToken.GetInfoUsers(Token);
                var unitOfWork = _unitOfWorkFactory.Create();
                var entityList = await unitOfWork.BusinessCard.GetList(new BusinessCard());
                var mapper = AutoMapperConfiguration.CreateMapper();
                var viewModelList = mapper.Map<IList<BusinessCardModel>>(entityList);

                apiResponse.Message = "Show Rows";
                apiResponse.Code = Ok().StatusCode;
                apiResponse.Success = true;
                apiResponse.Result = viewModelList;
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

        [HttpPost(nameof(GetListWithPagination))]
        public async Task<ResponseStandardJsonApi> GetListWithPagination([FromBody] Pagination pagination)
        {
            var apiResponse = new ResponseStandardJsonApi();

            try
            {
                var Token = Request.Headers["Authorization"].ToString();
                var UserInfo = InformationToken.GetInfoUsers(Token);

                pagination.PageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;
                pagination.PageSize = pagination.PageSize < 1 ? 10 : pagination.PageSize;

                var mapper = AutoMapperConfiguration.CreateMapper();
                var unitOfWork = _unitOfWorkFactory.Create();
                var userList = await unitOfWork.BusinessCard.GetListPagination
                    (new BusinessCard(), pagination.PageNumber, pagination.PageSize);
                var mappedData = mapper.Map<IList<BusinessCardModel>>(userList);
                var totalUsers = await unitOfWork.BusinessCard.GetTotalCount<int>();
                var totalPages = (int)Math.Ceiling((double)totalUsers / pagination.PageSize);


                var paginationInfo = new PaginationInfo
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalCount = totalUsers,
                    TotalPages = totalPages
                };

                if (mappedData != null && mappedData.Count > 0)
                {
                    apiResponse.Message = "Show Rows";
                    apiResponse.Code = Ok().StatusCode;
                    apiResponse.Success = true;
                    apiResponse.Result = new GetBusinessCardWithListPagination
                    {
                        BusinessCardModel = mappedData.ToList(),
                        Pagination = paginationInfo
                    };
                }
                else
                {
                    apiResponse.Message = "No Data";
                    apiResponse.Code = NotFound().StatusCode;
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

        [HttpPost(nameof(GetListWithPaginationDependOnCreatedId))]
        public async Task<ResponseStandardJsonApi> GetListWithPaginationDependOnCreatedId
            ([FromBody] Pagination pagination)
        {
            var apiResponse = new ResponseStandardJsonApi();

            try
            {
                var Token = Request.Headers["Authorization"].ToString();
                var UserInfo = InformationToken.GetInfoUsers(Token);

                pagination.PageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;
                pagination.PageSize = pagination.PageSize < 1 ? 10 : pagination.PageSize;

                var mapper = AutoMapperConfiguration.CreateMapper();
                var unitOfWork = _unitOfWorkFactory.Create();
                var userList = await unitOfWork.BusinessCard.GetListPaginationDependOnCreatedId
                    (new BusinessCard(), pagination.PageNumber, pagination.PageSize, UserInfo.nameid);
                var mappedData = mapper.Map<IList<BusinessCardModel>>(userList);
                var totalUsers = await unitOfWork.BusinessCard.
                    GetTotalCountDependOnCreatedId<int>(UserInfo.nameid);
                var totalPages = (int)Math.Ceiling((double)totalUsers / pagination.PageSize);


                var paginationInfo = new PaginationInfo
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalCount = totalUsers,
                    TotalPages = totalPages
                };

                if (mappedData != null && mappedData.Count > 0)
                {
                    apiResponse.Message = "Show Rows";
                    apiResponse.Code = Ok().StatusCode;
                    apiResponse.Success = true;
                    apiResponse.Result = new GetBusinessCardWithListPagination
                    {
                        BusinessCardModel = mappedData.ToList(),
                        Pagination = paginationInfo
                    };
                }
                else
                {
                    apiResponse.Message = "No Data";
                    apiResponse.Code = NotFound().StatusCode;
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

        [HttpPost(nameof(GetById))]
        public async Task<ResponseStandardJsonApi> GetById(BusinessCardModelId Card)
        {
            var apiResponse = new ResponseStandardJsonApi();
            try
            {
                var Mapper = AutoMapperConfiguration.CreateMapper();
                var Data = Mapper.Map<BusinessCard>(Card);
                var unitOfWork = _unitOfWorkFactory.Create();
                var obj = await unitOfWork.BusinessCard.GetSpecificRows(Data);
                var NewData = Mapper.Map<BusinessCardModel>(obj);
                var token = Request.Headers["Authorization"].ToString();
                var UserInfo = InformationToken.GetInfoUsers(token);

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

        [HttpPost(nameof(Add))]
        public async Task<ResponseStandardJsonApi> Add(BusinessCardModelCreate Card)
        {
            var apiResponse = new ResponseStandardJsonApi();
            var token = Request.Headers["Authorization"].ToString();
            var UserInfo = InformationToken.GetInfoUsers(token);

            if (!ModelState.IsValid)
            {
                apiResponse.Success = false;
                apiResponse.Message = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(e => e.ErrorMessage));
                apiResponse.Code = BadRequest().StatusCode;
                apiResponse.Result = new NullColumns[] { };
                return apiResponse;
            }

            try
            {
                var Mapper = AutoMapperConfiguration.CreateMapper();
                var Data = Mapper.Map<BusinessCard>(Card);
                Data.CreateId = UserInfo.nameid;
                var unitOfWork = _unitOfWorkFactory.Create();
                await unitOfWork.BusinessCard.Add(Data);
                apiResponse.Message = "Add Successfully";
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

        [HttpPost(nameof(Update))]
        public async Task<ResponseStandardJsonApi> Update(BusinessCardModelUpdate Card)
        {
            var apiResponse = new ResponseStandardJsonApi();
            var token = Request.Headers["Authorization"].ToString();
            var UserInfo = InformationToken.GetInfoUsers(token);

            if (!ModelState.IsValid)
            {
                apiResponse.Success = false;
                apiResponse.Message = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(e => e.ErrorMessage));
                apiResponse.Code = BadRequest().StatusCode;
                apiResponse.Result = new NullColumns[] { };
                return apiResponse;
            }

            try
            {
                var Mapper = AutoMapperConfiguration.CreateMapper();
                var Data = Mapper.Map<BusinessCard>(Card);
                Data.UpdateId = UserInfo.nameid;
                var unitOfWork = _unitOfWorkFactory.Create();
                await unitOfWork.BusinessCard.Update(Data);
                apiResponse.Message = "Update Successfully";
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

        [HttpPost(nameof(SoftDelete))]
        public async Task<ResponseStandardJsonApi> SoftDelete(BusinessCardModelId Card)
        {
            var apiResponse = new ResponseStandardJsonApi();
            var token = Request.Headers["Authorization"].ToString();
            var UserInfo = InformationToken.GetInfoUsers(token);

            try
            {
                var Mapper = AutoMapperConfiguration.CreateMapper();
                var Data = Mapper.Map<BusinessCard>(Card);
                Data.UpdateId = UserInfo.nameid;
                var unitOfWork = _unitOfWorkFactory.Create();
                await unitOfWork.BusinessCard.Delete(Data);
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
        public async Task<ResponseStandardJsonApi> HardDelete(BusinessCardModelId Card)
        {
            var apiResponse = new ResponseStandardJsonApi();
            var token = Request.Headers["Authorization"].ToString();
            var UserInfo = InformationToken.GetInfoUsers(token);

            try
            {
                var Mapper = AutoMapperConfiguration.CreateMapper();
                var Data = Mapper.Map<BusinessCard>(Card);
                var unitOfWork = _unitOfWorkFactory.Create();
                await unitOfWork.BusinessCard.HardDelete(Data);
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
        public async Task<ResponseStandardJsonApi> Active(BusinessCardModelId Card)
        {
            var apiResponse = new ResponseStandardJsonApi();
            var token = Request.Headers["Authorization"].ToString();
            var UserInfo = InformationToken.GetInfoUsers(token);

            try
            {
                var Mapper = AutoMapperConfiguration.CreateMapper();
                var Data = Mapper.Map<BusinessCard>(Card);
                var unitOfWork = _unitOfWorkFactory.Create();
                var Obj = await unitOfWork.BusinessCard.GetSpecificRows(Data);
                if (Obj != null)
                {
                    Obj.IsActive = !Obj.IsActive;
                    Obj.UpdateId = UserInfo.nameid;
                    await unitOfWork.BusinessCard.Active(Obj);
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

        [HttpPost("UploadBusinessCardsExcel")]
        public async Task<ResponseStandardJsonApi> UploadBusinessCardsExcel(IFormFile file)
        {
            var token = Request.Headers["Authorization"].ToString();
            var userInfo = InformationToken.GetInfoUsers(token);

            try
            {
                if (file == null || file.Length == 0)
                {
                    return new ResponseStandardJsonApi
                    {
                        Success = false,
                        Message = "No file uploaded.",
                        Code = BadRequest().StatusCode,
                        Result = Array.Empty<string>()
                    };
                }

                var allowedExtensions = new[] { ".xlsx", ".xls" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    return new ResponseStandardJsonApi
                    {
                        Success = false,
                        Message = "Invalid file format. Please upload a valid Excel file.",
                        Code = BadRequest().StatusCode,
                        Result = Array.Empty<string>()
                    };
                }

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Position = 0;

                var dataTable = _excelHelper.ReadExcelToDataTable(stream, file.FileName);

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    return new ResponseStandardJsonApi
                    {
                        Success = false,
                        Message = "Excel file is empty.",
                        Code = BadRequest().StatusCode,
                        Result = Array.Empty<string>()
                    };
                }

                // Validate headers
                var headerValidationResult = _bulkExcelUpload.ValidateHeadersAsync(dataTable);
                if (!headerValidationResult.IsValid)
                {
                    return new ResponseStandardJsonApi
                    {
                        Success = false,
                        Message = headerValidationResult.Message,
                        Code = BadRequest().StatusCode,
                        Result = headerValidationResult.MissingHeaders
                    };
                }

                // Validate content
                var validationErrors = await _bulkExcelUpload.ValidateDataAsync(dataTable, userInfo.nameid);
                if (validationErrors.Count > 0)
                {
                    return new ResponseStandardJsonApi
                    {
                        Success = false,
                        Message = "Validation failed.",
                        Code = BadRequest().StatusCode,
                        Result = validationErrors
                    };
                }

                // Convert DataTable to List<BusinessCardExcelModelCreate>
                var cards = await _bulkExcelUpload.ConvertDataTableToListAsync(dataTable, userInfo.nameid);

                // Perform insert using the list, not the DataTable
                await _bulkExcelUpload.BulkInsertAsync(cards, userInfo.nameid);

                return new ResponseStandardJsonApi
                {
                    Success = true,
                    Message = "Upload and processing completed successfully.",
                    Code = Ok().StatusCode,
                    Result = Array.Empty<string>()
                };
            }
            catch (Exception ex)
            {
                return new ResponseStandardJsonApi
                {
                    Success = false,
                    Message = $"Server error: {ex.Message}",
                    Code = StatusCode(500).StatusCode,
                    Result = Array.Empty<string>()
                };
            }
        }

    }
}
