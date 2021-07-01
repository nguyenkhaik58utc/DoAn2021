using iDAS.Web.Areas.Human.Models;
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
using System.Web.Script.Serialization;

namespace iDAS.Web.API.Human
{
    /// <summary>
    /// Hồ sơ  văn bằng
    /// </summary>
    public class V3_HumanProfileDiplomaAPI
    {
        private string baseUrl = Ultilities.APIServer;
        /// <summary>
        /// Lấy danh sách hồ
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<V3_HumanProfileDiplomaResponse>>> GetByEmployeeID(int employeeID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<V3_HumanProfileDiplomaResponse>> result = new ResponseModel<List<V3_HumanProfileDiplomaResponse>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/HumanProfileDiplomaAPI/GetByEmployeeID?employeeID=" + employeeID), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_HumanProfileDiplomaResponse>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy danh sách hồ
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<V3_HumanProfileDiplomaResponse>>> GetPagingByEmployeeID(int pageNumber, int pageSize, int employeeID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<V3_HumanProfileDiplomaResponse>> result = new ResponseModel<List<V3_HumanProfileDiplomaResponse>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/HumanProfileDiplomaAPI/GetPagingByEmployeeID?pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&employeeID=" + employeeID), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_HumanProfileDiplomaResponse>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Lấy danh sách hồ sơ theo danh sách EmployeeID
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<HumanProfileDiplomaExcel>>> GetByListEmployeeID(List<int> lstEmpID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<HumanProfileDiplomaExcel>> result = new ResponseModel<List<HumanProfileDiplomaExcel>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string output = jss.Serialize(lstEmpID);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                       RequestUri = new Uri(string.Format("api/HumanProfileDiplomaAPI/GetListDiplomaByEmpID?lstEmployeeID=" + output), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<HumanProfileDiplomaExcel>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// lấy theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<V3_HumanProfileDiplomaDTO> GetByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                V3_HumanProfileDiplomaDTO result = new V3_HumanProfileDiplomaDTO();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanProfileDiplomaAPI/GetByID?ID=" + ID, UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<V3_HumanProfileDiplomaDTO>>(jsonResponse);
                    result = respMode.Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<V3_HumanProfileDiplomaResponse> GetDetailByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                V3_HumanProfileDiplomaResponse result = new V3_HumanProfileDiplomaResponse();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanProfileDiplomaAPI/GetDetailByID?ID=" + ID, UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<V3_HumanProfileDiplomaResponse>>(jsonResponse);
                    result = respMode.Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string Insert(V3_HumanProfileDiplomaDTO entity, int userID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                entity.CreatedAt = DateTime.Now;
                entity.CreatedBy = userID;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = entity, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/HumanProfileDiplomaAPI/Insert";
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
        /// cập nhật
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string Update(V3_HumanProfileDiplomaDTO entity, int userID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedBy = userID;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = entity, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/HumanProfileDiplomaAPI/Update";
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

        //xóa
        public async Task<int> Delete(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                V3_HumanProfileDiplomaDTO req = new V3_HumanProfileDiplomaDTO();
                req.ID = ID;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/HumanProfileDiplomaAPI/Delete";
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