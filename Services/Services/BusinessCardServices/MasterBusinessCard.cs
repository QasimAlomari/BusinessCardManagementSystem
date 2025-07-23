using Domain.Entities;
using Repository.Repositories;
using Repository.Respositroies;
using Services.Interfaces.BusinessCard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Services.Services.BusinessCardServices
{
    public class MasterBusinessCard : IMasterBusinessCard<BusinessCard>
    {
        private readonly IRepository<BusinessCard> _Repository;
        public MasterBusinessCard(IRepository<BusinessCard> repository)
        {
            _Repository = repository;
        }
        public async Task Active(BusinessCard entity)
        {
            var obj = new
            {
                BusinessCardId = entity.BusinessCardId,
                IsActive = entity.IsActive,
                UpdateId = entity.UpdateId,
            };
            await _Repository.ExecCommand("Sp_Business_Card_Active", obj);
        }
        public async Task Add(BusinessCard entity)
        {
            var obj = new
            {
                BusinessCardName = entity.BusinessCardName,
                BusinessCardTitle = entity.BusinessCardTitle,
                BusinessCardPhone = entity.BusinessCardPhone,
                BusinessCardEmail = entity.BusinessCardEmail,
                BusinessCardCompany = entity.BusinessCardCompany,
                BusinessCardWebsite = entity.BusinessCardWebsite,
                BusinessCardAddress = entity.BusinessCardAddress,
                BusinessCardNotes = entity.BusinessCardNotes,
                CreateId = entity.CreateId,
            };
            await _Repository.ExecCommand("Sp_Business_Card_Insert", obj);
        }
        public async Task Delete(BusinessCard entity)
        {
            var obj = new
            {
                BusinessCardId = entity.BusinessCardId,
                UpdateId = entity.UpdateId,
            };
            await _Repository.ExecCommand("Sp_Business_Card_Soft_Delete", obj);
        }
        public async Task<IList<BusinessCard>> GetList(BusinessCard entity)
        {
            var obj = new
            {
            };
            return await _Repository.ListData("Sp_Business_Card_List", obj);
        }
        public async Task<IList<BusinessCard>> GetListPagination(BusinessCard entity, int pageNumber, int pageSize)
        {
            var obj = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return await _Repository.ListData("Sp_Business_Card_List_With_Pagination", obj);
        }
        
        public async Task<IList<BusinessCard>> 
            GetListPaginationDependOnCreatedId(BusinessCard entity, int pageNumber, int pageSize, string createId)
        {
            var obj = new
            {
                CreateId = createId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return await _Repository.ListData("Sp_Business_Card_List_With_Pagination_Depend_On_Created_Id", obj);
        }
        public async Task<BusinessCard> GetSpecificRows(BusinessCard entity)
        {
            var obj = new
            {
                BusinessCardId = entity.BusinessCardId,
            };
            return await _Repository.FindExecCommand("Sp_Business_Card_Get_By_Id", obj);
        }
        public async Task<T> GetTotalCount<T>()
        {
            var obj = new
            {
            };
            return await _Repository.ExecuteScalar<T>("Sp_Business_Card_Get_Total_Count", obj);
        }
        public async Task<T> GetTotalCountDependOnCreatedId<T>(string createId)
        {
            var obj = new
            {
                CreateId = createId
            };
            return await _Repository.ExecuteScalar<T>("Sp_Business_Card_Get_Total_Count_Depend_On_Created_Id", obj);
        }
        public async Task HardDelete(BusinessCard entity)
        {
            var obj = new
            {
                BusinessCardId = entity.BusinessCardId,
            };
            await _Repository.ExecCommand("Sp_Business_Card_Hard_Delete", obj);
        } 
        public async Task Update(BusinessCard entity)
        {
            var obj = new
            {
                BusinessCardId = entity.BusinessCardId,
                BusinessCardName = entity.BusinessCardName,
                BusinessCardTitle = entity.BusinessCardTitle,
                BusinessCardPhone = entity.BusinessCardPhone,
                BusinessCardEmail = entity.BusinessCardEmail,
                BusinessCardCompany = entity.BusinessCardCompany,
                BusinessCardWebsite = entity.BusinessCardWebsite,
                BusinessCardAddress = entity.BusinessCardAddress,
                BusinessCardNotes = entity.BusinessCardNotes,
                UpdateId = entity.UpdateId,
            };
            await _Repository.ExecCommand("Sp_Business_Card_Update", obj);
        }
    }
}
