using iDAS.Items;
using iDAS.Web.Areas.Customer.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.API
{
    public class CustomerAPI
    {
        
        [HttpPost]
        public async Task<CustomerRespModel> GetCustomerAPI(int page, int pageSize)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<CustomerItem> EmpInfo = new List<CustomerItem>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/customers/getall", UriKind.Relative),
                        //Content = new StringContent(JsonConvert.SerializeObject(reqModel), Encoding.UTF8, BasicConsts.ApplicationJson)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    EmpInfo = JsonConvert.DeserializeObject<List<CustomerItem>>(jsonResponse);
                    CustomerRespModel result = new CustomerRespModel();
                    result.lstCus = EmpInfo;
                    result.total = EmpInfo.Count;
                    return result;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}