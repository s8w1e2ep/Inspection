using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InspectionWeb.Models
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public bool Normal { get; set; }
        public bool Deployer { get; set; }
        public bool Manager { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");

            }

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            //取得使用角色
            FormsIdentity id = httpContext.User.Identity as FormsIdentity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string currentRoles = ticket.UserData;

            if (Normal == true)
            {
                if (currentRoles.Contains("Normal"))
                {
                    return true;
                }
            }

            if (Deployer == true)
            {
                if (currentRoles.Contains("Deployer"))
                {
                    return true;
                }
            }

            if (Manager == true)
            {
                if (currentRoles.Contains("Manager"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}