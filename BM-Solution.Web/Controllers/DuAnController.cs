using AutoMapper;
using BM_Solution.Model.Models;
using BM_Solution.Web.Infrastructure.Core;
using BM_Solution.Web.Infrastructure.Extensions;
using BM_Solution.Web.Models;
using BM_Solution.Web.Models.System;
using BM_Solutions.Common.Exceptions;
using BM_Solutions.Service;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BM_Solution.Web.Controllers
{
    [RoutePrefix("api/duan")]
    [Authorize]
    public class DuAnController : ApiControllerBase
    {
        private readonly IDuAnService _duAnService;

        public DuAnController(IErrorService errorService, IDuAnService duAnService) : base(errorService)
        {
            _duAnService = duAnService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                var query = _duAnService.GetAll(filter);

                var totalRow = query.Count();

                var model = query.OrderBy(x => x.Id).Skip(page * pageSize).Take(pageSize);

                IEnumerable<DuAnListViewModel> modelVm = Mapper.Map<IEnumerable<DuAn>, IEnumerable<DuAnListViewModel>>(model);

                PaginationSet<DuAnListViewModel> pagedSet = new PaginationSet<DuAnListViewModel>()
                {
                    Items = modelVm,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                var response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = AppRoleManager.Roles.ToList();
                IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<AppRole>, IEnumerable<ApplicationRoleViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị.");
            }
            DuAn duAn = _duAnService.GetById(id);
            if (duAn == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không tìm thấy dự án");
            }
            DuAnViewModel modelVm = Mapper.Map<DuAn, DuAnViewModel>(duAn);
            return request.CreateResponse(HttpStatusCode.OK, modelVm);
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, DuAnViewModel duAnViewModel)
        {
            if (ModelState.IsValid)
            {
                var newDuAn = new DuAn();
                newDuAn.UpdateDuAn(duAnViewModel);
                try
                {
                    _duAnService.Add(newDuAn);
                    _duAnService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, duAnViewModel);
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }

            }
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var appRole = AppRoleManager.FindById(applicationRoleViewModel.Id);
                try
                {
                    appRole.UpdateApplicationRole(applicationRoleViewModel, "update");
                    AppRoleManager.Update(appRole);
                    return request.CreateResponse(HttpStatusCode.OK, appRole);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            var appRole = AppRoleManager.FindById(id);
            AppRoleManager.Delete(appRole);
            return request.CreateResponse(HttpStatusCode.OK, id);
        }
    }
}