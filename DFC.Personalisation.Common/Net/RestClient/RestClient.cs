using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;

namespace DFC.Personalisation.Common.Net.RestClient
{
    
    public interface IRestClient
    {
        Task<TResponseObject> Get<TResponseObject>(string apiPath, string ocpApimSubscriptionKey) where TResponseObject : class;
        Task<TResponseObject> Get<TResponseObject>(string apiPath) where TResponseObject : class;
        Task<byte[]> Get(string apiPath);
        Task<TResponseObject> Post<TResponseObject>(string apiPath, HttpContent content) where TResponseObject : class;

        Task<TResponseObject> Post<TResponseObject>(string apiPath, HttpContent content, string ocpApimSubscriptionKey)
            where TResponseObject : class;
        Task<TRequestResponseObject> Post<TRequestResponseObject>(string apiPath,TRequestResponseObject requestObject) where TRequestResponseObject : class; 
        Task<TResponseObject> PostFormUrlEncodedContent<TResponseObject>(string apiPath,List<KeyValuePair<string, string>> formData) where TResponseObject : class;
        Task<TResponseObject> Put<TResponseObject>(string apiPath, HttpContent content) where TResponseObject : class;
        Task<TResponseObject> Patch<TResponseObject>(string apiPath,List<KeyValuePair<string, string>> requestBody) where TResponseObject : class;
        Task<TResponseObject> Delete<TResponseObject>(string apiPath) where TResponseObject : class;
    }
    public class RestClient :IRestClient
    {
        public class APIResponse
        {
            public HttpStatusCode StatusCode { get; internal set; }
            public bool IsSuccess { get; internal set; }
            public string Content { get; internal set; }

