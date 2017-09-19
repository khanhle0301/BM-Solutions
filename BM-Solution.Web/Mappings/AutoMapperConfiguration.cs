﻿using AutoMapper;
using BM_Solution.Model.Models;
using BM_Solution.Web.Models;
using BM_Solution.Web.Models.System;

namespace BM_Solution.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AppRole, ApplicationRoleViewModel>();
                cfg.CreateMap<AppUser, AppUserViewModel>();
                cfg.CreateMap<Permission, PermissionViewModel>();
                cfg.CreateMap<DuAn, DuAnViewModel>();
                cfg.CreateMap<DuAn, DuAnListViewModel>();
            });
        }
    }
}