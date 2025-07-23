using Services.Interfaces.DIInjection;
using Services.Services.BusinessCardServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.UserManagement
{
    public interface IMasterUser<MasterUser> :
        IGetByIdAction<MasterUser>, IAddAction<MasterUser>,IDeleteAction<MasterUser>, IActiveAction<MasterUser> ,
        IGetTotalCount,IHardDelete<MasterUser>, IGetListForDataTable<MasterUser>, IGetCountDeActiveAndDeleted,
        IGetFilteredCount, IUpdateAction<MasterUser>, IScopedService
    {
        Task<MasterUser> GetUserNamePasswordAsync(MasterUser entity);
        Task<T> IsUserExistsByEmailAsync<T>(MasterUser entity);
        Task<T> ResetPasswordAsync<T>(MasterUser entity, string oldPasswordPlainText);
    }
}
