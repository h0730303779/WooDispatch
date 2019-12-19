using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WooDispatch
{
    public class SchedulerFactory : StdSchedulerFactory
    {
        public SchedulerFactory(IJobFactory jobFactory)
        {
            JobFactory = jobFactory;
        }

        public SchedulerFactory(NameValueCollection props, IJobFactory jobFactory) : base(props)
        {
            JobFactory = jobFactory;
        }

        public IJobFactory JobFactory { get; }

        public override async Task<IScheduler> GetScheduler(
            CancellationToken cancellationToken = new CancellationToken())
        {
            var sch = await base.GetScheduler(cancellationToken).ConfigureAwait(false);
            sch.JobFactory = JobFactory;
            return sch;
        }

        public override async Task<IScheduler> GetScheduler(string schedName,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var sch = await base.GetScheduler(schedName, cancellationToken).ConfigureAwait(false);
            sch.JobFactory = JobFactory;
            return sch;
        }
    }
}
