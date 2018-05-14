using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AdminUI.Common
{
    public class WelrsAuthorize : AuthorizeAttribute
    {

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    var action = filterContext.ActionDescriptor;
        //    //if (action.IsDefined(typeof(OverrideAuthorizeAttribute), true)) return;

        //    base.OnAuthorization(filterContext);
        //}

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //if not logged, it will work as normal Authorize and redirect to the Login
                // base.HandleUnauthorizedRequest(filterContext);

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "NotAuthorized" }));

            }
            else
            {
                //logged and wihout the role to access it - redirect to the custom controller action
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "NotAuthorized" }));
            }
        }
    }
}