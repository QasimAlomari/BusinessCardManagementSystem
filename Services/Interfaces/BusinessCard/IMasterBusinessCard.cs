using Services.Interfaces.DIInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.BusinessCard
{
    public interface IMasterBusinessCard<MasterBusinessCard> : IGetListAction<MasterBusinessCard>
        , IGetByIdAction<MasterBusinessCard> , IAddAction<MasterBusinessCard> , IUpdateAction<MasterBusinessCard> 
        , IDeleteAction<MasterBusinessCard> , IActiveAction<MasterBusinessCard> , IGetListWithPagination<MasterBusinessCard>
        , IGetTotalCount, IHardDelete<MasterBusinessCard>, IScopedService
    {
        Task<IList<MasterBusinessCard>> GetListPaginationDependOnCreatedId
            (MasterBusinessCard entity, int pageNumber, int pageSize,string createId);
        Task<T> GetTotalCountDependOnCreatedId<T>(string createId);
    }
}
