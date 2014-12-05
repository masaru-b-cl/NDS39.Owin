using System.Diagnostics;

using Owin;
using Microsoft.Owin;

using NDS39.Owin.Middleware;

[assembly: OwinStartup(typeof(NDS39.Owin.Web.Startup))]

namespace NDS39.Owin.Web
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      app.Use<MinimalMiddleware>();

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
