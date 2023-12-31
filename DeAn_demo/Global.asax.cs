﻿using DeAn_demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DeAn_demo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //Tao moi Session
        protected void Session_Start()
        {
            Session["UserId"] = "1";
            Session["MyCart"] = "";//khoi tao gia tri = NULL
            Session["UserCustomer"] = "";
        }

    }
}