
using iDAS.Core;
using iDAS.Items.Problem;
using iDAS.Web.Areas.Department.Models;
using iDAS.Web.Areas.Problem.Models;
using ISO.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace iDAS.Web.API.Problem
{
    public class ProblemEmergencyAPI
    {
        private string baseUrl = Ultilities.APIServer;
        public ResponseModel<List<ProblemEmergencyItem>> GetData(int page, int pageSize)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemEmergencyRespModel req = new ProblemEmergencyRespModel();
                req.PageNumber = page;
                req.PageSize = pageSize;

                ResponseModel<List<ProblemEmergencyItem>> result = new ResponseModel<List<ProblemEmergencyItem>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProblemEmergencyAPI/GetPaging";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemEmergencyItem>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProblemEmergencyItem> GetByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ProblemEmergencyItem result = new ProblemEmergencyItem();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    //client.DefaultRequestHeaders.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ProblemEmergencyAPI/GetByID?ID="+ID, UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<ProblemEmergencyItem>>(jsonResponse);
                    result = respMode.Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(ProblemEmergencyItem emergencyItem)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = emergencyItem, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemEmergencyAPI/InsertUpdate";
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

        public string Update(ProblemEmergencyItem emergencyItem)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = emergencyItem, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemEmergencyAPI/InsertUpdate";
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

        public async Task<int> Delete(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ProblemEmergencyItem req = new ProblemEmergencyItem();
                req.ID = ID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemEmergencyAPI/Delete";
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
                    var uri = "api/ProblemEmergencyAPI/Default";
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