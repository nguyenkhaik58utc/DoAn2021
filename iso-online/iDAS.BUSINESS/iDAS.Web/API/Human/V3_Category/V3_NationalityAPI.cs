using iDAS.Web.Areas.Human.Models.V3_Category;
using ISO.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace iDAS.Web.API.Human.V3_Category
{
    public class V3_NationalityAPI
    {
        private string baseUrl = Ultilities.APIServer;
        /// <summary>
        /// lấy danh sách quốc tịch
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<V3_NationalityResponse>>> GetAll()
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<V3_NationalityResponse>> result = new ResponseModel<List<V3_NationalityResponse>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("/api/NationalityAPI/GetAll"), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_NationalityResponse>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseModel<List<V3_NationalityResponse>> GetPagging(int page, int pageSize)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                NationalityReqModel req = new NationalityReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;

                ResponseModel<List<V3_NationalityResponse>> result = new ResponseModel<List<V3_NationalityResponse>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/NationalityAPI/GetPaging";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_NationalityResponse>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<V3_NationalityResponse> GetByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                V3_NationalityResponse result = new V3_NationalityResponse();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/NationalityAPI/GetByID?ID=" + ID, UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<V3_NationalityResponse>>(jsonResponse);
                    result = respMode.Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(V3_NationalityResponse nationalItem)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                nationalItem.CreatedAt = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = nationalItem, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/NationalityAPI/Insert";
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

        public string Update(V3_NationalityResponse nationalItem)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                nationalItem.UpdatedAt = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = nationalItem, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/NationalityAPI/Update";
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
                V3_NationalityResponse req = new V3_NationalityResponse();
                req.ID = ID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/NationalityAPI/Delete";
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