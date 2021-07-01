using iDAS.Web.Areas.Human.Model;
using iDAS.Web.Areas.Human.Models;
using ISO.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace iDAS.Web.API
{
    public class HumanStatisticAPI
    {
        private string baseUrl = Ultilities.APIServer;

        // Thống kê nhân sự theo phòng ban
        public async Task<List<EmployeeTotal>> GetEmployeeTotal()
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                List<EmployeeTotal> result = new List<EmployeeTotal>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/StatisticsEmployeeAPI/GetEmployeeTotal", UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<List<EmployeeTotal>>>(jsonResponse);
                    result = respMode.Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Chi tiết thống kê nhân sự theo phòng ban
        public async Task<ResponseModel<List<V3_HumanEmployeeResponse>>> GetListEmp(int pageSize, int pageNumber, int depID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<V3_HumanEmployeeResponse>> result = new ResponseModel<List<V3_HumanEmployeeResponse>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("/api/StatisticsEmployeeAPI/GetListEmp?depID={0}&pageSize={1}&pageNumber={2}", depID, pageSize, pageNumber), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_HumanEmployeeResponse>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Thống kê nhân sự theo chức danh
        public async Task<List<EmployeeTitleTotal>> GetEmployeeTitleTotal()
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                List<EmployeeTitleTotal> result = new List<EmployeeTitleTotal>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/StatisticsEmployeeAPI/GetEmployeeTitleTotal", UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<List<EmployeeTitleTotal>>>(jsonResponse);
                    result = respMode.Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Chi tiết thống kê nhân sự theo chức danh
        public async Task<ResponseModel<List<V3_HumanEmployeeResponse>>> GetListEmpByTitle(int pageSize, int pageNumber, int titleID, int depID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<V3_HumanEmployeeResponse>> result = new ResponseModel<List<V3_HumanEmployeeResponse>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("/api/StatisticsEmployeeAPI/GetListEmpByTitle?titleID={0}&depID={1}&pageSize={2}&pageNumber={3}", titleID, depID, pageSize, pageNumber), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_HumanEmployeeResponse>>>(jsonResponse);
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