using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Domain.Common.CommonClass;
using System.Text;
using WebSite.Helpers;
using Domain.Entities;
using Domain.ViewModel.BusinessCardViewModel;
using Domain.ViewModel.ApplicationUserViewModel;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Domain.ViewModel.CommonViewModel;

namespace WebPortal.Controllers
{
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<GetBusinessCardWithListPagination>>
        GetBusinessCardList([FromBody] PaginationInfo paginationInfo)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    paginationInfo.PageNumber = paginationInfo.PageNumber > 0 ? paginationInfo.PageNumber : 1;
                    paginationInfo.PageSize = paginationInfo.PageSize > 0 ? paginationInfo.PageSize : 10;

                    if (Request.Cookies["_CookDataResult"] != null)
                    {
                        var ResData = JsonConvert.DeserializeObject<ResponseStandardJson<ApplicationUserModel>>(Request.Cookies["_CookDataResult"]);

                        client.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ResData.Result.ApplicationUserToken);

                        client.BaseAddress = new Uri(CommonWeb.UrlApi);

                        var postTask = await client.PostAsJsonAsync("api/BusinessCard/GetListWithPagination", paginationInfo);

                        if (postTask.IsSuccessStatusCode)
                        {
                            var result = await postTask.Content.ReadAsStringAsync();
                            var ResultData = JsonConvert.DeserializeObject<ResponseStandardJson<GetBusinessCardWithListPagination>>(result);

                            if (ResultData?.Result == null ||
                                ResultData.Result.BusinessCardModel == null ||
                                ResultData.Result.BusinessCardModel.Count == 0)
                            {
                                return new ResponseStandardJson<GetBusinessCardWithListPagination>
                                {
                                    Success = true,
                                    Code = 200,
                                    Message = "No business cards found.",
                                    Result = ResultData.Result
                                };
                            }

                            return ResultData;
                        }
                        else
                        {
                            return new ResponseStandardJson<GetBusinessCardWithListPagination>
                            {
                                Success = false,
                                Code = (int)postTask.StatusCode,
                                Message = "Failed to fetch users list."
                            };
                        }
                    }
                    else
                    {
                        return new ResponseStandardJson<GetBusinessCardWithListPagination>
                        {
                            Success = false,
                            Code = 401,
                            Message = "Authentication required. Please log in."
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseStandardJson<GetBusinessCardWithListPagination>
                    {
                        Success = false,
                        Code = 500,
                        Message = $"An error occurred: {ex.Message}"
                    };
                }
            }
        }

        [HttpPost]
        public async Task<ResponseStandardJson<BusinessCardModel>>
            HardDeleteBusinessCard([FromBody] BusinessCardModelId entity)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var stringContent = new StringContent("", Encoding.UTF8, "application/json");

                    BusinessCardModelId data = new BusinessCardModelId
                    {
                        BusinessCardId = entity.BusinessCardId,
                    };


                    ResponseStandardJson<ApplicationUserModel> ResData = (ResponseStandardJson<ApplicationUserModel>)
                        JsonConvert.DeserializeObject(Request.Cookies["_CookDataResult"].ToString(),
                        (typeof(ResponseStandardJson<ApplicationUserModel>)));

                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ResData.Result.ApplicationUserToken);
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);
                    var postTask = await client.PostAsJsonAsync<BusinessCardModelId>("api/BusinessCard/HardDelete", data);

                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = postTask.Content.ReadAsStringAsync().Result;
                        ResponseStandardJsonApi ResultData = (ResponseStandardJsonApi)
                            JsonConvert.DeserializeObject(result.ToString(),
                            (typeof(ResponseStandardJsonApi)));

                        return new ResponseStandardJson<BusinessCardModel>
                        {
                            Code = ResultData.Code,
                            Message = ResultData.Message,
                            Success = ResultData.Success,
                            Result = new BusinessCardModel()
                        };
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return new ResponseStandardJson<BusinessCardModel>();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<BusinessCardModel>>
            ActivateBusinessCard([FromBody] BusinessCardModelId entity)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var stringContent = new StringContent("", Encoding.UTF8, "application/json");

                    BusinessCardModelId data = new BusinessCardModelId
                    {
                        BusinessCardId = entity.BusinessCardId,
                    };


                    ResponseStandardJson<ApplicationUserModel> ResData = (ResponseStandardJson<ApplicationUserModel>)
                        JsonConvert.DeserializeObject(Request.Cookies["_CookDataResult"].ToString(),
                        (typeof(ResponseStandardJson<ApplicationUserModel>)));

                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ResData.Result.ApplicationUserToken);
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);
                    var postTask = await client.PostAsJsonAsync<BusinessCardModelId>("api/BusinessCard/Active", data);

                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = postTask.Content.ReadAsStringAsync().Result;
                        ResponseStandardJsonApi ResultData = (ResponseStandardJsonApi)
                            JsonConvert.DeserializeObject(result.ToString(),
                            (typeof(ResponseStandardJsonApi)));

                        return new ResponseStandardJson<BusinessCardModel>
                        {
                            Code = ResultData.Code,
                            Message = ResultData.Message,
                            Success = ResultData.Success,
                            Result = new BusinessCardModel()
                        };
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return new ResponseStandardJson<BusinessCardModel>();
        }
        public ActionResult RegisterUsers()
        {
            return View();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<ApplicationUserModel>>
            AdminRegisterUsers([FromBody] ApplicationUserModelRegister applicationUserRegister)
        {
            var response = new ResponseStandardJson<ApplicationUserModel>();

            try
            {
                if (!Request.Cookies.ContainsKey("_CookDataResult"))
                {
                    return new ResponseStandardJson<ApplicationUserModel>
                    {
                        Success = false,
                        Code = 401,
                        Message = "Authentication cookie '_CookDataResult' is missing.",
                        Result = null
                    };
                }

                var cookieValue = Request.Cookies["_CookDataResult"];
                if (string.IsNullOrEmpty(cookieValue))
                {
                    return new ResponseStandardJson<ApplicationUserModel>
                    {
                        Success = false,
                        Code = 401,
                        Message = "Authentication cookie '_CookDataResult' is empty.",
                        Result = null
                    };
                }

                var resData = JsonConvert.DeserializeObject<ResponseStandardJson<ApplicationUserModel>>(cookieValue);
                if (resData == null || resData.Result == null || string.IsNullOrEmpty(resData.Result.ApplicationUserToken))
                {
                    return new ResponseStandardJson<ApplicationUserModel>
                    {
                        Success = false,
                        Code = 401,
                        Message = "Invalid authentication cookie data or missing token.",
                        Result = null
                    };
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue
                        ("Bearer", resData.Result.ApplicationUserToken);

                    var data = new ApplicationUserModelRegister()
                    {
                        UserName = applicationUserRegister.UserName,
                        Email = applicationUserRegister.Email,
                        Password = applicationUserRegister.Password,
                        UserRole = applicationUserRegister.UserRole
                    };

                    var postTask = await client.PostAsJsonAsync("api/User/Register", data);

                    if (postTask.IsSuccessStatusCode)
                    {
                        var resultContent = await postTask.Content.ReadAsStringAsync();
                        var resultData = JsonConvert.DeserializeObject<ResponseStandardJsonApi>(resultContent);

                        return new ResponseStandardJson<ApplicationUserModel>
                        {
                            Code = resultData?.Code ?? 200,
                            Message = resultData?.Message ?? "User registered successfully",
                            Success = resultData?.Success ?? true,
                            Result = new ApplicationUserModel()
                        };
                    }
                    else
                    {
                        var errorContent = await postTask.Content.ReadAsStringAsync();
                        return new ResponseStandardJson<ApplicationUserModel>
                        {
                            Success = false,
                            Code = (int)postTask.StatusCode,
                            Message = $"User registration failed: {errorContent}",
                            Result = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseStandardJson<ApplicationUserModel>
                {
                    Success = false,
                    Code = 500,
                    Message = $"Exception: {ex.Message}",
                    Result = null
                };
            }
        }
        public ActionResult ListUsers()
        {
            return View();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<GetApplicationUserPaginationModel>>
             GetApplicationUserList([FromBody] DataTablesRequest dataTablesRequest)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    if (dataTablesRequest.Length <= 0)
                        dataTablesRequest.Length = 10;

                    if (Request.Cookies["_CookDataResult"] != null)
                    {
                        var cookieData = Request.Cookies["_CookDataResult"];
                        var ResData = JsonConvert.DeserializeObject
                            <ResponseStandardJson<ApplicationUserModel>>(cookieData);

                        client.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue
                            ("Bearer", ResData.Result.ApplicationUserToken);

                        client.BaseAddress = new Uri(CommonWeb.UrlApi);

                        var postTask = await client.
                            PostAsJsonAsync("api/User/GetListForDataTable", dataTablesRequest);

                        if (postTask.IsSuccessStatusCode)
                        {
                            var result = await postTask.Content.ReadAsStringAsync();
                            var ResultData = JsonConvert.DeserializeObject
                                <ResponseStandardJson<GetApplicationUserPaginationModel>>(result);

                            return ResultData;
                        }
                        else
                        {
                            var errorContent = await postTask.Content.ReadAsStringAsync();
                            return new ResponseStandardJson<GetApplicationUserPaginationModel>
                            {
                                Success = false,
                                Message = $"Failed to fetch users list. API returned {(int)postTask.StatusCode} {postTask.StatusCode}. Content: {errorContent}"
                            };
                        }
                    }
                    else
                    {
                        return new ResponseStandardJson<GetApplicationUserPaginationModel>
                        {
                            Success = false,
                            Message = "Authentication required. Please log in."
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseStandardJson<GetApplicationUserPaginationModel>
                    {
                        Success = false,
                        Message = $"An error occurred: {ex.Message}"
                    };
                }
            }
        }

        [HttpPost]
        public async Task<ResponseStandardJson<ApplicationUserModel>>
            HardDeleteUser([FromBody] ApplicationUserModelId entity)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var stringContent = new StringContent("", Encoding.UTF8, "application/json");

                    ApplicationUserModelId data = new ApplicationUserModelId
                    {
                        ApplicationUserId = entity.ApplicationUserId
                    };


                    ResponseStandardJson<ApplicationUserModel> ResData = (ResponseStandardJson<ApplicationUserModel>)
                        JsonConvert.DeserializeObject(Request.Cookies["_CookDataResult"].ToString(),
                        (typeof(ResponseStandardJson<ApplicationUserModel>)));

                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ResData.Result.ApplicationUserToken);
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);
                    var postTask = await client.PostAsJsonAsync("api/User/HardDelete", data);

                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = postTask.Content.ReadAsStringAsync().Result;
                        ResponseStandardJsonApi ResultData = (ResponseStandardJsonApi)
                            JsonConvert.DeserializeObject(result.ToString(),
                            (typeof(ResponseStandardJsonApi)));

                        return new ResponseStandardJson<ApplicationUserModel>
                        {
                            Code = ResultData.Code,
                            Message = ResultData.Message,
                            Success = ResultData.Success,
                            Result = new ApplicationUserModel()
                        };
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return new ResponseStandardJson<ApplicationUserModel>();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<ApplicationUserModel>>
            SoftDeleteUser([FromBody] ApplicationUserModelId entity)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var stringContent = new StringContent("", Encoding.UTF8, "application/json");

                    ApplicationUserModelId data = new ApplicationUserModelId
                    {
                        ApplicationUserId = entity.ApplicationUserId
                    };


                    ResponseStandardJson<ApplicationUserModel> ResData = (ResponseStandardJson<ApplicationUserModel>)
                        JsonConvert.DeserializeObject(Request.Cookies["_CookDataResult"].ToString(),
                        (typeof(ResponseStandardJson<ApplicationUserModel>)));

                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ResData.Result.ApplicationUserToken);
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);
                    var postTask = await client.PostAsJsonAsync("api/User/SoftDelete", data);

                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = postTask.Content.ReadAsStringAsync().Result;
                        ResponseStandardJsonApi ResultData = (ResponseStandardJsonApi)
                            JsonConvert.DeserializeObject(result.ToString(),
                            (typeof(ResponseStandardJsonApi)));

                        return new ResponseStandardJson<ApplicationUserModel>
                        {
                            Code = ResultData.Code,
                            Message = ResultData.Message,
                            Success = ResultData.Success,
                            Result = new ApplicationUserModel()
                        };
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return new ResponseStandardJson<ApplicationUserModel>();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<ApplicationUserModel>>
            ActivateUser([FromBody] ApplicationUserModelId entity)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var stringContent = new StringContent("", Encoding.UTF8, "application/json");

                    ApplicationUserModelId data = new ApplicationUserModelId
                    {
                        ApplicationUserId = entity.ApplicationUserId
                    };


                    ResponseStandardJson<ApplicationUserModel> ResData = (ResponseStandardJson<ApplicationUserModel>)
                        JsonConvert.DeserializeObject(Request.Cookies["_CookDataResult"].ToString(),
                        (typeof(ResponseStandardJson<ApplicationUserModel>)));

                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ResData.Result.ApplicationUserToken);
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);
                    var postTask = await client.PostAsJsonAsync("api/User/Active", data);

                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = postTask.Content.ReadAsStringAsync().Result;
                        ResponseStandardJsonApi ResultData = (ResponseStandardJsonApi)
                            JsonConvert.DeserializeObject(result.ToString(),
                            (typeof(ResponseStandardJsonApi)));

                        return new ResponseStandardJson<ApplicationUserModel>
                        {
                            Code = ResultData.Code,
                            Message = ResultData.Message,
                            Success = ResultData.Success,
                            Result = new ApplicationUserModel()
                        };
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return new ResponseStandardJson<ApplicationUserModel>();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<ApplicationUserModel>>
            ResetPasswordUser([FromBody] ApplicationUserResetPasswordModel entity)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var stringContent = new StringContent("", Encoding.UTF8, "application/json");

                    ApplicationUserResetPasswordModel data = new ApplicationUserResetPasswordModel
                    {
                        ApplicationUserId = entity.ApplicationUserId,
                        OldPasswordHash = entity.OldPasswordHash,
                        NewPasswordHash = entity.NewPasswordHash,
                        ConfirmNewPasswordHash = entity.ConfirmNewPasswordHash
                    };


                    ResponseStandardJson<ApplicationUserModel> ResData = (ResponseStandardJson<ApplicationUserModel>)
                        JsonConvert.DeserializeObject(Request.Cookies["_CookDataResult"].ToString(),
                        (typeof(ResponseStandardJson<ApplicationUserModel>)));

                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ResData.Result.ApplicationUserToken);
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);
                    var postTask = await client.PostAsJsonAsync("api/User/ResetPassword", data);

                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = postTask.Content.ReadAsStringAsync().Result;
                        ResponseStandardJsonApi ResultData = (ResponseStandardJsonApi)
                            JsonConvert.DeserializeObject(result.ToString(),
                            (typeof(ResponseStandardJsonApi)));

                        return new ResponseStandardJson<ApplicationUserModel>
                        {
                            Code = ResultData.Code,
                            Message = ResultData.Message,
                            Success = ResultData.Success,
                            Result = new ApplicationUserModel()
                        };
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return new ResponseStandardJson<ApplicationUserModel>();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<ApplicationUserUpdateInfoModel>>
            UpdateUserInfo([FromBody] ApplicationUserUpdateInfoModel entity)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var stringContent = new StringContent("", Encoding.UTF8, "application/json");

                    ApplicationUserUpdateInfoModel data = new ApplicationUserUpdateInfoModel
                    {
                        ApplicationUserId = entity.ApplicationUserId,
                        ApplicationUserEmail = entity.ApplicationUserEmail,
                        ApplicationUserUserName = entity.ApplicationUserUserName
                    };


                    ResponseStandardJson<ApplicationUserModel> ResData = (ResponseStandardJson<ApplicationUserModel>)
                        JsonConvert.DeserializeObject(Request.Cookies["_CookDataResult"].ToString(),
                        (typeof(ResponseStandardJson<ApplicationUserModel>)));

                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue
                        ("Bearer", ResData.Result.ApplicationUserToken);
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);
                    var postTask = await client.PostAsJsonAsync("api/User/Update", data);

                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = postTask.Content.ReadAsStringAsync().Result;
                        ResponseStandardJsonApi ResultData = (ResponseStandardJsonApi)
                            JsonConvert.DeserializeObject(result.ToString(),
                            (typeof(ResponseStandardJsonApi)));

                        return new ResponseStandardJson<ApplicationUserUpdateInfoModel>
                        {
                            Code = ResultData.Code,
                            Message = ResultData.Message,
                            Success = ResultData.Success,
                            Result = new ApplicationUserUpdateInfoModel()
                        };
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return new ResponseStandardJson<ApplicationUserUpdateInfoModel>();
        }

        [HttpPost]
        public async Task<ResponseStandardJsonApi> ImportExcel([FromForm] IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
                return new ResponseStandardJsonApi
                {
                    Success = false,
                    Message = "No file uploaded.",
                    Code = BadRequest().StatusCode,
                    Result = Array.Empty<string>()
                };

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(CommonWeb.UrlApi);

            var cookieData = Request.Cookies["_CookDataResult"];
            if (string.IsNullOrEmpty(cookieData))
                throw new InvalidOperationException("Authentication token not found.");

            var ResData = JsonConvert.DeserializeObject<ResponseStandardJson<ApplicationUserModel>>(cookieData);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
                ("Bearer", ResData.Result.ApplicationUserToken);

            using var content = new MultipartFormDataContent();

            using var stream = excelFile.OpenReadStream();
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(excelFile.ContentType);

            content.Add(streamContent, "file", excelFile.FileName);

            var response = await httpClient.PostAsync("api/BusinessCard/UploadBusinessCardsExcel", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new ResponseStandardJsonApi
                {
                    Success = false,
                    Message = $"Upload failed with status code: {response.StatusCode}. Details: {errorContent}",
                    Code = (int)response.StatusCode,
                    Result = Array.Empty<string>()
                };
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ResponseStandardJsonApi>(responseString);

            return apiResponse ?? new ResponseStandardJsonApi
            {
                Success = false,
                Message = "Failed to parse response from upload API.",
                Code = StatusCodes.Status500InternalServerError,
                Result = Array.Empty<string>()
            };
        }

    }
}
