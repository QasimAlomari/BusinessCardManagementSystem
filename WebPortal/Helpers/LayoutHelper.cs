using Domain.Enums;
using Domain.ViewModel.ApplicationUserViewModel;
using Newtonsoft.Json;
using static Domain.Common.CommonClass;

namespace WebPortal.Helpers
{
    public static class LayoutHelper
    {
        public static string GetLayoutFromUser(ApplicationUserModel? user)
        {
            if (user == null || string.IsNullOrEmpty(user.ApplicationUserRole))
                return "~/Views/Shared/_LayoutUserDashboard.cshtml";

            return user.ApplicationUserRole switch
            {
                nameof(UserRole.Admin) => "~/Views/Shared/_LayoutAdminDashboard.cshtml",
                nameof(UserRole.User) => "~/Views/Shared/_LayoutUserDashboard.cshtml",
                _ => "~/Views/Shared/_LayoutUserDashboard.cshtml"
            };
        }
        public static ApplicationUserModel? GetUserFromCookie(HttpRequest request)
        {
            var cookie = request.Cookies["_CookDataResult"];
            if (string.IsNullOrEmpty(cookie)) return null;

            try
            {
                var userWrapper = JsonConvert.DeserializeObject<ResponseStandardJson<ApplicationUserModel>>(cookie);
                return userWrapper?.Result;
            }
            catch
            {
                return null;
            }
        }
    }
}
