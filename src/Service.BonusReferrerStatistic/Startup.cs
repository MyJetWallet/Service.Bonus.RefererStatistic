using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using MyJetWallet.Sdk.GrpcSchema;
using MyJetWallet.Sdk.Postgres;
using MyJetWallet.Sdk.Service;
using Service.BonusRefererStatistic.Postgres;
using Service.BonusReferrerStatistic.Grpc;
using Service.BonusReferrerStatistic.Modules;
using Service.BonusReferrerStatistic.Services;

namespace Service.BonusReferrerStatistic
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureJetWallet<ApplicationLifetimeManager>(Program.Settings.ZipkinUrl);
            DatabaseContext.LoggerFactory = Program.LogFactory;
            services.AddDatabase(DatabaseContext.Schema, Program.Settings.PostgresConnectionString,
                o => new DatabaseContext(o));
            DatabaseContext.LoggerFactory = null;    }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.ConfigureJetWallet(env, endpoints =>
            {
                endpoints.MapGrpcSchema<ReferrerStatService, IReferrerStatService>();

            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigureJetWallet();
            builder.RegisterModule<SettingsModule>();
            builder.RegisterModule<ServiceModule>();
        }
    }
}
