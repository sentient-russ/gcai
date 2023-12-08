using gcai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace gcai.Services
{
    [ApiController]
    [Route("[Controller]")]
    public class AIAccess : ControllerBase
    {

        private static  HttpClient _httpClient;

        static AIAccess()
        {
            _httpClient = new HttpClient();
        }
           
        [HttpGet]
        public async Task<AIModel> QueryAI(AIModel aiModelIn)
        {
            AIModel? aiResultModel = new AIModel();
            /*string aiContext = "";*/
            /*aiModelIn.Prompt = aiContext + aiModelIn.Prompt;*/
            using (var client = new HttpClient())
            {
                string uri;
                var environ = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (environ == "Production")
                {
                    uri = "http://127.0.0.1:5110/api/prompt_route";
                }
                else
                {
                    uri = "http://162.205.232.101:5110/api/prompt_route";

                }


                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Add("Accept", "*/*");

                var Parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("user_prompt", aiModelIn.Prompt),
                };

                var Request = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = new FormUrlEncodedContent(Parameters)
                };

                Task<string> Result = client.SendAsync(Request).Result.Content.ReadAsStringAsync();
                aiResultModel = JsonSerializer.Deserialize < AIModel>(Result.Result);
                Debug.WriteLine($"Prompt: {aiResultModel?.Prompt}");
                Debug.WriteLine($"Answer: {aiResultModel?.Answer}");
                //insert code to add sourses to aiResultModel from serializer here.
                aiResultModel.idAIModel = aiModelIn.idAIModel;
                /*aiResultModel.Prompt = aiModelIn.Prompt.Replace(aiContext, "");*/
                aiResultModel.Context = aiModelIn.Prompt;
                aiResultModel.PostDate = aiModelIn.PostDate;
                aiResultModel.ScreenName = aiModelIn.ScreenName;
                

                return aiResultModel;
            }
        }
    }
}
