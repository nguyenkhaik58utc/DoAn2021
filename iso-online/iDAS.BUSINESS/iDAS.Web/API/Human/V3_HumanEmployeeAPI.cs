using iDAS.Web.Areas.Document.Models;
using iDAS.Web.Areas.Human.Model;
using iDAS.Web.Areas.Human.Models;
using ISO.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace iDAS.Web.API.Human
{
    public class V3_HumanEmployeeAPI
    {
        private string baseUrl = Ultilities.APIServer;

        //1. lấy danh sách nhân viên theo phòng ban
        public ResponseModel<List<V3_HumanEmployeeResponse>> GetListByDepartment(HumanEmployeeSearchRequest req)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ResponseModel<List<V3_HumanEmployeeResponse>> result = new ResponseModel<List<V3_HumanEmployeeResponse>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/HumanEmployeeAPI/GetListByDepartmentID";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_HumanEmployeeResponse>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseModel<List<DocumentEmployeeViewModel>>> GetByListEmployeeId(List<int> lstEmpID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ResponseModel<List<DocumentEmployeeViewModel>> result = new ResponseModel<List<DocumentEmployeeViewModel>>();
                JavaScriptSerializer jss = new JavaScriptSerializer();

                string output = jss.Serialize(lstEmpID);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/HumanEmployeeAPI/GetByListEmployeeId?lstID=" + output), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<DocumentEmployeeViewModel>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseModel<List<DocumentEmployeeViewModel>>> GetByDocument(int documentId)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ResponseModel<List<DocumentEmployeeViewModel>> result = new ResponseModel<List<DocumentEmployeeViewModel>>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/HumanEmployeeAPI/GetByDocument?documentId=" + documentId), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<DocumentEmployeeViewModel>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseModel<V3_HumanEmployeeResponse>> GetById(int id)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ResponseModel<V3_HumanEmployeeResponse> result = new ResponseModel<V3_HumanEmployeeResponse>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/HumanEmployeeAPI/GetById?id=" + id), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<V3_HumanEmployeeResponse>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}