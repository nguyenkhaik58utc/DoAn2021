using iDAS.Items.Position;
using iDAS.Items.Problem;
using iDAS.Web.Areas.Problem.Models;
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
using System.Web.Http;

namespace iDAS.Web.API.Problem
{
    public class ProblemTypeAPI
    {
        [HttpPost]
        public async Task<ProblemTypeRespModel> GetProblemTypeAPI(int page, int pageSize)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ProblemTypeItem> ProblemTypeInfo = new List<ProblemTypeItem>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ProblemTypeAPI/getall", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    ProblemTypeInfo = JsonConvert.DeserializeObject<List<ProblemTypeItem>>(data.data.ToString());
                    ProblemTypeRespModel result = new ProblemTypeRespModel();
                    result.lstProblemType = ProblemTypeInfo;
                    result.total = ProblemTypeInfo.Count;
                    return result;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<ProblemTypeRespModel> GetProblemTypeById(int ID)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ProblemTypeItem> ProblemTypeInfo = new List<ProblemTypeItem>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ProblemTypeAPI/GetByID?ID=" + ID, UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    ProblemTypeInfo = JsonConvert.DeserializeObject<List<ProblemTypeItem>>(data.data.ToString());
                    ProblemTypeRespModel result = new ProblemTypeRespModel();
                    result.lstProblemType = ProblemTypeInfo;
                    result.total = ProblemTypeInfo.Count;
                    return result;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<ProblemTypeRespModel> GetListTypeById(int ID)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ProblemTypeItem> ProblemTypeInfo = new List<ProblemTypeItem>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ProblemTypeAPI/GetListByID?ID=" + ID, UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    ProblemTypeInfo = JsonConvert.DeserializeObject<List<ProblemTypeItem>>(data.data.ToString());
                    ProblemTypeRespModel result = new ProblemTypeRespModel();
                    result.lstProblemType = ProblemTypeInfo;
                    result.total = ProblemTypeInfo.Count;
                    return result;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Create(ProblemTypeItem problemTypeItem)
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

                    var content = new StringContent(JsonConvert.SerializeObject(problemTypeItem), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemTypeAPI/create";
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

        public string Update(ProblemTypeItem problemTypeItem)
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

                    var content = new StringContent(JsonConvert.SerializeObject(problemTypeItem), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemTypeAPI/update";
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


        public async Task<int> delete(int ProblemTypeId)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                ProblemTypeItem PositionInfo = new ProblemTypeItem();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri("api/ProblemTypeAPI/Delete?ID=" + ProblemTypeId, UriKind.Relative)
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

        public async Task<int> Default(int ProblemTypeId)
        {
            string baseUrl = Ultilities.APIServer;
            try
            {
                string base64String = Ultilities.GetTokenString();
                ProblemTypeItem req = new ProblemTypeItem();
                req.ID = ProblemTypeId;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemTypeAPI/Default";
                    var response = client.PostAsync(uri, content);
                    var jsonResponse = response.Result.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse.Result);
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