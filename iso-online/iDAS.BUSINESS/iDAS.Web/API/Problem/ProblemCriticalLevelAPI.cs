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
    public class ProblemCriticalLevelAPI
    {
        [HttpPost]
        public async Task<ProblemCriticalLevelRespModel> GetAll(int page, int pageSize)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ProblemCriticalLevelItem> ProblemCriticalLevelInfo = new List<ProblemCriticalLevelItem>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ProblemCriticalLevelAPI/getall", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    ProblemCriticalLevelInfo = JsonConvert.DeserializeObject<List<ProblemCriticalLevelItem>>(data.data.ToString());
                    ProblemCriticalLevelRespModel result = new ProblemCriticalLevelRespModel();
                    result.lstProblemCriticalLevel = ProblemCriticalLevelInfo;
                    result.total = ProblemCriticalLevelInfo.Count;
                    return result;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<ProblemCriticalLevelRespModel> GetById(int ID)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ProblemCriticalLevelItem> problemCriticalLevelInfo = new List<ProblemCriticalLevelItem>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ProblemCriticalLevelAPI/GetByID?ID=" + ID, UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    problemCriticalLevelInfo = JsonConvert.DeserializeObject<List<ProblemCriticalLevelItem>>(data.data.ToString());
                    ProblemCriticalLevelRespModel result = new ProblemCriticalLevelRespModel();
                    result.lstProblemCriticalLevel = problemCriticalLevelInfo;
                    result.total = problemCriticalLevelInfo.Count;
                    return result;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string Create(ProblemCriticalLevelItem problemCriticalLevelItem)
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

                    var content = new StringContent(JsonConvert.SerializeObject(problemCriticalLevelItem), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemCriticalLevelAPI/create";
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

        public string Update(ProblemCriticalLevelItem problemCriticalLevelItem)
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

                    var content = new StringContent(JsonConvert.SerializeObject(problemCriticalLevelItem), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemCriticalLevelAPI/update";
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


        public async Task<int> delete(int ProblemCriticalLevelID)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri("api/ProblemCriticalLevelAPI/Delete?ID=" + ProblemCriticalLevelID, UriKind.Relative)
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

        public async Task<int> Default(int ProblemCriticalLevelId)
        {
            string baseUrl = Ultilities.APIServer;
            try
            {
                string base64String = Ultilities.GetTokenString();
                ProblemCriticalLevelItem req = new ProblemCriticalLevelItem();
                req.ID = ProblemCriticalLevelId;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemCriticalLevelAPI/Default";
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