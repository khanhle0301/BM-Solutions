using AutoMapper;
using BM_Solution.Model.Models;
using BM_Solution.Web.Infrastructure.Core;
using BM_Solution.Web.Infrastructure.Extensions;
using BM_Solution.Web.Models;
using BM_Solution.Web.Models.System;
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
    [RoutePrefix("api/chitietthuchi")]
    [Authorize]
    public class ChiTietThuChiController : ApiControllerBase
    {
        private readonly IChiTietThuChiService _chiTietThuChiService;

        public ChiTietThuChiController(IErrorService errorService,
            IChiTietThuChiService chiTietThuChiService) : base(errorService)
        {
            _chiTietThuChiService = chiTietThuChiService;
        }

        [Route("getbyduanid")]
        [HttpGet]
        public HttpResponseMessage GetByDuAnId(HttpRequestMessage request, string duAnId, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                var query = _chiTietThuChiService.GetByDuAnId(duAnId);

                var totalRow = query.Count();

                var model = query.OrderByDescending(x => x.NgayTao).Skip(page * pageSize).Take(pageSize);

                IEnumerable<ChiTietThuChiViewModel> modelVm = Mapper.Map<IEnumerable<ChiTietThuChi>, IEnumerable<ChiTietThuChiViewModel>>(model);

                PaginationSet<ChiTietThuChiViewModel> pagedSet = new PaginationSet<ChiTietThuChiViewModel>()
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

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppRole = new AppRole();
                newAppRole.UpdateApplicationRole(applicationRoleViewModel);
                if (AppRoleManager.Roles.Any(x => x.Name.Contains(newAppRole.Name)))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tên quyền " + newAppRole.Name + " đã tồn tại!");
                }
                try
                {
                    AppRoleManager.Create(newAppRole);
                    return request.CreateResponse(HttpStatusCode.OK, applicationRoleViewModel);
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
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