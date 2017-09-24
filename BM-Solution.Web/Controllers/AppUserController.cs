using AutoMapper;
using BM_Solution.Model.Models;
using BM_Solution.Web.Infrastructure.Core;
using BM_Solution.Web.Infrastructure.Extensions;
using BM_Solution.Web.Models.System;
using BM_Solution.Web.Providers;
using BM_Solutions.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace BM_Solution.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/appUser")]
    public class AppUserController : ApiControllerBase
    {
        private readonly ISystemLogService _systemLogService;
        private readonly IDuAnUserService _duAnUserService;

        public AppUserController(IErrorService errorService, IDuAnUserService duAnUserService, ISystemLogService systemLogService)
            : base(errorService)
        {
            _duAnUserService = duAnUserService;
            _systemLogService = systemLogService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        [Permission(Role = "Admin")]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                var query = AppUserManager.Users.Where(x => x.Status);
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
                var response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        [Permission(Role = "Admin")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = AppUserManager.Users.Where(x => x.Status);
                IEnumerable<AppUserViewModel> modelVm =
                    Mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(model);
                var response = request.CreateResponse(HttpStatusCode.OK, modelVm);
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
                List<RoleVm> roleVms = new List<RoleVm>();
                foreach (var item in roles)
                {
                    var roleVm = new RoleVm
                    {
                        Name = item
                    };
                    roleVms.Add(roleVm);
                }
                var modelVm = Mapper.Map<AppUser, AppUserViewModel>(user);
                modelVm.Roles = roleVms;
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
        [Permission(Role = "Admin")]
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
                        _systemLogService.Create(new SystemLog
                        {
                            Id = 0,
                            User = User.Identity.Name,
                            IsDelete = false,
                            NgayTao = DateTime.Now,
                            NoiDung = "Thêm người dùng: " + appUserViewModel.UserName
                        });
                        _duAnUserService.SaveChange();
                        return request.CreateResponse(HttpStatusCode.OK, appUserViewModel);
                    }
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
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
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, AppUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var appUser = await AppUserManager.FindByIdAsync(applicationUserViewModel.Id);
                try
                {
                    appUser.UpdateAppUser(applicationUserViewModel, "update");
                    var oldeRoles = await AppUserManager.GetRolesAsync(appUser.Id);

                    foreach (var role in oldeRoles)
                    {
                        await AppUserManager.RemoveFromRoleAsync(appUser.Id, role);
                    }
                    var result = await AppUserManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        var duAns = _duAnUserService.GetDuAnByUserId(appUser.Id);
                        foreach (var duan in duAns)
                        {
                            _duAnUserService.DeleteAll(duan, appUser.Id);
                        }
                        List<string> roles = new List<string>();
                        applicationUserViewModel.Roles.ForEach(a => roles.Add(a.Name));
                        await AppUserManager.AddToRolesAsync(appUser.Id, roles.ToArray());
                        foreach (var item in applicationUserViewModel.DuAns)
                        {
                            var duAnUser = new DuAnUser
                            {
                                UserId = appUser.Id,
                                DuaAnId = item.Id
                            };
                            _duAnUserService.Add(duAnUser);
                        }
                        _systemLogService.Create(new SystemLog
                        {
                            Id = 0,
                            User = User.Identity.Name,
                            IsDelete = false,
                            NgayTao = DateTime.Now,
                            NoiDung = "Cập nhật người dùng: " + appUser.UserName
                        });
                        _duAnUserService.SaveChange();
                        return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
                    }
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [Route("getListString")]
        [HttpGet]
        public HttpResponseMessage GetUser(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = AppUserManager.Users.Where(x => x.Status).Select(x => x.UserName);
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

        [HttpDelete]
        [Route("delete")]
        [Permission(Role = "Admin")]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, string id)
        {
            var appUser = await AppUserManager.FindByIdAsync(id);
            appUser.Status = false;
            var result = await AppUserManager.UpdateAsync(appUser);
            if (result.Succeeded)
            {
                _systemLogService.Create(new SystemLog
                {
                    Id = 0,
                    User = User.Identity.Name,
                    IsDelete = false,
                    NgayTao = DateTime.Now,
                    NoiDung = "Xóa người dùng: " + appUser.UserName
                });
                return request.CreateResponse(HttpStatusCode.OK, id);
            }
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
        }

        [Route("deletemulti")]
        [Permission(Role = "Admin")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteMulti(HttpRequestMessage request, string checkedList)
        {
            HttpResponseMessage response;
            if (!ModelState.IsValid)
            {
                response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                var listId = new JavaScriptSerializer().Deserialize<List<string>>(checkedList);
                foreach (var item in listId)
                {
                    var appUser = await AppUserManager.FindByIdAsync(item);
                    appUser.Status = false;
                    var result = await AppUserManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        _systemLogService.Create(new SystemLog
                        {
                            Id = 0,
                            User = User.Identity.Name,
                            IsDelete = false,
                            NgayTao = DateTime.Now,
                            NoiDung = "Xóa người dùng: " + appUser.UserName
                        });
                    }
                    else
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                    }
                }
                response = request.CreateResponse(HttpStatusCode.OK, listId.Count);
            }
            // trả về response
            return response;
        }
    }
}