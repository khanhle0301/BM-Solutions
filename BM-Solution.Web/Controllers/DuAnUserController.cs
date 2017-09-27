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
    [Authorize]
    [RoutePrefix("api/duAnUser")]
    public class DuAnUserController : ApiControllerBase
    {
        private readonly IDuAnService _duAnService;
        private readonly IDuAnUserService _duAnUserService;

        public DuAnUserController(IErrorService errorService, IDuAnUserService duAnUserService,
            IDuAnService duAnService)
            : base(errorService)
        {
            _duAnUserService = duAnUserService;
            _duAnService = duAnService;
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, DuAnUserViewModel duAnUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var newDuAnUser = new DuAnUser();
                newDuAnUser.UpdateDuAnUser(duAnUserViewModel);
                newDuAnUser.UserId = AppUserManager.FindByName(duAnUserViewModel.UserId).Id;
                try
                {
                    _duAnUserService.Add(newDuAnUser);
                    _duAnUserService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.OK, duAnUserViewModel);
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
        public HttpResponseMessage Update(HttpRequestMessage request, DuAnUserViewModel duAnUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var newDuAnUser = new DuAnUser();
                newDuAnUser.UpdateDuAnUser(duAnUserViewModel);
                newDuAnUser.UserId = AppUserManager.FindByName(duAnUserViewModel.UserId).Id;
                try
                {
                    _duAnUserService.Update(newDuAnUser);
                    _duAnUserService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.OK, duAnUserViewModel);
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
        public HttpResponseMessage Delete(HttpRequestMessage request, string duAnId, string userId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // var _userId = AppUserManager.FindByName(userId).Id;
                    _duAnUserService.DeleteAll(duAnId, userId);
                    _duAnUserService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.OK);
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