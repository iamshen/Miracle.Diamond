using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using netCore.Utitly.Interface;
using netCore.Utitly.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace netCore.Utitly
{
    /// <summary>
    /// 初始化拓展
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// 框架注册入口
        /// </summary>
        /// <param name="app"></param>
        public static IServiceCollection RegisterBaseServices(this IServiceCollection services)
        {
            services.AddHttpClient<IHttpClientEntity, HttpClientEntity>();
            return services;
        }
    }
}
