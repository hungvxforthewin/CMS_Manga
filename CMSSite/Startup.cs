using System;
using CRMBussiness;
using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMSite.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Globalization;
using CRMSite.Models;
using CRMSite.Providers;
using Microsoft.Extensions.Logging;
using Hangfire;
using CMSBussiness.IService;
using CMSBussiness.ServiceImp;

namespace CRMSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var connectionString = configuration["connectionString"].ToString();
            OpenDapper.ConnectStr(connectionString);
            Configuration = configuration;
            Environment = env;
            //Configuration = configuration;
        }

        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            var mailSettings = Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailSettings);

            // Đăng ký SendMailService với kiểu Transient, mỗi lần gọi dịch
            // vụ ISendMailService một đới tượng SendMailService tạo ra (đã inject config)
            services.AddTransient<SendMailService, SendMailService>();

            //HungVX ADD SERVICE
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index";
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    //options.AccessDeniedPath = "/User/Forbidden/";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = Environment.IsDevelopment()
                      ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                });


            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.None;
                options.Secure = Environment.IsDevelopment()
                  ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
            });
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddHttpContextAccessor();
            services.AddRazorPages();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //HungVX ADD SERVICE
            //services.AddControllersWithViews();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddControllers();
            services.AddSignalR();
            services.AddTransient<IAccount, AccountImp>();
            //services.AddTransient<ICategory, CategoryImp>();
            services.AddTransient<IContractStaff, ContractStaffImp>();
            services.AddTransient<IKpiSaleManager, KpiSaleManagerImp>();
            services.AddTransient<IInvestor, InvestorImp>();
            services.AddTransient<IDeposit, DepositImp>();
            services.AddTransient<IStatusContractInvestors, StatusContractInvestorsImp>();
            services.AddTransient<IContractInvester, ContractInvesterImp>();
            services.AddTransient<IContractInvestorInstallments, ContractInvestorInstallmentsImp>();
            services.AddTransient<IBranch, BranchImp>();
            services.AddTransient<IOffice, OfficeImp>();
            services.AddTransient<IDepartment, DepartmentImp>();
            services.AddTransient<ITeamInCompany, TeamInCompanyImp>();
            services.AddTransient<IIntermediaries, IntermediariesImp>();
            services.AddTransient<IRatingSale, RatingSaleIpm>();
            services.AddTransient<ILevelConcern, LevelConcernImp>();
            services.AddTransient<IInvestorsCareHistory, InvestorsCareHistoryImp>();
            services.AddTransient<IInvestorsCareHistoryDetail, InvestorsCareHistoryDetailImp>();
            services.AddTransient<IBook, BookImp>();
            services.AddTransient<ICategory, CategoryImp>();
            services.AddTransient<IBookCategory, BookCategoryImp>();
            services.AddTransient<IBookChapter, BookChapterImp>();
            services.AddTransient<IBookChapterDetail, BookChapterDetailImp>();

            services.AddTransient<IProduct, ProductIpm>();
            services.AddTransient<IEvent, EventImp>();

            services.AddTransient<IStatistic, StatisticImp>();
            services.AddTransient<ITarget, TargetImp>();


            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            //services.AddTransient<ITimeKeeping, TimeKeepingImp>();
            //services.AddSingleton<IRecurringJobs, RecurringJobs>();
            //services.AddHangfire(x => x.UseSqlServerStorage(Configuration["connectionString"]));
            //services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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

            // Log system
            var path = Directory.GetCurrentDirectory();
            DateTime day = DateTime.Now;
            loggerFactory.AddFile(Path.Combine(path, "Logs", day.Year.ToString(), day.Month.ToString(), day.Day.ToString(), "Log.txt"), retainedFileCountLimit: null);
            // Log system

            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberGroupSeparator = ",";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseHttpsRedirection();
            app.UseRequestLocalization();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseHangfireServer(new BackgroundJobServerOptions{ WorkerCount = 1});
            //GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

            //app.UseHangfireDashboard();
            //RecurringJob.AddOrUpdate<IRecurringJobs>(x => x.UpdateShare(), Cron.Monthly);

            app.UseSession();
            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value;
                // Rewrite to index
                if (url.Contains("/import-nhan-su"))
                {
                    // rewrite and continue processing
                    context.Request.Path = "/Employees/Index";
                }
                await next();
            });
            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value;
                // Rewrite to index
                if (url.Contains("/bang-xep-hang/lich-su"))
                {
                    // rewrite and continue processing
                    context.Request.Path = "/RankingSales/History";
                }
                await next();
            });
            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value;
                // Rewrite to index
                if (url.Contains("/bang-xep-hang"))
                {
                    // rewrite and continue processing
                    context.Request.Path = "/RankingSales/Index";
                }
                await next();
            });
            
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(env.WebRootPath, Configuration["PathSaveFileExport"].ToString())),
            //    RequestPath = "/Download/Report/"
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "import",
                //    "import-nhan-su",
                //    new { controller = "Employees", action = "Index" });
                //endpoints.MapControllerRoute(
                //    name: "user",
                //    pattern: "/import-nhan-su",
                //    defaults: new { controller = "Employees", action = "Index" });
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("import", "import-nhan-su", new { Controller = "Employees", Action = "Index" });

                routes.MapRoute("default", "{area:exists}/{controller=Login}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "NoArea",
                    template: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
