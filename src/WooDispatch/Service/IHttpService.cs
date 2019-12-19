using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WooDispatch.Service
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> GetAsync(string requestUrl, Dictionary<string, string> headers);
        Task<HttpResponseMessage> PostAsync(string requestUrl, string requestParameters, Dictionary<string, string> headers);
        Task<HttpResponseMessage> PutAsync(string requestUrl, string requestParameters, Dictionary<string, string> headers);
        Task<HttpResponseMessage> DeleteAsync(string requestUrl, Dictionary<string, string> headers);
    }
}
