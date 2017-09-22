using AutoMapper;
using BM_Solution.Model.Models;
using BM_Solution.Web.Infrastructure.Core;
using BM_Solution.Web.Infrastructure.Extensions;
using BM_Solution.Web.Models.System;
using BM_Solutions.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BM_Solution.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/appUser")]
    public class AppUserController : ApiControllerBase
    {
        private readonly IDuAnUserService _duAnUserService;

        public AppUserController(IErrorService errorService, IDuAnUserService duAnUserService)
            : base(errorService)
        {
            _duAnUserService = duAnUserService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var query = AppUserManager.Users;
                if (!string.IsNullOrEmpty(filter))
                    query = query.Where(x => x.UserName.Contains(filter) || x.FullName.Contains(filter));
                var totalRow = query.Count();

                var model = query.OrderBy(x => x.UserName).Skip(page * pageSize).Take(pageSize);

                IEnumerable<AppUserViewModel> modelVm =
                    Mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(model);

                PaginationSet<AppUserViewModel> pagedSet = new PaginationSet<AppUserViewModel>()
                {
                    Items = modelVm,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
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
                var modelVm = Mapper.Map<AppUser, AppUserViewModel>(user);
                modelVm.Roles = (List<RoleVm>)roles;
                var duAns = _duAnUserService.GetDuAnByUserId(id);
                List<DuAnVm> listDuAns = new List<DuAnVm>();
                foreach (var item in duAns)
                {
                    var duAnVm = new DuAnVm { Id = item };
                    listDuAns.Add(duAnVm);
                }
                modelVm.DuAns = listDuAns;
                return request.CreateResponse(HttpStatusCode.OK, modelVm);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, AppUserViewModel appUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppUser = new AppUser();
                newAppUser.UpdateAppUser(appUserViewModel);
                try
                {
                    var result = await AppUserManager.CreateAsync(newAppUser, appUserViewModel.Password);
                    if (result.Succeeded)
                    {
                        List<string> roles = new List<string>();
                        appUserViewModel.Roles.ForEach(a => roles.Add(a.Name));
                        await AppUserManager.AddToRolesAsync(newAppUser.Id, roles.ToArray());
                        foreach (var item in appUserViewModel.DuAns)
                        {
                            var duAnUser = new DuAnUser
                            {
                                UserId = newAppUser.Id,
                                DuaAnId = item.Id
                            };
                            _duAnUserService.Add(duAnUser);
                        }
                        _duAnUserService.SaveChange();
                        return request.CreateResponse(HttpStatusCode.OK, appUserViewModel);
                    }
                    else
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("getListString")]
        [HttpGet]
        public HttpResponseMessage GetUser(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = AppUserManager.Users.Select(x => x.UserName);
                var response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [Route("getListStringByDuAnId")]
        [HttpGet]
        public HttpResponseMessage GetUserByDuAnId(HttpRequestMessage request, string duAnId)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _duAnUserService.GetUserByDuAnId(duAnId);
                var response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }
    }
}