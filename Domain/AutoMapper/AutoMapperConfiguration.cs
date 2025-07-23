using AutoMapper;
using Domain.Entities;
using Domain.ViewModel;
using Domain.ViewModel.ApplicationUserViewModel;
using Domain.ViewModel.BusinessCardViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static IMapper CreateMapper()
        {
            var MapConfig = new MapperConfiguration(x =>
            {
                #region
                    x.CreateMap<ApplicationUser, ApplicationUserModel>().ReverseMap();    
                    x.CreateMap<ApplicationUser, ApplicationUserModelId>().ReverseMap();    
                    x.CreateMap<ApplicationUser, ApplicationUserToViewFrontModel>().ReverseMap();    
                    x.CreateMap<ApplicationUser, ApplicationUserResetPasswordModel>().ReverseMap();    
                    x.CreateMap<ApplicationUser, ApplicationUserUpdateInfoModel>().ReverseMap();    
                    x.CreateMap<ApplicationUserModelRegister, ApplicationUser>()
                        .ForMember(dest => dest.ApplicationUserUserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.ApplicationUserEmail, opt => opt.MapFrom(src => src.Email))
                        .ForMember(dest => dest.ApplicationUserPasswordHash, opt => opt.MapFrom(src => src.Password)) 
                        .ForMember(dest => dest.ApplicationUserRole, opt => opt.MapFrom(src => (int)src.UserRole));
                    x.CreateMap<ApplicationUserModelLogin, ApplicationUser>()
                        .ForMember(dest => dest.ApplicationUserEmail, opt => opt.MapFrom(src => src.ApplicationUserEmail))
                        .ForMember(dest => dest.ApplicationUserPasswordHash, opt => opt.MapFrom(src => src.Password));
                #endregion

                #region
                    x.CreateMap<BusinessCard, BusinessCardModel>().ReverseMap();
                    x.CreateMap<BusinessCard, BusinessCardModelId>().ReverseMap();
                    x.CreateMap<BusinessCard, BusinessCardModelCreate>().ReverseMap();
                    x.CreateMap<BusinessCard, BusinessCardModelUpdate>().ReverseMap();
                #endregion
            });
            return MapConfig.CreateMapper();
        }
    }
}
