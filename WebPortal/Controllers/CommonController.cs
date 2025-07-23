using Domain.ViewModel.ApplicationUserViewModel;
using Domain.ViewModel.BusinessCardViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Domain.Common.CommonClass;
using WebSite.Helpers;
using WebPortal.Helpers;
using System.Text;

namespace WebPortal.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult Create()
        {
            var user = LayoutHelper.GetUserFromCookie(Request);
            ViewBag.LayoutPath = LayoutHelper.GetLayoutFromUser(user);
            return View();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<BusinessCardModel>>
            CreateBusinessCard([FromBody] BusinessCardModelCreate businessCard)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    ResponseStandardJson<ApplicationUserModel> ResData = JsonConvert.DeserializeObject<ResponseStandardJson<ApplicationUserModel>>(Request.Cookies["_CookDataResult"]);

                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ResData.Result.ApplicationUserToken);
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);

                    var postTask = await client.PostAsJsonAsync("api/BusinessCard/Add", businessCard);

                    if (postTask.IsSuccessStatusCode)
                    {
                        var resultJson = await postTask.Content.ReadAsStringAsync();
                        ResponseStandardJsonApi ResultData = JsonConvert.DeserializeObject<ResponseStandardJsonApi>(resultJson);

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
            return new ResponseStandardJson<BusinessCardModel> { Success = false, Message = "Failed to create business card" };
        }

        public IActionResult Edit()
        {
            var user = LayoutHelper.GetUserFromCookie(Request);
            ViewBag.LayoutPath = LayoutHelper.GetLayoutFromUser(user);
            return View();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<BusinessCardModel>>
            EditBusinessCard([FromBody] BusinessCardModelUpdate collection)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    ResponseStandardJson<ApplicationUserModel> ResData =
                        (ResponseStandardJson<ApplicationUserModel>)JsonConvert.DeserializeObject
                        (Request.Cookies["_CookDataResult"].ToString(),
                        (typeof(ResponseStandardJson<ApplicationUserModel>)));

                    client.DefaultRequestHeaders.Authorization
                        = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ResData.Result.ApplicationUserToken);
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);

                    BusinessCardModelUpdate modelUpdate = new BusinessCardModelUpdate
                    {
                        BusinessCardId = collection.BusinessCardId,
                        BusinessCardName = collection.BusinessCardName,
                        BusinessCardAddress = collection.BusinessCardAddress,
                        BusinessCardCompany = collection.BusinessCardCompany,
                        BusinessCardPhone = collection.BusinessCardPhone,
                        BusinessCardEmail = collection.BusinessCardEmail,
                        BusinessCardNotes = collection.BusinessCardNotes,
                        BusinessCardTitle = collection.BusinessCardTitle,
                        BusinessCardWebsite = collection.BusinessCardWebsite
                    };

                    var postTask = await client.
                        PostAsJsonAsync<BusinessCardModelUpdate>("api/BusinessCard/Update", modelUpdate);
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

        public async Task<ResponseStandardJson<BusinessCardModel>>
            Details([FromQuery] BusinessCardModelId collection)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var stringContent = new StringContent("", Encoding.UTF8, "application/json");

                    BusinessCardModelId data = new BusinessCardModelId
                    {
                        BusinessCardId = collection.BusinessCardId,
                    };


                    ResponseStandardJson<ApplicationUserModel> ResData = (ResponseStandardJson<ApplicationUserModel>)
                        JsonConvert.DeserializeObject(Request.Cookies["_CookDataResult"].ToString(),
                        (typeof(ResponseStandardJson<ApplicationUserModel>)));

                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ResData.Result.ApplicationUserToken);
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);
                    var postTask = await client.PostAsJsonAsync("api/BusinessCard/GetById", data);

                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = postTask.Content.ReadAsStringAsync().Result;
                        ResponseStandardJson<BusinessCardModel> ResultData = (ResponseStandardJson<BusinessCardModel>)
                            JsonConvert.DeserializeObject(result.ToString(),
                            (typeof(ResponseStandardJson<BusinessCardModel>)));

                        return ResultData;
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
            SoftDeleteBusinessCard([FromBody] BusinessCardModelId entity)
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
                    var postTask = await client.PostAsJsonAsync<BusinessCardModelId>("api/BusinessCard/SoftDelete", data);

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
    }

}
