using Nancy.Json;
using static Domain.Common.CommonClass;

namespace WebApi.Authorization
{
    public class SecuredHelper
    {
        public static JwtAuthResponse GetInfoFromToken(string Token)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string[] source = Token.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            JwtAuthResponse authResponse = serializer.Deserialize<JwtAuthResponse>(source[1]);
            return authResponse;
        }
    }
}