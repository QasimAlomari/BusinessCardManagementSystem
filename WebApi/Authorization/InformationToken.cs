using System.IdentityModel.Tokens.Jwt;
using static Domain.Common.CommonClass;

namespace WebApi.Authorization
{
    public class InformationToken
    {
        public static JwtAuthResponse GetInfoUsers(string strToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var JwtToken = handler.ReadJwtToken(strToken.Replace("Bearer ", ""));
            var InfoDecrypt = SecuredHelper.GetInfoFromToken(JwtToken.ToString());
            return InfoDecrypt;
        }
    }
}
