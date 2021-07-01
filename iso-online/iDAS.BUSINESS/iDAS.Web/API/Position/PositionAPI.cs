using iDAS.Items.Position;
using iDAS.Web.Areas.Position.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.API
{
    public class PositionAPI
    {
        public async Task<PositiomRespModel> GetPositionAPI()
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<PositionItem> PositionInfo = new List<PositionItem>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/positionApi/getall", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    PositionInfo = JsonConvert.DeserializeObject<List<PositionItem>>(data.data.ToString());
                    PositiomRespModel result = new PositiomRespModel();
                    result.lstPosition = PositionInfo;
                    result.total = PositionInfo.Count;
                    return result;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PositiomRespModel> GetPositionById(int ID)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<PositionItem> PositionInfo = new List<PositionItem>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/PositionApi/GetByID?ID=" + ID, UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    PositionInfo = JsonConvert.DeserializeObject<List<PositionItem>>(data.data.ToString());
                    PositiomRespModel result = new PositiomRespModel();
                    result.lstPosition = PositionInfo;
                    result.total = PositionInfo.Count;
                    return result;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Create(PositionItem position)
        {
            try
            {
                string baseUrl = Ultilities.APIServer;
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));

                    var content = new StringContent(JsonConvert.SerializeObject(position), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/positionApi/create";
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = client.PostAsync(uri, content);
                    var jsonResponse = response.Result.Content.ReadAsStringAsync();
                    return jsonResponse.Result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public string Update(PositionItem position)
        {
            try
            {
                string baseUrl = Ultilities.APIServer;
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));

                    var content = new StringContent(JsonConvert.SerializeObject(position), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/positionApi/update";
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = client.PutAsync(uri, content);
                    var jsonResponse = response.Result.Content.ReadAsStringAsync();
                    return jsonResponse.Result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        public async Task<int> delete(int PositionId)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                PositionItem PositionInfo = new PositionItem();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri("api/PositionApi/Delete?ID=" + PositionId, UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    return Convert.ToInt32(data.data.ToString());

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}