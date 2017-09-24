using AutoMapper;
using BM_Solution.Model.Models;
using BM_Solution.Web.Infrastructure.Core;
using BM_Solution.Web.Infrastructure.Extensions;
using BM_Solution.Web.Models;
using BM_Solution.Web.Models.System;
using BM_Solution.Web.Providers;
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
    [RoutePrefix("api/chitietthuchi")]
    [Authorize]
    public class ChiTietThuChiController : ApiControllerBase
    {
        private readonly ISystemLogService _systemLogService;
        private readonly IChiTietThuChiService _chiTietThuChiService;
        private readonly IDuAnService _duAnService;

        public ChiTietThuChiController(IErrorService errorService,
            IChiTietThuChiService chiTietThuChiService, IDuAnService duAnService,
            ISystemLogService systemLogService)
            : base(errorService)
        {
            _chiTietThuChiService = chiTietThuChiService;
            _duAnService = duAnService;
            _systemLogService = systemLogService;
        }

        [Route("getbyduanid")]
        [HttpGet]
        public HttpResponseMessage GetByDuAnId(HttpRequestMessage request, string duAnId, string startDate,
            string endDate, int page, int pageSize)
        {
            try
            {
                return CreateHttpResponse(request, () =>
                {
                    var query = _chiTietThuChiService.GetByDuAnId(duAnId, startDate, endDate);
                    var chiTietThuChis = query as ChiTietThuChi[] ?? query.ToArray();

                    //List<ChiTietListViewModel> listChitiet = new List<ChiTietListViewModel>();
                    //foreach (var item in chiTietThuChis)
                    //{
                    //    ChiTietListViewModel chitiet = new ChiTietListViewModel
                    //    {
                    //        Id = item.Id,
                    //        UserId = item.UserId,
                    //        DuAnId = item.DuAnId,
                    //        NgayTao = item.NgayTao,
                    //        TienChi = item.TienChi,
                    //        TienThu = item.TienThu,
                    //        IsDelete = item.IsDelete,
                    //        MoreImages = (item.MoreImages == null) ? new List<string>() : new JavaScriptSerializer().Deserialize<List<string>>(item.MoreImages),
                    //        AppUser = item.AppUser
                    //    };
                    //    listChitiet.Add(chitiet);
                    //}



                    var totalRow = chiTietThuChis.Count();
                    var model = chiTietThuChis.OrderByDescending(x => x.NgayTao).Skip(page * pageSize).Take(pageSize);
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
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, ChiTietThuChiViewModel chiTietThuChiViewModel)
        {
            if (ModelState.IsValid)
            {
                var newChiTietThuChi = new ChiTietThuChi();
                newChiTietThuChi.UpdateApplicationChiTietThuChi(chiTietThuChiViewModel);
                newChiTietThuChi.UserId = User.Identity.GetUserId();
                try
                {
                    _chiTietThuChiService.Add(newChiTietThuChi);
                    _duAnService.UpdateProfit(newChiTietThuChi);
                    _systemLogService.Create(new SystemLog
                    {
                        Id = 0,
                        User = User.Identity.Name,
                        IsDelete = false,
                        NgayTao = DateTime.Now,
                        NoiDung = "Thêm giao dịch (tiền thu: " + newChiTietThuChi.TienThu + ", tiền chi " + newChiTietThuChi.TienChi
                    });
                    _chiTietThuChiService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, chiTietThuChiViewModel);
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [Route("deletemulti")]
        [Permission(Role = "Admin")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedList)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listId = new JavaScriptSerializer().Deserialize<List<int>>(checkedList);
                    foreach (var item in listId)
                    {
                        _chiTietThuChiService.Delete(item);
                    }
                    _duAnService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listId.Count);
                }

                return response;
            });
        }

        [Route("nhatkygiaodich")]
        [HttpGet]
        public HttpResponseMessage NhatKyGiaoDich(HttpRequestMessage request, string startDate, string endDate,
            int page, int pageSize)
        {
            try
            {
                return CreateHttpResponse(request, () =>
                {
                    var roles = ((ClaimsIdentity)User.Identity).Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
                    var userId = User.Identity.GetUserId();
                    var query = _chiTietThuChiService.NhatKyGiaoDich(roles, userId, startDate, endDate);
                    var chiTietThuChis = query as ChiTietThuChi[] ?? query.ToArray();
                    var totalRow = chiTietThuChis.Count();

                    var model = chiTietThuChis.OrderByDescending(x => x.NgayTao).Skip(page * pageSize).Take(pageSize);

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
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}