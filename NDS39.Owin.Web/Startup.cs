using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NDS39.Owin.Web.Startup))]

namespace NDS39.Owin.Web
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      app.Run(async context =>
        {
          var response = context.Response;
          response.StatusCode = 200;
          response.ContentType = "text/plain";
          await response.WriteAsync("Hello world");
        });
    }
  }
}
