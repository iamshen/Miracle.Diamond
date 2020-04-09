using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace netCore.Utitly.Interface
{
    public interface IHttpClientEntity
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="header">头部信息</param>
        /// <returns></returns>
        T Get<T>(string url, IDictionary<string, string> header = null);
        /// <summary>
        /// 异步Get请求
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="header">头部信息</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string url, IDictionary<string, string> header = null);
        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="pairs">传递参数</param>
        /// <param name="header">头部信息</param>
        /// <returns></returns>
        T Post<T>(string url, IDictionary<string, object> pairs, IDictionary<string, string> header);
        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="pairs">传递参数</param>
        /// <param name="header">头部信息</param>
        /// <returns></returns>
        Task<T> PostAsync<T>(string url, IDictionary<string, object> pairs, IDictionary<string, string> header);
    }
}
