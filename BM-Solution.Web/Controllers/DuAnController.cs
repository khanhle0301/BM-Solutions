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
using System.Security.Claims;
using System.Web.Http;
using System.Web.Script.Serialization;
using BM_Solution.Web.Providers;

namespace BM_Solution.Web.Controllers
{
    [RoutePrefix("api/duan")]
    [Authorize]
    public class DuAnController : ApiControllerBase
    {
        private readonly IDuAnService _duAnService;
        private readonly IDuAnUserService _duAnUserService;

        public DuAnController(IErrorService errorService, IDuAnService duAnService,
            IDuAnUserService duAnUserService) : base(errorService)
        {
            _duAnService = duAnService;
            _duAnUserService = duAnUserService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                var roles = ((ClaimsIdentity)User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);
                var userId = User.Identity.GetUserId();

                var query = _duAnService.GetByUserId(userId, filter, roles);

                var totalRow = query.Count();

                var model = query.OrderBy(x => x.TrangThai).Skip(page * pageSize).Take(pageSize);

                IEnumerable<DuAnViewModel> modelVm = Mapper.Map<IEnumerable<DuAn>, IEnumerable<DuAnViewModel>>(model);

                PaginationSet<DuAnViewModel> pagedSet = new PaginationSet<DuAnViewModel>()
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

        [Route("getListString")]
        [HttpGet]
        public HttpResponseMessage GetListString(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _duAnService.GetAll().Select(x => x.Id);
                var response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _duAnService.GetAll();
                IEnumerable<DuAnViewModel> modelVm = Mapper.Map<IEnumerable<DuAn>, IEnumerable<DuAnViewModel>>(model);
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
            var appUsers = _duAnUserService.GetUserByDuAnId(id);
            DuAnViewModel modelVm = Mapper.Map<DuAn, DuAnViewModel>(duAn);
            List<User> listAppUsers = new List<User>();
            foreach (var item in appUsers)
            {
                var user = new User { UserName = item };
                listAppUsers.Add(user);
            }
            modelVm.AppUsers = listAppUsers;
            return request.CreateResponse(HttpStatusCode.OK, modelVm);
        }

        [HttpPost]
        [Route("add")]
        [Permission(Role = "Admin")]
        public HttpResponseMessage Create(HttpRequestMessage request, DuAnViewModel duAnViewModel)
        {
            if (ModelState.IsValid)
            {
                var newDuAn = new DuAn();
                newDuAn.UpdateDuAn(duAnViewModel);
                try
                {
                    _duAnService.Add(newDuAn);
                    foreach (var item in duAnViewModel.AppUsers)
                    {
                        var duAnUser = new DuAnUser
                        {
                            UserId = AppUserManager.FindByName(item.UserName).Id,
                            DuaAnId = duAnViewModel.Id
                        };
                        _duAnUserService.Add(duAnUser);
                    }
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
        public HttpResponseMessage Update(HttpRequestMessage request, DuAnViewModel duAnViewModel)
        {
            if (ModelState.IsValid)
            {
                var newDuAn = new DuAn();
                newDuAn.UpdateDuAn(duAnViewModel);
                try
                {
                    _duAnUserService.DeleteAll(duAnViewModel.Id);
                    _duAnService.Update(newDuAn);
                    foreach (var item in duAnViewModel.AppUsers)
                    {
                        var duAnUser = new DuAnUser
                        {
                            UserId = AppUserManager.FindByName(item.UserName).Id,
                            DuaAnId = duAnViewModel.Id
                        };
                        _duAnUserService.Add(duAnUser);
                    }
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

        [HttpDelete]
        [Permission(Role = "Admin")]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            var appRole = AppRoleManager.FindById(id);
            AppRoleManager.Delete(appRole);
            return request.CreateResponse(HttpStatusCode.OK, id);
        }


        [HttpDelete]
        [Route("ketthuc")]
        public HttpResponseMessage KetThuc(HttpRequestMessage request, string id)
        {
            _duAnService.KetThucDuAn(id);
            _duAnService.Save();
            return request.CreateResponse(HttpStatusCode.OK, id);
        }

        [Route("deletemulti")]
        [Permission(Role = "Admin")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedList)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listId = new JavaScriptSerializer().Deserialize<List<string>>(checkedList);
                    foreach (var item in listId)
                    {
                        _duAnService.Delete(item);
                    }
                    _duAnService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listId.Count);
                }

                return response;
            });
        }
    }
}