using BM_Solution.Model.Models;
using BM_Solution.Web.Infrastructure.Core;
using BM_Solution.Web.Providers;
using BM_Solutions.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace BM_Solution.Web.Controllers
{
    [RoutePrefix("api/systemlog")]
    public class SystemLogController : ApiControllerBase
    {
        private readonly ISystemLogService _systemLogService;

        public SystemLogController(IErrorService errorService, ISystemLogService systemLogService)
            : base(errorService)
        {
            _systemLogService = systemLogService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        [Permission(Role = "Admin")]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize,
            string startDate, string endDate)
        {
            try
            {
                return CreateHttpResponse(request, () =>
                {
                    var query = _systemLogService.NhatKyHeThong(startDate, endDate);
                    var systemLogs = query as SystemLog[] ?? query.ToArray();
                    var totalRow = systemLogs.Count();
                    var model = systemLogs.OrderByDescending(x => x.NgayTao).Skip(page * pageSize).Take(pageSize);
                    PaginationSet<SystemLog> pagedSet = new PaginationSet<SystemLog>()
                    {
                        Items = model,
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
                        _systemLogService.Delete(item);
                    }
                    _systemLogService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listId.Count);
                }

                return response;
            });
        }
    }
}