using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WooDispatch.Entity;

namespace WooDispatch.DispatchBuilder
{
    public class DefaultLogDashboardBuilder : IDispatchBuilder
    {
        public DefaultLogDashboardBuilder()
        {
        }

        public DefaultLogDashboardBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public DispatchOptions DispatchOptions { get; set; }

        public IServiceCollection Services { get; set; }
    }
}
