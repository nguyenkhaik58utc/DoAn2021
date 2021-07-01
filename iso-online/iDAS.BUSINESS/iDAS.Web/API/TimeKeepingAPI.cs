using iDAS.Web.Areas.Timekeeping.Models;
using iDAS.Web.Areas.Timekeeping.Models.TimeKeeping;
using ISO.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace iDAS.Web.API
{
    public class TimeKeepingAPI
    {
        private string baseUrl = Ultilities.APIServer;

        /// <summary>
        /// Lấy danh sách hàng tháng
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<TimeKeepingOfEmployeeViewModel>>> GetByMonth(GetTimeKeepingByMonthRequest request)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<TimeKeepingOfEmployeeViewModel>> result = new ResponseModel<List<TimeKeepingOfEmployeeViewModel>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var httpRequest = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/TimeKeepingAPI/GetByMonth?Month=" + request.Month
                        + "&Year=" + request.Year + "&EmployeeId=" + request.EmployeeId), UriKind.Relative)
                    };
                    var response = await client.SendAsync(httpRequest).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<TimeKeepingOfEmployeeViewModel>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<WorkOutDTO>> GetById(int id)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<WorkOutDTO> result = new ResponseModel<WorkOutDTO>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var httpRequest = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format($"api/WorkOutAPI/GetById?id=" + id), UriKind.Relative)
                    };
                    var response = await client.SendAsync(httpRequest).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<WorkOutDTO>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy danh sách hàng tháng
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<TimeKeepingExplanationViewModel>>> GetExplanationByMonth(GetTimeKeepingExplanationByMonthRequest request)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<TimeKeepingExplanationViewModel>> result = new ResponseModel<List<TimeKeepingExplanationViewModel>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var httpRequest = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/TimeKeepingAPI/GetExplanationByMonth?Month=" + request.Month
                        + "&Year=" + request.Year + "&Status=" + request.Status), UriKind.Relative)
                    };
                    var response = await client.SendAsync(httpRequest).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<TimeKeepingExplanationViewModel>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xóa
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<int>> DeleteExplanation(int id)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<int> result = new ResponseModel<int>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(string.Format("api/TimeKeepingAPI/DeleteExPlanation?id=" + id), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<int>>(jsonResponse);
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