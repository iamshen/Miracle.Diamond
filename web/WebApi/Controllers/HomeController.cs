using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using netCore.Utitly.Interface;

namespace WebApi.Controllers
{
    /// <summary>
    /// 控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IHttpClientEntity _httpClientEntity;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="httpClientEntity"></param>
        public HomeController(IHttpClientEntity httpClientEntity)
        {
            _httpClientEntity = httpClientEntity;
        }
        /// <summary>
        /// 测试接口
        /// </summary>
        /// <param name="id">参数1</param>
        /// <param name="name">参数2</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> Test(string id, string name)
        {
            var data = _httpClientEntity.Get<object>($"http://localhost:9000/api/Home/GetApi?name={name}");
            var data2 = _httpClientEntity.GetAsync<object>($"http://localhost:9000/api/Home/GetApi?name={name}");
            return await Task.FromResult(true);
        }
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> GetApi(string name)
        {
            return await Task.FromResult(name);
        }
        /// <summary>
        /// Post
        /// </summary>
        /// <param name="id">参数1</param>
        /// <param name="name">参数2</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> PostApi(string id, string name)
        {
            return await Task.FromResult(new { id, name });
        }
    }
}