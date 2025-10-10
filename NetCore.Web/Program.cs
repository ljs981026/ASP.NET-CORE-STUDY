using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCore.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostbuilder = CreateHostBuilder(args).Build();
            // 애플리케이션이 실행될 때 자동으로 데이터가 생성되게끔 설정
            using (var scope = hostbuilder.Services.CreateScope())
            {
                DBFirstDbInitializer initializer = scope.ServiceProvider
                    .GetService<DBFirstDbInitializer>();

                int rowAffected = initializer.PlantSeedData();
            }

            hostbuilder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
