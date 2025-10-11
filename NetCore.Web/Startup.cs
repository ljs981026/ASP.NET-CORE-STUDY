using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.Services.Interfaces;
using NetCore.Services.Svcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCore.Services.Data;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using NetCore.Utilities.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;

namespace NetCore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            //Common.SetDataProtection(services, @"/Users/jaeseok/DataProtector/", "NetCore", Enums.CryptoType.Managed);
            Common.SetDataProtection(services, @"D:\Lecture\ASP.NET-CORE-STUDY\DataProtector\", "NetCore", Enums.CryptoType.CngCbc);
            // ������ ������ ����ϱ� ���ؼ� ���񽺷� �����
            // 껍데기          내용물
            // IUser 인터페이스 UserService 클래스를 받기 위해 services에 등록해야 함
            services.AddScoped<DBFirstDbInitializer>();
            services.AddScoped<IUser, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            // db 접속정보, 마이그레이션스 프로젝트 지정
            //services.AddDbContext<CodeFirstDbContext>(options =>
            //    options.UseSqlServer(connectionString: Configuration.GetConnectionString(name: "DefaultConnection"),
            //        sqlServerOptionsAction: mig => mig.MigrationsAssembly(assemblyName: "NetCore.Services")));

            services.AddDbContext<DBFirstDbContext>(options =>
                options.UseSqlServer(connectionString: Configuration.GetConnectionString(name: "DBFirstDBConnection")
                ));

            // Logging
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection(key: "Logging"));
                builder.AddConsole();
                builder.AddDebug();
            });

            services.AddControllersWithViews();

            // 신원보증과 승인권한
            services.AddAuthentication(defaultScheme: CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(cook => {
                    cook.AccessDeniedPath = "/Membership/Forbidden";
                    cook.LoginPath = "/Membership/Login";
                });

            services.AddAuthorization();

            // 세션을 저장할 캐쉬메모리 등록
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = ".NetCore.Session";
                // 세션의 제한시간
                options.IdleTimeout = TimeSpan.FromMinutes(30); // 기본값은 20분
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // 신원보증만
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            // 세션 지정
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
