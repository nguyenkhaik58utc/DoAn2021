using iDAS.Items;
using iDAS.Items.Position;
using iDAS.Web.Areas.Customer.Models;
using iDAS.Web.Areas.Department;
using iDAS.Web.Areas.Position.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
namespace iDAS.Web.API.ManagementLevel
{
    public class ManagementLevelAPI
    {
        public async Task<ManagementLevelRespModel> GetManagementLevelAPI(int page, int pageSize)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ManagementLevelItem> ManagementLevelInfo = new List<ManagementLevelItem>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/managementlevelAPI/getall", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    ManagementLevelInfo = JsonConvert.DeserializeObject<List<ManagementLevelItem>>(data.data.ToString());
                    ManagementLevelRespModel result = new ManagementLevelRespModel();
                    result.lstManagementItem = ManagementLevelInfo;
                    result.total = ManagementLevelInfo.Count;
                    return result;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ManagementLevelRespModel> GetManagementLevelById(int ID)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ManagementLevelItem> ManagementLevelInfo = new List<ManagementLevelItem>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/managementlevelApi/GetByID?ID=" + ID, UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    ManagementLevelInfo = JsonConvert.DeserializeObject<List<ManagementLevelItem>>(data.data.ToString());
                    ManagementLevelRespModel result = new ManagementLevelRespModel();
                    result.lstManagementItem = ManagementLevelInfo;
                    result.total = ManagementLevelInfo.Count;
                    return result;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Create(ManagementLevelItem managementLevel)
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

                    var content = new StringContent(JsonConvert.SerializeObject(managementLevel), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/managementLevelApi/create";
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

        public string Update(ManagementLevelItem managementLevel)
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

                    var content = new StringContent(JsonConvert.SerializeObject(managementLevel), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/managementlevelApi/update";
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

        public async Task<int> delete(int managementLevelId)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                ManagementLevelItem ManagementLevelInfo = new ManagementLevelItem();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri("api/managementlevelApi/Delete?ID=" + managementLevelId, UriKind.Relative)
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
