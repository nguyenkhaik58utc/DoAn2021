
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
    public class ProblemResidentAgencyGroupAPI
    {
        private string baseUrl = Ultilities.APIServer;
        public ResponseModel<List<ProblemResidentAgencyGroupDTO>> GetData(int page, int pageSize)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemResidentAgencyGroupReqModel req = new ProblemResidentAgencyGroupReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;

                ResponseModel<List<ProblemResidentAgencyGroupDTO>> result = new ResponseModel<List<ProblemResidentAgencyGroupDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProblemResidentAgencyGroupAPI/GetPaging";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemResidentAgencyGroupDTO>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProblemResidentAgencyGroupDTO> GetByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ProblemResidentAgencyGroupDTO result = new ProblemResidentAgencyGroupDTO();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ProblemResidentAgencyGroupAPI/GetByID?ID=" + ID, UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<ProblemResidentAgencyGroupDTO>>(jsonResponse);
                    result = respMode.Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(ProblemResidentAgencyGroupDTO agencyGroupDTO)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = agencyGroupDTO, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemResidentAgencyGroupAPI/InsertUpdate";
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

        public string Update(ProblemResidentAgencyGroupDTO agencyGroupDTO)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = agencyGroupDTO, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemResidentAgencyGroupAPI/InsertUpdate";
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
                ProblemResidentAgencyGroupDTO req = new ProblemResidentAgencyGroupDTO();
                req.ID = ID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemResidentAgencyGroupAPI/delete";
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