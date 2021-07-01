using iDAS.Web.Areas.Department.Models;
using ISO.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iDAS.Web.API
{
    public class DepartmentTitleAPI
    {
        private string baseUrl;

        public DepartmentTitleAPI()
        {
            baseUrl = Ultilities.APIServer;
        }

        public string Insert(DepartmentTitle departmentTitle, int userId)
        {
            try
            {
                departmentTitle.UserID = userId;
                departmentTitle.IsActive = true;

                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = departmentTitle, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/DepartmentTitleAPI/Insert";
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

        public List<DepartmentTitle> GetByDepartment(int departmentID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                List<DepartmentTitle> lstResult = new List<DepartmentTitle>();
                DepartmentTitleReqModel req = new DepartmentTitleReqModel();
                req.DepartmentID = departmentID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = req, signature = base64String };
                    var uri = string.Format("api/DepartmentTitleAPI/GetByDepartment");
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    lstResult = JsonConvert.DeserializeObject<ResponseModel<List<DepartmentTitle>>>(jsonResponse).Data;
                }
                return lstResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DepartmentTitle>> GetByTitle(int titleID)
        {
            try
            {
                List<DepartmentTitle> lstResult = new List<DepartmentTitle>();
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/DepartmentTitleAPI/GetByTitle?titleID={0}", titleID), UriKind.Relative)
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    lstResult = JsonConvert.DeserializeObject<ResponseModel<List<DepartmentTitle>>>(jsonResponse).Data;
                }
                return lstResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Lấy danh sách chức danh chưa được gán vào đơn vị
        // nếu departmentID = 0 thì getall chức danh
        public async Task<List<TitleDTO>> GetTitleForCreate(int departmentID)
        {
            try
            {
                List<TitleDTO> lstResult = new List<TitleDTO>();
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/TitleAPI/GetNotAsDepartment?departmentID={0}", departmentID), UriKind.Relative)
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ResponseModel<List<TitleDTO>>>(jsonResponse).Data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Delete(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                DepartmentTitleReqModel req = new DepartmentTitleReqModel();
                req.ID = ID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = string.Format("api/DepartmentTitleAPI/delete");
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