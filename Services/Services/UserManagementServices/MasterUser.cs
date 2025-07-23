using Domain.Common;
using Domain.Entities;
using Domain.ViewModel.CommonViewModel;
using Repository.Respositroies;
using Services.Interfaces.UserManagement;
using Services.Services.BusinessCardServices;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Services.Services.UserManagementServices
{
    public class MasterUser : IMasterUser<ApplicationUser>
    {
        private readonly IRepository<ApplicationUser> _Repository;
        public MasterUser(IRepository<ApplicationUser> repository)
        {
            _Repository = repository;
        }
        public async Task Active(ApplicationUser entity)
        {
            var obj = new
            {
                ApplicationUserId = entity.ApplicationUserId,
                IsActive = entity.IsActive,
                UpdateId = entity.UpdateId,
            };
            await _Repository.ExecCommand("Sp_User_Active", obj);
        }
        public async Task Add(ApplicationUser entity)
        {
            var obj = new
            {
                UserName = entity.ApplicationUserUserName,
                Email    = entity.ApplicationUserEmail,
                PasswordHash = CommonClass.HashPassword(entity.ApplicationUserPasswordHash),
                Role = (int)entity.ApplicationUserRole,
                CreateId = entity.CreateId,
            };
            await _Repository.ExecCommand("Sp_User_Register", obj);
        }
        public async Task Delete(ApplicationUser entity)
        {
            var obj = new
            {
                ApplicationUserId = entity.ApplicationUserId,
                UpdateId = entity.UpdateId,
            };
            await _Repository.ExecCommand("Sp_User_Soft_Delete", obj);
        }
        public async Task<T> GetFilteredCountAsync<T>(string searchValue)
        {
            var obj = new
            {
                SearchValue = searchValue,
            };
            return await _Repository.ExecuteScalar<T>("Sp_User_Get_Filtered_Count", obj);
        }
        public async Task<T> GetGetCountDeActiveAndDeleted<T>()
        {
            var obj = new
            {
            };
            return await _Repository.GetSingleRow<T>("Sp_User_Get_Count_For_DeActive_Delete", obj);
        }
        public async Task<IList<ApplicationUser>> GetListDataTableAsync
            (string searchValue, int pageNumber, int pageSize, string sortColumn, string sortDirection)
        {
            var obj = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchValue = searchValue,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };
            return await _Repository.ListData("Sp_User_List_For_Data_Table", obj);
        }    
        public async Task<ApplicationUser> GetSpecificRows(ApplicationUser entity)
        {
            var obj = new
            {
                ApplicationUserId = entity.ApplicationUserId,
            };
            return await _Repository.FindExecCommand("Sp_User_Get_By_Id", obj);
        }
        public async Task<T> GetTotalCount<T>()
        {
            var obj = new
            {
            };
            return await _Repository.ExecuteScalar<T>("Sp_User_Get_Total_Count", obj);
        }
        public async Task<ApplicationUser> GetUserNamePasswordAsync(ApplicationUser entity)
        {
            string hashedPassword = CommonClass.HashPassword(entity.ApplicationUserPasswordHash);
            var obj = new
            {
                Email = entity.ApplicationUserEmail,
                Password = hashedPassword,
            };
            return await _Repository.FindExecCommand("Sp_User_Login", obj);
        }
        public async Task HardDelete(ApplicationUser entity)
        {
            var obj = new
            {
                ApplicationUserId = entity.ApplicationUserId,
            };
            await _Repository.ExecCommand("Sp_User_Hard_Delete", obj);
        }
        public async Task<T> IsUserExistsByEmailAsync<T>(ApplicationUser entity)
        {
            var obj = new
            {
                Email = entity.ApplicationUserEmail,
                Role = entity.ApplicationUserRole
            };
            return await _Repository.ExecuteScalar<T>("Sp_Is_User_Exists_By_Email", obj);
        }
        public async Task<T> ResetPasswordAsync<T>(ApplicationUser entity, string oldPasswordPlainText)
        {
            var obj = new
            {
                ApplicationUserId = entity.ApplicationUserId,
                OldPassword = CommonClass.HashPassword(oldPasswordPlainText),  
                NewPassword = CommonClass.HashPassword(entity.ApplicationUserPasswordHash)
            };
            return await _Repository.ExecuteScalar<T>("Sp_User_Update_Password", obj);
        }
        public async Task Update(ApplicationUser entity)
        {
            var obj = new
            {
                ApplicationUserId = entity.ApplicationUserId,
                UserName = entity.ApplicationUserUserName,
                Email = entity.ApplicationUserEmail
            };
            await _Repository.ExecCommand("Sp_User_Update_User_Info", obj);
        }
    }
}