            internal APIResponse(HttpResponseMessage responseMessage)
            {
                StatusCode = responseMessage.StatusCode;
                IsSuccess = responseMessage.IsSuccessStatusCode;
                Content = responseMessage.Content?.ReadAsStringAsync().Result;
            }
        }
        private const string _mediaTypeJson = "application/json";
        private const string _mediaTypeJsonPatch = "application/json-patch+json";
        private const string _ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
        private readonly HttpClient _httpClient;
        private APIResponse _lastResponse;
        public APIResponse LastResponse
        {
            get { return _lastResponse; }
            internal set { _lastResponse = value; }
        }
        public RestClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(_mediaTypeJson));
        }

        #region Private Methods
        private static T JsonStringToObject<T>(string json) where T:class
        {
            JToken token = JToken.Parse(json);
            T obj = token.ToObject<T>();
            return obj;
        }

        private static string ObjectToJsonString<T>(T obj) where T:class
        {
            JToken token = JToken.FromObject(obj);
            string jsonString = token.ToString();
            return jsonString;
        }

        #endregion Private Methods
        
        #region Public Methods
        public async Task<TResponseObject> Get<TResponseObject>(string apiPath) where TResponseObject : class
        {
            TResponseObject responseObject = default;
            try
            {
                LastResponse = null;
                var response = await _httpClient.GetAsync(apiPath, HttpCompletionOption.ResponseContentRead);
                LastResponse = new APIResponse(response);
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    responseObject = JsonStringToObject<TResponseObject>(jsonString);
                }
                return responseObject;
            }
            catch (AggregateException ex)
            {
                Exception e = ex.InnerException;
                throw e;
            }
        }

        public async Task<TResponseObject> Get<TResponseObject>(string apiPath, string ocpApimSubscriptionKey) where TResponseObject : class
        {
            _httpClient.DefaultRequestHeaders.Add(_ocpApimSubscriptionKeyHeader, ocpApimSubscriptionKey);
            return await Get<TResponseObject>(apiPath);
        }
        
        public async Task<byte[]> Get(string apiPath)
        {
            byte[] responseData = default;
            try
            {
                LastResponse = null;
                var response = await _httpClient.GetAsync(apiPath, HttpCompletionOption.ResponseContentRead);
                LastResponse = new APIResponse(response);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsByteArrayAsync();
                }
                return responseData;
            }
            catch (AggregateException ex)
            {
                Exception e = ex.InnerException;
                throw e;
            }
        }

        
        public async Task<TResponseObject>  Post<TResponseObject>(string apiPath, HttpContent content) where TResponseObject : class
        {
            TResponseObject responseObject = default;
            try
            {
                LastResponse = null;
                var response = await _httpClient.PostAsync(apiPath, content);
                LastResponse = new APIResponse(response);
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();
                if(!string.IsNullOrWhiteSpace(jsonString))
                {
                    responseObject = JsonStringToObject<TResponseObject>(jsonString);
                }
                return responseObject;
            }
            catch (AggregateException ex)
            {
                Exception e = ex.InnerException;
                throw e;
            }
        }

        public async Task<TResponseObject> Post<TResponseObject>(string apiPath, HttpContent content,string ocpApimSubscriptionKey)
            where TResponseObject : class
        {
            _httpClient.DefaultRequestHeaders.Add(_ocpApimSubscriptionKeyHeader, ocpApimSubscriptionKey);
            return await Post<TResponseObject>(apiPath,content);
        }

        public async Task<TResponseObject> Post<TResponseObject>(string apiPath, TResponseObject requestObject) where TResponseObject : class
        {
            try
            {
                string jsonString = ObjectToJsonString(requestObject);
                using StringContent content = new StringContent(jsonString, System.Text.Encoding.UTF8, _mediaTypeJson);
                return  await Post<TResponseObject>(apiPath, content);
            }
            catch (AggregateException ex)
            {
                Exception e = ex.InnerException;
                throw e;
            }
        }

        public async Task<TResponseObject> PostFormUrlEncodedContent<TResponseObject>(string apiPath, List<KeyValuePair<string, string>> formData) where TResponseObject : class
        {
            try
            {
                using var content = new FormUrlEncodedContent(formData);
                return await Post<TResponseObject>(apiPath, content);
            }
            catch (AggregateException ex)
            {
                Exception e = ex.InnerException;
                throw e;
            }
        }

        public async Task<TResponseObject> Put<TResponseObject>(string apiPath, HttpContent content) where TResponseObject : class
        {
            TResponseObject responseObject = default;
            try
            {
                LastResponse = null;
                HttpResponseMessage response = await _httpClient.PutAsync(apiPath, content);
                LastResponse = new APIResponse(response);
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    responseObject = JsonStringToObject<TResponseObject>(jsonString);
                }

                return responseObject;
            }
            catch (AggregateException ex)
            {
                Exception e = ex.InnerException;
                throw e;
            }
        }

        public async Task<TResponseObject> Delete<TResponseObject>(string apiPath) where TResponseObject: class
        {
            try
            {
                LastResponse = null;
                HttpResponseMessage response = await _httpClient.DeleteAsync(apiPath);
                LastResponse = new APIResponse(response);
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonStringToObject<TResponseObject>(jsonString);
                return responseObject;
            }
            catch (AggregateException ex)
            {
                Exception e = ex.InnerException;
                throw e;
            }
        }

        public async Task<TResponseObject> Patch<TResponseObject>( string apiPath, List<KeyValuePair<string, string>> requestBody) where TResponseObject : class
        {

            TResponseObject responseObject = default;

            try
            {
                LastResponse = null;
                var jsonRequest = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, _mediaTypeJsonPatch);
                HttpResponseMessage response = await _httpClient.PatchAsync(apiPath, content);
                LastResponse = new APIResponse(response);
                response.EnsureSuccessStatusCode();
                string jsonString = response.Content.ReadAsStringAsync().Result;
                if(!string.IsNullOrWhiteSpace(jsonString))
                {
                    responseObject = JsonStringToObject<TResponseObject>(jsonString);
                }
                return responseObject;
            }
            catch (AggregateException ex)
            {
                Exception e = ex.InnerException;
                throw e;
            }


           
        }
        
        
        #endregion Public Methods


    }
}
