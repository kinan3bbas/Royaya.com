using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using Royaya.com.Models;
using Royaya.com.Extras;

namespace Royaya.com
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<InterprationPath>("InterprationPaths");
            builder.EntitySet<Attachment>("Attachments");
            builder.EntitySet<Dream>("Dreams");
            builder.EntitySet<ApplicationUser>("Users");
            builder.EntitySet<ContactUs>("ContactUs");
            builder.EntitySet<DreamComment>("DreamComments");
            builder.EntitySet<UsersDeviceTokens>("UsersDeviceTokens");
            builder.EntitySet<NotificationLog>("NotificationLogs");
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

            // Web API routes
            config.MapHttpAttributeRoutes();

            //Enable SSL
            config.Filters.Add(new RequireHttpsAttribute());


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
