using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.Service;
using MyJetWallet.Sdk.ServiceBus;

namespace Service.BonusReferrerStatistic
{
    public class ApplicationLifetimeManager : ApplicationLifetimeManagerBase
    {
        private readonly ILogger<ApplicationLifetimeManager> _logger;
        private readonly ServiceBusLifeTime _serviceBusLifeTime;
        private readonly MyNoSqlClientLifeTime _sqlClientLifeTime;

        public ApplicationLifetimeManager(IHostApplicationLifetime appLifetime, ILogger<ApplicationLifetimeManager> logger, ServiceBusLifeTime serviceBusLifeTime, MyNoSqlClientLifeTime sqlClientLifeTime)
            : base(appLifetime)
        {
            _logger = logger;
            _serviceBusLifeTime = serviceBusLifeTime;
            _sqlClientLifeTime = sqlClientLifeTime;
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
            _serviceBusLifeTime.Start();
            _sqlClientLifeTime.Start();
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
            _serviceBusLifeTime.Stop();
            _sqlClientLifeTime.Stop();
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}
