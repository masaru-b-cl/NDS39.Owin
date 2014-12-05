using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Diagnostics;

[assembly: OwinStartup(typeof(NDS39.Owin.Web.Startup))]

namespace NDS39.Owin.Web
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      app.Use(async (context, next) =>
        {
          Debug.WriteLine("@@@ start middleware @@@");
          await next();
          Debug.WriteLine("@@@ end middleware @@@");
        });

      app.Run(async context =>
        {
          Debug.WriteLine("@@@ run core app @@@");

          var response = context.Response;
          response.StatusCode = 200;
          response.ContentType = "text/plain";
          await response.WriteAsync("Hello world");
        });
    }
  }
}
