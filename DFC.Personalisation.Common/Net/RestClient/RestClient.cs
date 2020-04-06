using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DFC.Personalisation.Common.Net.RestClient
{
    
    public interface IRestClient
    {
        RestClient.APIResponse LastResponse { get; set; }
        Task<TResponseObject> GetAsync<TResponseObject>(string apiPath, HttpRequestMessage request) where TResponseObject : class;
        Task<TResponseObject> GetAsync<TResponseObject>(string apiPath) where TResponseObject : class;
        Task<byte[]> GetAsync(string apiPath);
        Task<TResponseObject> PostAsync<TResponseObject>(string apiPath, HttpContent content) where TResponseObject : class;
        Task<TResponseObject> PostAsync<TResponseObject>(string apiPath, HttpRequestMessage requestMessage) where TResponseObject : class;
        Task<TResponseObject> PostAsync<TResponseObject>(string apiPath, HttpContent content, string ocpApimSubscriptionKey)
            where TResponseObject : class;
        Task<TRequestResponseObject> PostAsync<TRequestResponseObject>(string apiPath,TRequestResponseObject requestObject) where TRequestResponseObject : class; 
        Task<TResponseObject> PostFormUrlEncodedContentAsync<TResponseObject>(string apiPath,List<KeyValuePair<string, string>> formData) where TResponseObject : class;
        Task<TResponseObject> PutAsync<TResponseObject>(string apiPath, HttpContent content) where TResponseObject : class;
        Task<TResponseObject> PatchAsync<TResponseObject>(string apiPath,List<KeyValuePair<string, string>> requestBody) where TResponseObject : class;
        Task<TResponseObject> DeleteAsync<TResponseObject>(string apiPath) where TResponseObject : class;
    }
    public class RestClient :HttpClient, IRestClient
    {
        public class APIResponse
        {
            public HttpStatusCode StatusCode { get;  set; }
            public bool IsSuccess { get;  set; }
            public string Content { get;  set; }

            public APIResponse()
            {
                
            }
            
            internal APIResponse(HttpResponseMessage responseMessage)
            {
                StatusCode = responseMessage.StatusCode;
                IsSuccess = responseMessage.IsSuccessStatusCode;
                Content = responseMessage.Content?.ReadAsStringAsync().Result;
            }
        }
        
        private const string MediaTypeJsonPatch = "application/json-patch+json";
        private const string OcpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
        
        public virtual APIResponse LastResponse { get; set; }
       

        public RestClient(): base()
        {
            InitialiseDefaultRequestHeaders();
        }
        public RestClient(HttpMessageHandler handler)
            : base(handler)
        {
            InitialiseDefaultRequestHeaders();
        }
        
        #region Private Methods
        
        private static T JsonStringToObject<T>(string json) where T:class
        {
            var token = JToken.Parse(json);
            var obj = token.ToObject<T>();
            return obj;
        }
        private static string ObjectToJsonString<T>(T obj) where T:class
        {
            var token = JToken.FromObject(obj);
            var jsonString = token.ToString();
            return jsonString;
        }
        
        #endregion Private Methods
        
        #region Public Methods
        public async Task<TResponseObject> GetAsync<TResponseObject>(string apiPath) where TResponseObject : class
        {
            TResponseObject responseObject = default;
            try
            {
                LastResponse = null;
                var response = await GetAsync(apiPath, HttpCompletionOption.ResponseContentRead);
                LastResponse = new APIResponse(response);
                var jsonString = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    responseObject = JsonStringToObject<TResponseObject>(jsonString);
                }
                return responseObject;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<TResponseObject> GetAsync<TResponseObject>(string apiPath, HttpRequestMessage request) where TResponseObject : class
        {
            foreach (var (key, value) in request.Headers)
            {
                DefaultRequestHeaders.Add(key, value);
            }

            return await GetAsync<TResponseObject>(apiPath);
        }
        
        public new async Task<byte[]>GetAsync(string apiPath)
        {
            byte[] responseData = default;
            try
            {
                LastResponse = null;
                var response = await GetAsync(apiPath, HttpCompletionOption.ResponseContentRead);
                LastResponse = new APIResponse(response);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsByteArrayAsync();
                }
                return responseData;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public  async Task<TResponseObject>  PostAsync<TResponseObject>(string apiPath, HttpContent content) where TResponseObject : class
        {
            TResponseObject responseObject = default;
            try
            {
                LastResponse = null;
                
                var response = await base.PostAsync(apiPath, content);
                LastResponse = new APIResponse(response);
                var jsonString = await response.Content.ReadAsStringAsync();
                if(!string.IsNullOrWhiteSpace(jsonString))
                {
                    responseObject = JsonStringToObject<TResponseObject>(jsonString);
                }
                return responseObject;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<TResponseObject> PostAsync<TResponseObject>(string apiPath, HttpContent content,string ocpApimSubscriptionKey)
            where TResponseObject : class
        {
            DefaultRequestHeaders.Add(OcpApimSubscriptionKeyHeader, ocpApimSubscriptionKey);
            return await PostAsync<TResponseObject>(apiPath,content);
        }

        public async Task<TResponseObject> PostAsync<TResponseObject>(string apiPath, HttpRequestMessage requestMessage)
            where TResponseObject : class
        {
            foreach (var header in requestMessage.Headers)
            {
                DefaultRequestHeaders.Add(header.Key,header.Value);
            }
            return await PostAsync<TResponseObject>(apiPath,requestMessage.Content);
        }

        public async Task<TResponseObject> PostAsync<TResponseObject>(string apiPath, TResponseObject requestObject) where TResponseObject : class
        {
            try
            {
                var jsonString = ObjectToJsonString(requestObject);
                using var content = new StringContent(jsonString, System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
                return  await PostAsync<TResponseObject>(apiPath, content);
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<TResponseObject> PostFormUrlEncodedContentAsync<TResponseObject>(string apiPath, List<KeyValuePair<string, string>> formData) where TResponseObject : class
        {
            try
            {
                using var content = new FormUrlEncodedContent(formData);
                return await PostAsync<TResponseObject>(apiPath, content);
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<TResponseObject> PutAsync<TResponseObject>(string apiPath, HttpContent content) where TResponseObject : class
        {
            TResponseObject responseObject = default;
            try
            {
                LastResponse = null;
                var response = await PutAsync(apiPath, content);
                LastResponse = new APIResponse(response);
                var jsonString = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    responseObject = JsonStringToObject<TResponseObject>(jsonString);
                }

                return responseObject;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<TResponseObject> DeleteAsync<TResponseObject>(string apiPath) where TResponseObject: class
        {
            try
            {
                LastResponse = null;
                var response = await DeleteAsync(apiPath);
                LastResponse = new APIResponse(response);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonStringToObject<TResponseObject>(jsonString);
                return responseObject;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<TResponseObject> PatchAsync<TResponseObject>( string apiPath, List<KeyValuePair<string, string>> requestBody) where TResponseObject : class
        {

            TResponseObject responseObject = default;

            try
            {
                LastResponse = null;
                var jsonRequest = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, MediaTypeJsonPatch);
                var response = await PatchAsync(apiPath, content);
                LastResponse = new APIResponse(response);
                var jsonString = response.Content.ReadAsStringAsync().Result;
                if(!string.IsNullOrWhiteSpace(jsonString))
                {
                    responseObject = JsonStringToObject<TResponseObject>(jsonString);
                }
                return responseObject;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }

        }

        #endregion Public Methods

        public void InitialiseDefaultRequestHeaders()
        {
            DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        }
       

    }
}

