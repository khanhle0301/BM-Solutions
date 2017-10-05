using AutoMapper;
using BM_Solution.Model.Models;
using BM_Solution.Web.Infrastructure.Core;
using BM_Solution.Web.Infrastructure.Extensions;
using BM_Solution.Web.Models;
using BM_Solution.Web.Providers;
using BM_Solutions.Common.Enums;
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

namespace BM_Solution.Web.Controllers
{
    [RoutePrefix("api/duan")]
    [Authorize]
    public class DuAnController : ApiControllerBase
    {
        private readonly ISystemLogService _systemLogService;
        private readonly IDuAnService _duAnService;
        private readonly IDuAnUserService _duAnUserService;
        private readonly IChiTietThuChiService _chiTietThuChiService;

        public DuAnController(IErrorService errorService, IDuAnService duAnService,
            IDuAnUserService duAnUserService, IChiTietThuChiService chiTietThuChiService,
            ISystemLogService systemLogService)
            : base(errorService)
        {
            _duAnService = duAnService;
            _duAnUserService = duAnUserService;
            _chiTietThuChiService = chiTietThuChiService;
            _systemLogService = systemLogService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize,
            string status, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                var listStatus = new JavaScriptSerializer().Deserialize<List<StatusEnum>>(status);
                // lấy danh sách role
                var roles = ((ClaimsIdentity)User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);
                // lấy id user login
                var userId = User.Identity.GetUserId();
                // get dự án theo user
                var query = _duAnService.GetByUserId(userId, filter, roles, listStatus);
                var duAns = query as DuAn[] ?? query.ToArray();
                // tổng dự án
                var totalRow = duAns.Count();
                // lấy theo phân trang
                var model = duAns.OrderBy(x => x.TrangThai).Skip(page * pageSize).Take(pageSize);
                // map qua view model
                IEnumerable<DuAnViewModel> modelVm = Mapper.Map<IEnumerable<DuAn>, IEnumerable<DuAnViewModel>>(model);
                // PaginationSet
                PaginationSet<DuAnViewModel> pagedSet = new PaginationSet<DuAnViewModel>()
                {
                    Items = modelVm,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                // response trả về
                return request.CreateResponse(HttpStatusCode.OK, pagedSet);
            });
        }

        [Route("getListString")]
        [HttpGet]
        public HttpResponseMessage GetListString(HttpRequestMessage request)
        {
            // lấy mã danh sách dự án
            return CreateHttpResponse(request, () =>
            {
                var model = _duAnService.GetAll().Select(x => x.Id);
                return request.CreateResponse(HttpStatusCode.OK, model);
            });
        }

