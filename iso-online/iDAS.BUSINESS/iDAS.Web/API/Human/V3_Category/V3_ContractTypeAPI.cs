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
    /// <summary>
    /// Danh mục loại hợp đồng
    /// </summary>
    public class V3_ContractTypeAPI
    {
        private string baseUrl = Ultilities.APIServer;
        /// <summary>
        /// lấy danh sách
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<V3_ContractTypeResponse>>> GetAll()
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<V3_ContractTypeResponse>> result = new ResponseModel<List<V3_ContractTypeResponse>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("/api/ContractTypeAPI/GetAll"), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_ContractTypeResponse>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<V3_ContractTypeResponse> GetByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                V3_ContractTypeResponse result = new V3_ContractTypeResponse();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ContractTypeAPI/GetByID?ID=" + ID, UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<V3_ContractTypeResponse>>(jsonResponse);
                    result = respMode.Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(V3_ContractTypeResponse entity)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                entity.CreatedAt = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = entity, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ContractTypeAPI/Insert";
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

        public string Update(V3_ContractTypeResponse entity)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                entity.UpdatedAt = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = entity, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ContractTypeAPI/Update";
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
                V3_ContractTypeResponse req = new V3_ContractTypeResponse();
                req.ID = ID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ContractTypeAPI/Delete";
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