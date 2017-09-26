using BM_Solution.Model.Models;
using BM_Solution.Web.Infrastructure.Core;
using BM_Solution.Web.Infrastructure.Extensions;
using BM_Solution.Web.Models;
using BM_Solutions.Service;
using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BM_Solution.Web.Controllers
{
    [RoutePrefix("api/tienvonbandau")]
    [Authorize]
    public class TienVonBanDauController : ApiControllerBase
    {
        private readonly ITienVonBanDauService _tienVonBanDauService;

        public TienVonBanDauController(IErrorService errorService, ITienVonBanDauService tienVonBanDauService)
            : base(errorService)
        {
            _tienVonBanDauService = tienVonBanDauService;
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, TienVonBanDauViewModel tienVonBanDauViewModel)
        {
            if (ModelState.IsValid)
            {
                var newTienVonBanDau = new TienVonBanDau();
                newTienVonBanDau.UpdateTienVonBanDau(tienVonBanDauViewModel);
                newTienVonBanDau.UserId = User.Identity.GetUserId();
                try
                {
                    _tienVonBanDauService.Create(newTienVonBanDau);
                    _tienVonBanDauService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, tienVonBanDauViewModel);
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
    }
}