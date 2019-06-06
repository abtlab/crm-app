using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit.DependencyInjection;
using Xunit.Sdk;

namespace Crm.Infrastructure.DependencyInjection.Tests
{
    public class BaseStartup : DependencyInjectionTestFramework
    {
        public BaseStartup()
            : base(new NullMessageSink())
        {
        }

        protected sealed override void ConfigureServices(IServiceCollection services)
        {
            Configure(services);

            services.BuildServiceProvider();
        }

        protected virtual void Configure(IServiceCollection services)
        {
        }
    }
}