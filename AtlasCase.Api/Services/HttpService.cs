using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AtlasCase.Api.Services
{
    public static class HttpService
    {
        public static Task PostAsync(string customerApiUrl, object postModel)
        {
            return Task.Run(() =>
              {
                  try
                  {
                      var httpWebRequest = (HttpWebRequest)WebRequest.Create(customerApiUrl);
                      httpWebRequest.ContentType = "application/json";
                      httpWebRequest.Method = "POST";
                      //httpWebRequest.Headers.Add("Authorization: TOKEN");

                      using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                      {
                          var json = JsonSerializer.Serialize(postModel);

                          streamWriter.Write(json);
                          streamWriter.Flush();
                          streamWriter.Close();
                      }

                      var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                      var result = string.Empty;
                      using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                      {
                          result = streamReader.ReadToEnd();
                      }

                      //TODO:Completed Log
                  }
                  catch (Exception e)
                  {
                      //TODO:Failed Log
                  }
              });
        }
    }
}
