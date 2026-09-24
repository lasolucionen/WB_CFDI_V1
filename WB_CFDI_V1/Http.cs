using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WB_CFDI_V1
{
    public class Http<T>
    {
        private System.Threading.Timer timer;

        public object ObjectJSON { get; set; }
        public string requestURI { get; set; }
        public string log { get; set; }
        public bool IsSuccess { get; set; }
        public ExceptionResponse Err { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public T Response { get; set; }



        public void Post()
        {

            try
            {


                // HttpClientHandler hand = new HttpClientHandler();
                //ProgressMessageHandler processMessageHander = new ProgressMessageHandler(hand);

                using (HttpClient client = new HttpClient(/*processMessageHander*/))
                {

                    //processMessageHander.HttpSendProgress += (sender, e) =>
                    //{
                    //    //if (uploadBar != null)
                    //    //{
                    //    //    Win.Invoke(
                    //    //            new Action(
                    //    //                    () => { uploadBar.Position = e.ProgressPercentage; }));
                    //    //}
                    //};

                    client.Timeout=TimeSpan.FromMinutes(20);
                    MultipartFormDataContent form = new MultipartFormDataContent();

                    if (ObjectJSON != null)
                    {
                        form.Add(new StringContent(ObjectJSON.Json(), Encoding.UTF8, "application/json"), "json");
                    }


                    //if (!String.IsNullOrEmpty(log))
                    //{
                    //    Log.Write(log, "Post()");
                    //}

                    var response = client.PostAsync(requestURI, form).Result;


                    //if (!String.IsNullOrEmpty(log))
                    //{

                    //    Log.Write(log, JsonConvert.SerializeObject(response));
                    //}


                    IsSuccess = response.IsSuccessStatusCode;
                    StatusCode = response.StatusCode;
                    ReasonPhrase = response.ReasonPhrase;



                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;

                        Response = json.GetClass<T>();
                    }
                    else
                    {
                        Err = response.Content.ReadAsStringAsync().Result.GetClass<ExceptionResponse>();
                    }

                }
            }
            catch (Exception e)
            {
                IsSuccess = false;
                Err = new ExceptionResponse
                {
                    ExceptionMessage = e.Message
                };
            }
            finally
            {

            }
        }




        public async Task Get()
        {

            try
            {


                // HttpClientHandler hand = new HttpClientHandler();
                //ProgressMessageHandler processMessageHander = new ProgressMessageHandler(hand);

                using (HttpClient client = new HttpClient(/*processMessageHander*/))
                {

                    //processMessageHander.HttpSendProgress += (sender, e) =>
                    //{
                    //    //if (uploadBar != null)
                    //    //{
                    //    //    Win.Invoke(
                    //    //            new Action(
                    //    //                    () => { uploadBar.Position = e.ProgressPercentage; }));
                    //    //}
                    //};



                    //if (!String.IsNullOrEmpty(log))
                    //{
                    //    Log.Write(log, "Get()");
                    //}

                    var response = await client.GetAsync(requestURI);


                    //if (!String.IsNullOrEmpty(log))
                    //{

                    //    Log.Write(log, JsonConvert.SerializeObject(response));
                    //}


                    IsSuccess = response.IsSuccessStatusCode;
                    StatusCode = response.StatusCode;
                    ReasonPhrase = response.ReasonPhrase;



                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;

                        Response = json.GetClass<T>();
                    }
                    else
                    {
                        Err = response.Content.ReadAsStringAsync().Result.GetClass<ExceptionResponse>();
                    }

                }
            }
            catch (Exception e)
            {
                IsSuccess = false;
                Err = new ExceptionResponse
                {
                    ExceptionMessage = e.Message
                };
            }
            finally
            {

            }
        }
    }
}