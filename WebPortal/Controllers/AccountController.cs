using Domain.ViewModel.ApplicationUserViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebSite.Helpers;
using static Domain.Common.CommonClass;

namespace WebPortal.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            Response.Cookies.Delete("_CookDataResult");
            return View();
        }

        [HttpPost]
        public async Task<ResponseStandardJson<ApplicationUserModel>>
        Login([FromBody] ApplicationUserModelLogin collection)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(CommonWeb.UrlApi);

                    var postTask = await client.PostAsJsonAsync("api/User/Login", collection);

                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = await postTask.Content.ReadAsStringAsync();

                        var ResultData = JsonConvert.DeserializeObject<ResponseStandardJson<ApplicationUserModel>>(result);

                        if (ResultData == null || ResultData.Result == null)
                        {
                            return new ResponseStandardJson<ApplicationUserModel>
                            {
                                Success = false,
                                Code = 404,
                                Message = "Login failed. User not found or credentials incorrect.",
                                Result = null
                            };
                        }

                        var optionUsers = new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(1)
                        };
                        Response.Cookies.Append("_CookDataResult", JsonConvert.SerializeObject(ResultData), optionUsers);

                        return ResultData;
                    }
                    else
                    {
                        return new ResponseStandardJson<ApplicationUserModel>
                        {
                            Success = false,
                            Code = (int)postTask.StatusCode,
                            Message = "Login request failed. Invalid response from server.",
                            Result = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseStandardJson<ApplicationUserModel>
                    {
                        Success = false,
                        Code = 500,
                        Message = $"An exception occurred: {ex.Message}",
                        Result = null
                    };
                }
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("_CookDataResult");
            return RedirectToAction(nameof(Login));
        }
    }
}
