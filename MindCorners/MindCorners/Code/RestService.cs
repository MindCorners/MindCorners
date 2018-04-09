using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.Models.Results;
using Newtonsoft.Json;

namespace MindCorners.Code
{
    public class RestService : IRestService
    {
        //public;

        public RestService()
        {  
        }


        public async Task<T> GetResult<T>(string url)
        {
            T result = default(T);
            try
            {
                using (HttpClient client  = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000000;
                    client.BaseAddress = new Uri(Constants.RestURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User_Id", Settings.CurrentUserId.ToString());
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LocalSettings.CurrentUserId.ToString());

                    HttpResponseMessage response = await client.GetAsync(string.Format("api/" + url));
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(data);
                    }
                }
            }
            catch (Exception e)
            {
                string s = e.ToString();
            }

            return result;
        }

        public async Task<T> PostResult<T, T_Object>(string url, T_Object data)
        {
            T result = default(T);
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constants.RestURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //client.MaxResponseContentBufferSize = 256000000;
                    //client.BaseAddress = new Uri(Constants.RestURL);
                    //client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User_Id", Settings.CurrentUserId.ToString());

                    var json = JsonConvert.SerializeObject(data);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string resultData = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(resultData);
                    }
                }
            }
            catch (Exception e)
            {
                string s = e.ToString();
            }

            return result;

            //T result = default(T);
            //try
            //{
            //    using (client)
            //    {
            //        client.BaseAddress = new Uri(Constants.RestURL);
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //        var jsonData = JsonConvert.SerializeObject(data);

            //        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");


            //        HttpResponseMessage response = await client.PostAsync(string.Format("api/" + url), content);
            //        response.EnsureSuccessStatusCode();
            //        if (response.IsSuccessStatusCode)
            //        {
            //            var resultData = await response.Content.ReadAsStringAsync();
            //            return JsonConvert.DeserializeObject<T>(resultData);
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    string s = e.ToString();
            //}

            //return result;
        }



        //public async Task<HttpResponseMessage> UploadFile(byte[] file)
        //{
        //    var videoThumbName = Guid.NewGuid().ToString();
        //    //var progress = new System.Net.Http.Handlers.ProgressMessageHandler();

        //    // progress.HttpSendProgress += progress_HttpSendProgress;

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(Constants.RestURL);

        //        // Set the Accept header for BSON.
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(
        //                new MediaTypeWithQualityHeaderValue("application/bson"));

        //        var request = new uploadFileModel { data = file, dateCreated = DateTime.Now, fileName = fileName, username = loggedUser, VideoThumbName = videoThumbName };

        //        // POST using the BSON formatter.
        //        MediaTypeFormatter bsonFormatter = new BsonMediaTypeFormatter();
        //        var m = client.MaxResponseContentBufferSize;
        //        var result = await client.PostAsync("api/media/upload", request, bsonFormatter);

        //        return result.EnsureSuccessStatusCode();
        //    }
        //}
    }

    public interface IRestService
    {
    }
}
