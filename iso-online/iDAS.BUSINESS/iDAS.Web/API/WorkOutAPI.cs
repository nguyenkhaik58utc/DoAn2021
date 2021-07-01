using iDAS.Web.Areas.Timekeeping.Models;
using ISO.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Web.API
{
    public class WorkOutAPI
    {
        private string baseUrl = Ultilities.APIServer;

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string Insert(InsertWorkOutRequest request)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = request, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/WorkOutAPI/Insert";
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

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string Update(UpdateWorkOutRequest request)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = request, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/WorkOutAPI/Update";
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

        /// <summary>
        /// Xóa
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<int>> Delete(int id)
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
                        RequestUri = new Uri(string.Format("api/WorkOutAPI/Delete?id=" + id), UriKind.Relative)
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

        /// <summary>
        /// kiểm tra tồn tại
        /// </summary>
        /// <returns></returns>
        public async Task<int> CheckExist(CheckExistWorkOutRequest request)
        {
            //try
            //{
            //    string base64String = Ultilities.GetTokenString();
            //    ResponseModel<int> result = new ResponseModel<int>();

            //    using (var client = new HttpClient())
            //    {
            //        client.BaseAddress = new Uri(baseUrl);
            //        client.DefaultRequestHeaders.Add("signature", base64String);

            //        var httpRequest = new HttpRequestMessage
            //        {
            //            Method = HttpMethod.Get,
            //            RequestUri = new Uri(string.Format("api/WorkOutAPI/CheckExist?EmployeeId=" + request.EmployeeId
            //            + "&StartTime=" + request.StartTime + "&EndTime=" + request.EndTime), UriKind.Relative)
            //        };
            //        var response = await client.SendAsync(httpRequest).ConfigureAwait(false);
            //        var jsonResponse = await response.Content.ReadAsStringAsync();
            //        result = JsonConvert.DeserializeObject<ResponseModel<int>>(jsonResponse);
            //    }
            //    return result.Data;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<int> result = new ResponseModel<int>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = request, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/WorkOutAPI/CheckExist";
                    var response = client.PostAsync(uri, content);
                    var jsonResponse = response.Result.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<ResponseModel<int>>(jsonResponse);
                    return result.Data;
                }
            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }
        }

        /// <summary>
        /// Lấy danh sách hàng tháng
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<WorkOutViewModel>>> GetByMonth(GetWorkOutByMonthRequest request)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<WorkOutViewModel>> result = new ResponseModel<List<WorkOutViewModel>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var httpRequest = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/WorkOutAPI/GetByMonth?Month=" + request.Month
                        + "&Year=" + request.Year + "&Status=" + request.Status
                        + "&PageSize=" + request.PageSize + "&PageNumber=" + request.PageNumber + "&EmployeeId=" + request.EmployeeId), UriKind.Relative)
                    };
                    var response = await client.SendAsync(httpRequest).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<WorkOutViewModel>>>(jsonResponse);
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
    }
}