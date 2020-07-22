using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MovieStore.MVC
{
    public class Program
    {//the starting point of asp.net is the main method in program.cs
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        //created a server called Kestral Server...
        //ASP.NET Core Cross Platform-- MAC/Windows/Linux
        //previously ASP.NET Framework was just in windows
        //interview question:
        //if you see exception, the first thing is to look at starups
        //starting point of the http request is browser, the end point is giving back result to the server
    }
    //what is the difference between int?x an int x
    //if you dont assign anything values to them, the fisrt x is null and the latter is 0

}
