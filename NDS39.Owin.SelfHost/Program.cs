using System;
using System.Linq;
using System.Threading;

using Microsoft.Owin.Hosting;

using NDS39.Owin.App;

namespace NDS39.Owin.SelfHost
{
  class Program
  {
    private static ManualResetEvent quitEvent = new ManualResetEvent(false);

    static void Main(string[] args)
    {
      Console.CancelKeyPress += (sender, e) =>
      {
        quitEvent.Set();
        e.Cancel = true;
      };

      var port = 9000;
      if (args.Any())
      {
        int.TryParse(args[0], out port);
      }

      var url = String.Format("http://+:{0}", port);
      using (WebApp.Start<Startup>(url))
      {
        Console.WriteLine("Started");
        quitEvent.WaitOne();
      }
    }
  }
}
