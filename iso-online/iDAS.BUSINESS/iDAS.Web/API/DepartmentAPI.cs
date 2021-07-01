using iDAS.Web.Areas.Department.Models;
using ISO.API.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace iDAS.Web.API
{
    public class DepartmentAPI
    {
        private string baseUrl = Ultilities.APIServer;
      
        public async Task<DepartmentDTO> GetByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                DepartmentDTO result = new DepartmentDTO();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/HumanDepartmentAPI/GetByID?depID={0}", ID), UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<DepartmentDTO>>(jsonResponse);
                    result = respMode.Data;
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