using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.Owin;

namespace NDS39.Owin.Middleware
{
  public class MinimalMiddleware : OwinMiddleware
  {
    public MinimalMiddleware(OwinMiddleware next) : base(next) { }

    public override async Task Invoke(IOwinContext context)
    {
      Debug.WriteLine("@@@ start middleware @@@");
      await Next.Invoke(context);
      Debug.WriteLine("@@@ finish middleware @@@");
    }
  }
}