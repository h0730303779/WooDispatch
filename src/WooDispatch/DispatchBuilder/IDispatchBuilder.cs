using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WooDispatch.DispatchBuilder
{
    public interface IDispatchBuilder
    {
        IServiceCollection Services { get; }
    }
}
