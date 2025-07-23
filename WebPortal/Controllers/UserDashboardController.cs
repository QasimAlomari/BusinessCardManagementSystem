using Domain.ViewModel.ApplicationUserViewModel;
using Domain.ViewModel.BusinessCardViewModel;
using Domain.ViewModel.CommonViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Domain.Common.CommonClass;
using WebSite.Helpers;
using System.Text;

namespace WebPortal.Controllers
{
    public class UserDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<GetBusinessCardWithListPagination>>
        GetBusinessCardListDependOnCreatedId([FromBody] PaginationInfo paginationInfo)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    paginationInfo.PageNumber = paginationInfo.PageNumber > 0 ? paginationInfo.PageNumber : 1;
                    paginationInfo.PageSize = paginationInfo.PageSize > 0 ? paginationInfo.PageSize : 10;

                    if (Request.Cookies["_CookDataResult"] != null)
                    {
                        var ResData = JsonConvert.DeserializeObject<ResponseStandardJson<ApplicationUserModel>>
                            (Request.Cookies["_CookDataResult"]);

                        client.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ResData.Result.ApplicationUserToken);

                        client.BaseAddress = new Uri(CommonWeb.UrlApi);
                        var postTask = await client.PostAsJsonAsync(
                            "api/BusinessCard/GetListWithPaginationDependOnCreatedId", paginationInfo);

                        if (postTask.IsSuccessStatusCode)
                        {
                            var result = await postTask.Content.ReadAsStringAsync();
                            var ResultData = JsonConvert.DeserializeObject<ResponseStandardJson<GetBusinessCardWithListPagination>>(result);

                            // ✅ Validate if the list is empty
                            if (ResultData?.Result == null ||
                                ResultData.Result.BusinessCardModel == null ||
                                !ResultData.Result.BusinessCardModel.Any())
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
                                Code = 500,
                                Message = "Failed to fetch business card list.",
                                Result = null
                            };
                        }
                    }
                    else
                    {
                        return new ResponseStandardJson<GetBusinessCardWithListPagination>
                        {
                            Success = false,
                            Code = 401,
                            Message = "Authentication required. Please log in.",
                            Result = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseStandardJson<GetBusinessCardWithListPagination>
                    {
                        Success = false,
                        Code = 500,
                        Message = $"An error occurred: {ex.Message}",
                        Result = null
                    };
                }
            }
        }
    }
}
