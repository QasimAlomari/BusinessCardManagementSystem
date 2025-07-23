using Domain.ViewModel.CommonViewModel;
using Services.Services.BusinessCardServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IGetListAction<T>
    {
        Task<IList<T>> GetList(T entity);
    }
    public interface IGetByIdAction<T>
    {
        Task<T> GetSpecificRows(T entity);
    }
    public interface IAddAction<T>
    {
        Task Add(T entity);
    }
    public interface IUpdateAction<T>
    {
        Task Update(T entity);
    }
    public interface IDeleteAction<T>
    {
        Task Delete(T entity);
    }
    public interface IActiveAction<T>
    {
        Task Active(T entity);
    }
    public interface IGetListWithPagination<T>
    {
        Task<IList<T>> GetListPagination(T entity, int pageNumber, int pageSize);
    }
    public interface IGetTotalCount
    {
        Task<T> GetTotalCount<T>();
    }   
    public interface IHardDelete<T>
    {
        Task HardDelete(T entity);
    } 
    public interface IGetListForDataTable<T>
    {
        Task<IList<T>> GetListDataTableAsync(
        string searchValue,
        int pageNumber,
        int pageSize,
        string sortColumn,
        string sortDirection);
    }
    public interface IGetFilteredCount
    {
        Task<T> GetFilteredCountAsync<T>(string searchValue);
    }
    public interface IGetCountDeActiveAndDeleted
    {
        Task<T> GetGetCountDeActiveAndDeleted<T>();
    }
}
