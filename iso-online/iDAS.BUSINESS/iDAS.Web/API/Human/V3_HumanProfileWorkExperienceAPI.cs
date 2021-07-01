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
    /// Hồ sơ quá trình công tác
    /// </summary>
    public class V3_HumanProfileWorkExperienceAPI
    {
        private string baseUrl = Ultilities.APIServer;

        /// <summary>
        /// Lấy danh sách 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<V3_HumanProfileWorkExperienceDTO>>> GetByEmployeeID(int employeeID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<V3_HumanProfileWorkExperienceDTO>> result = new ResponseModel<List<V3_HumanProfileWorkExperienceDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/HumanProfileWorkExperienceAPI/GetByEmployeeID?employeeID=" + employeeID), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_HumanProfileWorkExperienceDTO>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy danh sách theo list EmployeeID
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<V3_HumanProfileWorkExperienceDTO>>> GetByListEmployeeID(List<int> lstEmpID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<V3_HumanProfileWorkExperienceDTO>> result = new ResponseModel<List<V3_HumanProfileWorkExperienceDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string output = jss.Serialize(lstEmpID);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                       RequestUri = new Uri(string.Format("api/HumanProfileWorkExperienceAPI/GetByListEmployeeID?lstEmployeeID=" + output), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_HumanProfileWorkExperienceDTO>>>(jsonResponse);
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