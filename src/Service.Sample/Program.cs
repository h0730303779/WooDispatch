using Alexinea.Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Topshelf;
using Topshelf.Autofac;
using WYDispatch.Samples;

namespace Service.Sample
{
    class Program
    {
        static void Main(string[] args)
        {


            var Configurationbuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var Configuration = Configurationbuilder.Build();
            IServiceCollection services = new ServiceCollection();
            services.Configure<DBConfig>(Configuration.GetSection("DBConfig"));


            var builder = new ContainerBuilder();
            builder.RegisterModule(new QuartzAutofacFactoryModule());
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(BackupJob).Assembly));

            builder.Populate(services);

            var container = builder.Build();

            //创建一个服务
            HostFactory.Run(x =>
            {
                x.UseAutofacContainer(container);
                //x.Service<ServiceCenter>(setting => {

                //    setting.ConstructUsingAutofacContainer();
                //    setting.ConstructUsing(name => new ServiceCenter(container));
                //});
                x.Service<ServiceCenter>();
                x.SetDisplayName("数据库自动备份");
                x.SetServiceName("DB自动备份");
                x.SetDescription("数据库自动备份");
                //x.UseNLog();

                x.RunAsLocalSystem();
            });

        }

        //public static ContainerBuilder ConfigureQuartz(this ContainerBuilder builder)
        //{
        //    // 1) Register IScheduler
        //    builder.RegisterModule(new QuartzAutofacFactoryModule());
        //    // 2) Register jobs
        //    builder.RegisterModule(new QuartzAutofacJobsModule(typeof(MyJob1).Assembly));

        //    return builder;
        //}
    }
}
