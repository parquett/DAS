﻿using SecurityCRM.ApiContracts.DTO.User;
using Lib.ErrorHandler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SecurityCRM.Helpers
{
    //public static class SecurityCRMAuthApiHelper
    //{
    //    private static readonly string BaseUrl = "http://api.SecurityCRM.galex.md/auth";
    //    private static T MakeRequest<T>(string httpMethod, string route, Dictionary<string, string> postParams = null)
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod(httpMethod), $"{BaseUrl}/{route}");

    //            if (postParams != null)
    //                requestMessage.Content = new FormUrlEncodedContent(postParams);   // This is where your content gets added to the request body


    //            HttpResponseMessage response = client.SendAsync(requestMessage).Result;

    //            string apiResponse = response.Content.ReadAsStringAsync().Result;
    //            try
    //            {
    //                // Attempt to deserialise the reponse to the desired type, otherwise throw an expetion with the response from the api.
    //                if (apiResponse != "")
    //                    return JsonConvert.DeserializeObject<T>(apiResponse);
    //                else
    //                    throw new Exception();
    //            }
    //            catch (Exception ex)
    //            {
    //                throw new Exception($"An error ocurred while calling the API. It responded with the following message: {response.StatusCode} {response.ReasonPhrase}");
    //            }
    //        }
    //    }
    
    //    public static UserResource LoginApi(LoginResource loginForm)
    //    {
    //        var postParams = new Dictionary<string, string>() { { "Username", loginForm.Username }, { "Password",loginForm.Password} };
    //        return MakeRequest<UserResource>("POST", "login", postParams);
    //    }

    //}
}