        [Route("getlistall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            // lấy tất cả dự án
            return CreateHttpResponse(request, () =>
            {
                var model = _duAnService.GetAll();
                IEnumerable<DuAnViewModel> modelVm = Mapper.Map<IEnumerable<DuAn>, IEnumerable<DuAnViewModel>>(model);
                return request.CreateResponse(HttpStatusCode.OK, modelVm);
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            // không có id dự án
            if (string.IsNullOrEmpty(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị.");
            }
            // get dự án theo id
            DuAn duAn = _duAnService.GetById(id);
            // k có dự án
            if (duAn == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không tìm thấy dự án");
            }
            // get user nằm trong dự án theo id dự án
            var duAnUser = _duAnUserService.GetDuAnUserByDuAnId(id);
            // map qua view model
            DuAnViewModel modelVm = Mapper.Map<DuAn, DuAnViewModel>(duAn);
            modelVm.DuAnUserViewModels = Mapper.Map<IEnumerable<DuAnUser>, IEnumerable<DuAnUserViewModel>>(duAnUser);
            // response trả về
            return request.CreateResponse(HttpStatusCode.OK, modelVm);
        }

        [HttpPost]
        [Route("add")]
        [Permission(Role = "Admin")]
        public HttpResponseMessage Create(HttpRequestMessage request, DuAnViewModel duAnViewModel)
        {
            // kiểm tra validate dữ liệu
            if (ModelState.IsValid)
            {
                // tạo mới dự án
                var newDuAn = new DuAn();
                newDuAn.UpdateDuAn(duAnViewModel);
                try
                {
                    // add dự án
                    _duAnService.Add(newDuAn);
                    _duAnService.Save();
                    // lặp qua danh sách user
                    foreach (var item in duAnViewModel.DuAnUserViewModels)
                    {
                        // tạo mới dự án user
                        var duAnUser = new DuAnUser
                        {
                            UserId = AppUserManager.FindByName(item.UserId).Id,
                            DuAnId = duAnViewModel.Id,
                            TienVonBanDau = item.TienVonBanDau,
                            IsDelete = false,
                            NgayTao = item.NgayTao,
                            PhanTramHoaHong = item.PhanTramHoaHong,
                            PhanTramVon = item.PhanTramVon
                        };
                        // add dự án user
                        _duAnUserService.Add(duAnUser);
                    }
                    // ghi lại nhật ký hệ thống
                    _systemLogService.Create(new SystemLog
                    {
                        Id = 0,
                        User = User.Identity.Name,
                        IsDelete = false,
                        NgayTao = DateTime.Now,
                        NoiDung = "Thêm dự án: " + newDuAn.Id
                    });
                    // lưu vào db
                    _duAnUserService.SaveChange();
                    // trả về response
                    return request.CreateResponse(HttpStatusCode.OK, duAnViewModel);
                }
                // Exception
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            // lỗi validate dữ liệu
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, DuAnViewModel duAnViewModel)
        {
            // kiểm tra validate dữ liệu
            if (ModelState.IsValid)
            {
                // tạo mới dự án
                var newDuAn = new DuAn();
                newDuAn.UpdateDuAn(duAnViewModel);
                try
                {
                    // cập nhật lại dự ắn
                    _duAnService.Update(newDuAn);
                    // lưu lại hệ thống
                    _systemLogService.Create(new SystemLog
                    {
                        Id = 0,
                        User = User.Identity.Name,
                        IsDelete = false,
                        NgayTao = DateTime.Now,
                        NoiDung = "Cập nhật dự án: " + duAnViewModel.Id
                    });
                    // lưu vào db
                    _duAnService.Save();
                    // trả về response
                    return request.CreateResponse(HttpStatusCode.OK, duAnViewModel);
                }
                // Exception
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            // lỗi validate dự liệu
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpDelete]
        [Permission(Role = "Admin")]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            // xóa dự án theo id
            _duAnService.Delete(id);
            // lưu lại hệ thống
            _systemLogService.Create(new SystemLog
            {
                Id = 0,
                User = User.Identity.Name,
                IsDelete = false,
                NgayTao = DateTime.Now,
                NoiDung = "Xóa dự án: " + id
            });
            // lưu vào db
            _duAnService.Save();
            // trả về response
            return request.CreateResponse(HttpStatusCode.OK, id);
        }

        [HttpDelete]
        [Route("ketthuc")]
        public HttpResponseMessage KetThuc(HttpRequestMessage request, string id)
        {
            // kết thúc dự án
            _duAnService.KetThucDuAn(id);
            // ghi lại hệ thống
            _systemLogService.Create(new SystemLog
            {
                Id = 0,
                User = User.Identity.Name,
                IsDelete = false,
                NgayTao = DateTime.Now,
                NoiDung = "Kết thúc dự án: " + id
            });
            // lưu vào db
            _duAnService.Save();
            // trả về response
            return request.CreateResponse(HttpStatusCode.OK, id);
        }

        [Route("deletemulti")]
        [Permission(Role = "Admin")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedList)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                // validate dầu vào
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    // lấy ra dánh sách id dự án
                    var listId = new JavaScriptSerializer().Deserialize<List<string>>(checkedList);
                    // lặp qa dánh sách
                    foreach (var item in listId)
                    {
                        // xóa dự án theo id
                        _duAnService.Delete(item);
                        // ghi lại hệ thống
                        _systemLogService.Create(new SystemLog
                        {
                            Id = 0,
                            User = User.Identity.Name,
                            IsDelete = false,
                            NgayTao = DateTime.Now,
                            NoiDung = "Xóa dự án: " + item
                        });
                    }
                    // lưu vào db
                    _duAnService.Save();
                    // trả về response
                    response = request.CreateResponse(HttpStatusCode.OK, listId.Count);
                }
                // trả về response
                return response;
            });
        }
    }
}