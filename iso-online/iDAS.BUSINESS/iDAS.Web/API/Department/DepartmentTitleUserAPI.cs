
using iDAS.Core;
using iDAS.Items.Department;
using iDAS.Items.Problem;
using iDAS.Web.Areas.Department.Models;
using iDAS.Web.Areas.Problem.Models;
using ISO.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace iDAS.Web.API.Problem
{
    public class DepartmentTitleUserAPI
    {
        private string baseUrl = Ultilities.APIServer;
        public ResponseModel<List<DepartmentTitleUserItem>> GetTitleOfEmployee(int userId)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                DepartmentTitleUserItem req = new DepartmentTitleUserItem();
                req.HumanEmployeeID = userId;

                ResponseModel<List<DepartmentTitleUserItem>> result = new ResponseModel<List<DepartmentTitleUserItem>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/DepartmentTitleUserAPI/GetTitleOfEmployee";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<DepartmentTitleUserItem>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<DepartmentTitleGetByDepEmpModel>> GetByDepAndEmp(int depId, int userId)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                DepartmentTitleUserItem req = new DepartmentTitleUserItem();
                req.HumanEmployeeID = userId;
                req.DepartmentTitleID = depId;
                req.CreatedBy = userId;
                req.CreatedAt = DateTime.Now;

                List<DepartmentTitleGetByDepEmpModel> result = new List<DepartmentTitleGetByDepEmpModel>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/DepartmentTitleAPI/GetByDepAndEmp?depID=" + depId + "&empID=" + userId), UriKind.Relative)
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    result = JsonConvert.DeserializeObject<List<DepartmentTitleGetByDepEmpModel>>(data.data.ToString());
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Insert(int departmentTitleId, int userId)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                DepartmentTitleUserInsUpdModel insUpdModel = new DepartmentTitleUserInsUpdModel();
                insUpdModel.DepartmentTitleID = departmentTitleId;
                insUpdModel.HumanEmployeeID = userId;
                insUpdModel.UpdatedAt = DateTime.Now;
                insUpdModel.CreatedAt = DateTime.Now;
                insUpdModel.UpdatedBy = userId;
                insUpdModel.CreatedBy = userId;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = insUpdModel, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/DepartmentTitleUserAPI/InsertUpdate";
                    var response = client.PostAsync(uri, content);
                    //var jsonResponse =  response.Content.ReadAsStringAsync();
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

        public string Delete(int departmentTitleId, int userId)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                DepartmentTitleUserDeleteReqModel req = new DepartmentTitleUserDeleteReqModel();
                req.DepartmentTitleID = departmentTitleId;
                req.HumanEmployeeID = userId;
                req.UpdatedAt = DateTime.Now;
                req.UpdatedBy = userId;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = string.Format("api/DepartmentTitleUserAPI/DeleteByDepartmentTitleAndUser");
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
    }
}