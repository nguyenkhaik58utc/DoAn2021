using iDAS.Web.Areas.Human.Models;
using ISO.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace iDAS.Web.API.Human
{
    public class V3_HumanProfileInsuranceAPI
    {
        private string baseUrl = Ultilities.APIServer;

        /// <summary>
        /// Lấy danh sách 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<V3_HumanProfileInsuranceDTO>>> GetByEmployeeID(int employeeID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<V3_HumanProfileInsuranceDTO>> result = new ResponseModel<List<V3_HumanProfileInsuranceDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                       RequestUri = new Uri(string.Format("api/HumanProfileInsuranceAPI/GetAllByEmployeeID?employeeID=" + employeeID), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_HumanProfileInsuranceDTO>>>(jsonResponse);
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