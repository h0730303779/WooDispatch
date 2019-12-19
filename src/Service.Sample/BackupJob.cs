using Microsoft.Extensions.Options;
using NLog;
using Quartz;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Sample
{
    public class BackupJob : IJob
    {
        private readonly Logger logger;
        private DBConfig dBConfig;

        public BackupJob(IOptions<DBConfig> options)
        {
            dBConfig = options.Value;
            logger = LogManager.GetCurrentClassLogger();
        }


        public Task Execute(IJobExecutionContext context)
        {

            logger.Debug("Start BackupJob"+ dBConfig.Uris.ToString());
            return Task.CompletedTask;
        }
    }
}
