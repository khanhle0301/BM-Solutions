﻿using AutoMapper;
using BM_Solution.Model.Models;
using BM_Solution.Web.Infrastructure.Core;
using BM_Solution.Web.Models.System;
using BM_Solutions.Service;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BM_Solution.Web.Providers;
using System.Threading.Tasks;

namespace BM_Solution.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/appUser")]
    public class AppUserController : ApiControllerBase
    {
        public AppUserController(IErrorService errorService)
            : base(errorService)
        {
        }

        [Route("getlistpaging")]
        [Permission(Action = "Read", Function = "USER")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = AppUserManager.Users;
                IEnumerable<AppUserViewModel> modelVm =
                    Mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, model);

                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        [Permission(Action = "Read", Function = "USER")]
        public async Task<HttpResponseMessage> Details(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị.");
            }
            var user = await AppUserManager.FindByIdAsync(id);
            if (user == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có dữ liệu");
            }
            else
            {
                var roles = await AppUserManager.GetRolesAsync(user.Id);
                var applicationUserViewModel = Mapper.Map<AppUser, AppUserViewModel>(user);
                applicationUserViewModel.Roles = roles;
                return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
            }
        }

        [HttpPost]
        [Route("add")]
        [Permission(Action = "Create", Function = "USER")]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, AppUserViewModel appUserViewModel)
        {
            if (string.IsNullOrEmpty(appUserViewModel.Id))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(appUserViewModel.Id) + " không có giá trị.");
            }
            var user = await AppUserManager.FindByIdAsync(appUserViewModel.Id);
            if (user == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có dữ liệu");
            }
            else
            {
                var roles = await AppUserManager.GetRolesAsync(user.Id);
                var applicationUserViewModel = Mapper.Map<AppUser, AppUserViewModel>(user);
                applicationUserViewModel.Roles = roles;
                return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
            }
        }
    }
}