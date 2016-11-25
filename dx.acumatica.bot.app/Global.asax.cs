
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using dx.acumatica.bot.app.Modules;
using Microsoft.Bot.Builder.Dialogs.Internals;

namespace dx.acumatica.bot.app
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            {
                // http://docs.autofac.org/en/latest/integration/webapi.html#quick-start
                var builder = new ContainerBuilder();

                // register the Bot Builder module
                builder.RegisterModule(new DialogModule());
                // register the alarm dependencies
                builder.RegisterModule(new OpprtunitiesModule());
                var config = GlobalConfiguration.Configuration;

                // Register your Web API controllers.
                builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

                // OPTIONAL: Register the Autofac filter provider.
                builder.RegisterWebApiFilterProvider(config);

                // Set the dependency resolver to be Autofac.
                var container = builder.Build();
                config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            }

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
