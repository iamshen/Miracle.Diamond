using netCore.Utitly.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace netCore.Utitly.Services
{
    public class HttpClientEntity : IHttpClientEntity
    {
        private readonly HttpClient _httpClient;

        public HttpClientEntity(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="header">头部信息</param>
        /// <returns></returns>
        public T Get<T>(string url, IDictionary<string, string> header = null)
        {
            FillHeader(header);
            var task = _httpClient.GetStringAsync(url);
            task.Wait();
            return !task.Result.IsNullOrEmpty() ? task.Result.FromJson<T>() : default;
        }
        /// <summary>
        /// 异步Get请求
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="header">头部信息</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string url, IDictionary<string, string> header = null)
        {
            FillHeader(header);
            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return await Task.FromResult(json.FromJson<T>());
            }
            return await Task.FromResult(default(T));
        }
        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="pairs">传递参数</param>
        /// <param name="header">头部信息</param>
        /// <returns></returns>
        public T Post<T>(string url, IDictionary<string, object> pairs, IDictionary<string, string> header)
        {
            FillHeader(header);
            var stream = GetDataStream(pairs);
            var task = _httpClient.PostAsync(url, new StreamContent(stream));
            task.Wait();
            if (task.Result.StatusCode == HttpStatusCode.OK)
            {

            }
            return default;
        }
        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="pairs">传递参数</param>
        /// <param name="header">头部信息</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string url, IDictionary<string, object> pairs, IDictionary<string, string> header)
        {
            FillHeader(header);
            var stream = GetDataStream(pairs);
            var response = await _httpClient.PostAsync(url, new StreamContent(stream));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return await Task.FromResult(json.FromJson<T>());
            }
            return await Task.FromResult(default(T));
        }
        /// <summary>
        /// 填充头部信息
        /// </summary>
        /// <param name="headers">头部信息</param>
        private void FillHeader(IDictionary<string, string> headers)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            if (headers != null && headers.Any())
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            if (!_httpClient.DefaultRequestHeaders.Contains("Accpet"))
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        /// <summary>
        /// 获取请求数据流
        /// </summary>
        /// <param name="pairs">参数集合</param>
        /// <returns></returns>
        private Stream GetDataStream(IDictionary<string, object> pairs)
        {
            MemoryStream ms = new MemoryStream();
            var str = GetQueryString(pairs);
            var bytes = str.IsNullOrEmpty() ? new byte[0] : Encoding.UTF8.GetBytes(str);
            ms.Write(bytes, 0, str.Length);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
        /// <summary>
        /// 获取请求数据
        /// </summary>
        /// <param name="pairs">传入参数集合</param>
        /// <returns></returns>
        private string GetQueryString(IDictionary<string, object> pairs)
        {
            StringBuilder sb = new StringBuilder();
            if (pairs != null && pairs.Any())
            {
                foreach (var item in pairs)
                {
                    sb.Append($"{item.Key}={item.Value.ToJson()}&");
                }
            }
            return sb.ToString().TrimEnd('&');
        }
    }
}
