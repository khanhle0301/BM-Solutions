using BM_Solutions.Common.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace BM_Solution.Web.Providers
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        public string Role;

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            else
            {
                var roles = JsonConvert.DeserializeObject<List<string>>(principal.FindFirst("roles").Value);
                if (roles.Count > 0)
                {
                    if (!roles.Contains(RoleEnum.Admin.ToString()))
                    {
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    }
                }
                else
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                }
            }
        }
    }
}