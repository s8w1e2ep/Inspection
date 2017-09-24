using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InspectionWeb
{
    public static class HTMLHelperExtensions
    {
        public static string isActive(this HtmlHelper html, string controller = null, string controller2 = null, string controller3 = null, string action = null)
        {
            string activeClass = "active"; // change here if you another name to activate sidebar items
            // detect current app state
            string actualAction = (string)html.ViewContext.RouteData.Values["action"];
            string actualController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = actualController;

            if (String.IsNullOrEmpty(controller2))
                controller2 = "";

            if (String.IsNullOrEmpty(controller3))
                controller3 = "";

            if (String.IsNullOrEmpty(action))
                action = actualAction;

            string result = ((controller == actualController || controller2 == actualController || controller3 == actualController) && action == actualAction && action != "Login") ? activeClass : String.Empty;

            return result;
        }

        
    }
}