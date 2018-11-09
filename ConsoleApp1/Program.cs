using System;
using System.IO;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddNodeServices(options => {
                options.LaunchWithDebugging = false;                
                options.ProjectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "js");
            });

            //var js = @"
            //        module.exports = function (callback, first, second) {
            //            var result = first + second;
            //            callback(/* error */ null, result);
            //        };
            //     ";
            var serviceProvider = services.BuildServiceProvider();
            var nodeServices = serviceProvider.GetRequiredService<INodeServices>();
            for (int i = 0; i < 100; i++)
            {
                var result = nodeServices.InvokeAsync<int>("./addNumbers", 1, 2).Result;
            }
            Console.WriteLine("Hello World!");
        }
    }
}
