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
      // Basic authentication
      app.UseAuthBasic((user, password) => user == password);

      app.Run(async context =>
        {
          var response = context.Response;
          response.StatusCode = 200;
          response.ContentType = "text/plain";
          await response.WriteAsync(string.Format("Hello {0}", context.Request.User.Identity.Name));
        });
    }
  }
}
