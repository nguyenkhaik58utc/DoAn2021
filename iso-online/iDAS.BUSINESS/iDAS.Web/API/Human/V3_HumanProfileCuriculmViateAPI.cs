using iDAS.Web.Areas.Human.Models;
using iDAS.Web.Areas.Human.Models.V3_Category;
using ISO.API.Models;
using Newtonsoft.Json;
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
    public class V3_HumanProfileCuriculmViateAPI
    {
        private string baseUrl = Ultilities.APIServer;
        /// <summary>
        /// Lấy hồ sơ lý lịch theo ID
        /// </summary>
        /// <returns></returns>
        public async Task<V3_HumanProfileCuriculmViateResponse> GetByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
              V3_HumanProfileCuriculmViateResponse result = new V3_HumanProfileCuriculmViateResponse();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("/api/HumanProfileCuriculmViateAPI/GetByHumanEmployeeID?ID=" + ID), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<V3_HumanProfileCuriculmViateResponse>>(jsonResponse);
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
        /// Lấy hồ sơ lý lịch theo ID
        /// </summary>
        /// <returns></returns>
        public async Task<V3_HumanProfileCuriculmViateReportResponse> GetFullByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                V3_HumanProfileCuriculmViateReportResponse result = new V3_HumanProfileCuriculmViateReportResponse();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("/api/HumanProfileCuriculmViateAPI/GetFullByHumanEmployeeID?ID=" + ID), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<V3_HumanProfileCuriculmViateReportResponse>>(jsonResponse);
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
        /// Lấy danh sách thông tin chung list employeeID
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<HumanCommonInformationDTO>>> GetCommonInformationByListEmployeeID(List<int> lstEmpID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<HumanCommonInformationDTO>> result = new ResponseModel<List<HumanCommonInformationDTO>>();
                JavaScriptSerializer jss = new JavaScriptSerializer();

                string output = jss.Serialize(lstEmpID);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/HumanProfileCuriculmViateAPI/GetHumanCommonInformationByListEmployeeID?lstID="+ output), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<HumanCommonInformationDTO>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy danh sách thông tin lý lịch chính trị theo list employeeID
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<HumanPoliticInformationDTO>>> GetPoliticInformationByListEmployeeID(List<int> lstEmpID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<HumanPoliticInformationDTO>> result = new ResponseModel<List<HumanPoliticInformationDTO>>();
                JavaScriptSerializer jss = new JavaScriptSerializer();

                string output = jss.Serialize(lstEmpID);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/HumanProfileCuriculmViateAPI/GetHumanPoliticInformationByListEmployeeID?lstID=" + output), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<HumanPoliticInformationDTO>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Lấy hồ sơ lý lịch theo ID
        /// </summary>
        /// <returns></returns>
        public async Task<V3_HumanProfileCuriculmViateResponse> GetDetailByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                V3_HumanProfileCuriculmViateResponse result = new V3_HumanProfileCuriculmViateResponse();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("/api/HumanProfileCuriculmViateAPI/GetDetailByID?ID=" + ID), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<V3_HumanProfileCuriculmViateResponse>>(jsonResponse);
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
        /// Lưu lại hồ sơ lý lịch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool SaveHumanProfileCuricilmViate(V3_HumanProfileCuriculmViateResponse request)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = request, signature = base64String };
                    var uri = "api/HumanProfileCuriculmViateAPI/Save";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

