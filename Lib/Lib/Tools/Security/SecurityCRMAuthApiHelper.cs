using SecurityCRM.ApiContracts.DTO.User;
using Lib.ErrorHandler;
using Lib.Helpers;
using Lib.Tools.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SecurityCRM.Helpers
{
    public static class SecurityCRMAuthApiHelper
    {
        private static readonly string BaseUrl = Config.GetConfigValue("AuthApiURL");


        public static async Task<T> GetRequestAsync<T>(string route, HttpContext context)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SessionHelper<string>.Pull("Token", out var token);
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.TrimStart('\"').TrimEnd('\"'));
                    var response = await client.GetAsync($"{BaseUrl}{route}");
                  
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        context.Request.Cookies.TryGetValue("refreshToken", out var refresh);
                        context.Request.Cookies.TryGetValue("DeviceId", out var deviceId);
                        CookieContainer cookies = new CookieContainer();
                        cookies.Add(new Uri(BaseUrl), new Cookie("DeviceId", deviceId));
                        cookies.Add(new Uri(BaseUrl), new Cookie("refreshToken", refresh));
                        client.DefaultRequestHeaders.Add("cookie", cookies.GetCookieHeader((new Uri(BaseUrl))));
                        response = await client.GetAsync($"{BaseUrl}Token/refresh");
                        if (response.IsSuccessStatusCode)
                        {
                            AuthenticatedResponse? user = await response.Content.ReadFromJsonAsync<AuthenticatedResponse>();
                            context.Session.Remove("token");
                            context.Session.SetString("token", user.AccessToken);
                            response = await client.GetAsync($"{BaseUrl}{route}");
                            return await response.Content.ReadFromJsonAsync<T>();
                        }
                    
                    }
                    T? data = await response.Content.ReadFromJsonAsync<T>();
                    return data;
                }
            }
            catch(Exception ex)
            {
                return default(T); 
            }

        }
        public static async Task<T> PostRequestAsync<T>(string route, HttpContext context, object postparams = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SessionHelper<string>.Pull("Token", out var token);
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.TrimStart('\"').TrimEnd('\"'));
                    JsonContent content = JsonContent.Create(postparams);
                    var response = await client.PostAsync($"{BaseUrl}{route}", content);
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        context.Request.Cookies.TryGetValue("refreshToken", out var refresh);
                        context.Request.Cookies.TryGetValue("DeviceId", out var deviceId);
                        CookieContainer cookies = new CookieContainer();
                        cookies.Add(new Uri(BaseUrl), new Cookie("DeviceId", deviceId));
                        cookies.Add(new Uri(BaseUrl), new Cookie("refreshToken", refresh));
                        client.DefaultRequestHeaders.Add("cookie", cookies.GetCookieHeader((new Uri(BaseUrl))));
                        response = await client.GetAsync($"{BaseUrl}Token/refresh");
                        if (response.IsSuccessStatusCode)
                        {
                            AuthenticatedResponse? user = await response.Content.ReadFromJsonAsync<AuthenticatedResponse>();
                            context.Session.Remove("token");
                            context.Session.SetString("token", user.AccessToken);
                            response = await client.PostAsync($"{BaseUrl}{route}",content);
                            return await response.Content.ReadFromJsonAsync<T>();
                        }

                    }
                    T? data = await response.Content.ReadFromJsonAsync<T>();
                    return data;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }

        }
        //private static T MakeRequest<T>(string httpMethod, string route, Dictionary<string, string> postParams = null)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod(httpMethod), $"{BaseUrl}/{route}");

        //        if (postParams != null)
        //            requestMessage.Content = new FormUrlEncodedContent(postParams);   // This is where your content gets added to the request body


        //        HttpResponseMessage response = client.SendAsync(requestMessage).Result;

        //        string apiResponse = response.Content.ReadAsStringAsync().Result;
        //        try
        //        {
        //            // Attempt to deserialise the reponse to the desired type, otherwise throw an expetion with the response from the api.
        //            if (apiResponse != "")
        //                return JsonConvert.DeserializeObject<T>(apiResponse);
        //            else
        //                throw new Exception();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception($"An error ocurred while calling the API. It responded with the following message: {response.StatusCode} {response.ReasonPhrase}");
        //        }
        //    }
        //}
        [ActionInterceptor]
        public static async void Logout()
        {
            HttpClient http = new HttpClient();
            var response = await http.PostAsync("https://localhost:5001/api/Token/logout", null);
        }
        public static async Task<AuthenticatedResponse> RefreshTokens(HttpContext context)
        {
            context.Request.Cookies.TryGetValue("refreshToken", out var refresh);
            context.Request.Cookies.TryGetValue("DeviceId", out var deviceId);
            CookieContainer cookies = new CookieContainer();
            cookies.Add(new Uri("https://localhost:5001/"), new Cookie("DeviceId", deviceId));
            cookies.Add(new Uri("https://localhost:5001/"), new Cookie("refreshToken", refresh));
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.Add("cookie", cookies.GetCookieHeader((new Uri("https://localhost:5001/"))));
            var response = await http.GetAsync("https://localhost:5001/api/Token" + "/refresh");
            AuthenticatedResponse? user = await response.Content.ReadFromJsonAsync<AuthenticatedResponse>();

            return user;
        }

        public static async Task<AuthenticatedResponse> LoginApi(LoginResource loginForm, HttpContext context = null)
        {

            HttpClient http = new HttpClient();
            JsonContent content = JsonContent.Create(loginForm);
            var response = await http.PostAsync(BaseUrl + "/login", content);
            var x = response.Content.Headers.GetEnumerator();
            Console.WriteLine(x);
            AuthenticatedResponse? user = await response.Content.ReadFromJsonAsync<AuthenticatedResponse>();
            //context.Response.Cookies.Append("refreshToken", user.RefreshToken);
            //context.Response.Cookies.Append("DeviceId", loginForm.DeviceGuidId.ToString());
            return user;

        }

        public static async Task<UserResource> RegistestApi(RegisterResource registerResource)
        {
            HttpClient http = new HttpClient();
            JsonContent content = JsonContent.Create(registerResource);
            var response = await http.PostAsync(BaseUrl + "/register", content);
            UserResource? user = await response.Content.ReadFromJsonAsync<UserResource>();
            return user;

        }

    }
}
