using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Owin;
using Microsoft.Owin;
using System.Security.Claims;

namespace NDS39.Owin.Middleware
{
  public delegate bool AuthenticateDelegate(string user, string password);

  public static class AuthBasicExtensions
  {
    public static void UseAuthBasic(this IAppBuilder app, AuthenticateDelegate authenticate)
    {
      app.Use<AuthBasicMiddleware>(authenticate);
    }
  }

  public class AuthBasicMiddleware : OwinMiddleware
  {
    public AuthBasicMiddleware(OwinMiddleware next) : base(next) { }

    private readonly AuthenticateDelegate authenticate;

    public AuthBasicMiddleware(OwinMiddleware next, AuthenticateDelegate authenticate)
      : base(next)
    {
      this.authenticate = authenticate;
    }

    public override async Task Invoke(IOwinContext context)
    {
      var request = context.Request;

      var authorization = request.Headers["Authorization"];

      if (string.IsNullOrEmpty(authorization))
      {
        Unauthorized(context);
        return;
      }

      if (authorization.StartsWith("Basic") == false)
      {
        Unauthorized(context);
        return;
      }

      var base64Parameter = authorization.Replace("Basic ", "");
      var parameter = Encoding.UTF8.GetString(Convert.FromBase64String(base64Parameter));
      var groups = new Regex("(?<user>.*):(?<password>.*)").Match(parameter).Groups;
      var user = groups["user"].Value;
      var password = groups["password"].Value;

      if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
      {
        Unauthorized(context);
        return;
      }

      if (authenticate(user, password) == false)
      {
        Unauthorized(context);
        return;
      }

      context.Request.User = new ClaimsPrincipal(
        new ClaimsIdentity(
          new[] { new Claim(ClaimTypes.Name, user) },
          "Basic"
        )
      );

      await Next.Invoke(context);
    }

    private static void Unauthorized(IOwinContext context)
    {
      var response = context.Response;
      response.StatusCode = 401;
      response.Headers["WWW-Authenticate"] = @"Basic realm=""NDS39""";
    }
  }
}