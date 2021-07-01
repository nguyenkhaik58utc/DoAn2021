using System;
using System.Collections.Generic;
using System.Text;
using iDAS.Web.Areas.Department.Models.TitleMenuRoleV3;
using System.Net.Http;
using Newtonsoft.Json;
using iDAS.Web.Models;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using ISO.API.Models;

namespace iDAS.Web.API.DepartmentV3
{
    public class TitleMenuRoleV3API
    {
        private string baseUrl;

        public TitleMenuRoleV3API()
        {
            baseUrl = Ultilities.APIServer;
        }

        public List<DepartmentTitleDTO> GetAllTitlesByDepartmentID(int departmentID)
        {
            try
            {
                // Lay ra tat ca cac chuc danh trong mot phong ban cu the.
                string base64String = Ultilities.GetTokenString();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    DepartmentTitleRequestModel paramRequestModel = new DepartmentTitleRequestModel() { departmentID = departmentID };
                    object param = new { data = paramRequestModel, signature = base64String };
                    var uri = "api/DepartmentTitleAPI/GetByDepartment";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    HttpResponseMessage result = client.PostAsync(uri, content).Result;
                    var rs = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //Console.WriteLine(rs);
                    var model = JsonConvert.DeserializeObject<ResponseModelList<DepartmentTitleDTO>>(rs);
                    return model.Data;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<V3DepartmentTitleMenuDTO> GetMenuRoleByDepartmentIdAndTitleId(int departmentID, int titleID)
        {
            try
            {
                // Lay ra quyen menu tu chuc danh va phong ban.
                string base64String = Ultilities.GetTokenString();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    MenuRoleRequestModel paramRequestModel = new MenuRoleRequestModel() { departmentID = departmentID, titleID = titleID };
                    object param = new { data = paramRequestModel, signature = base64String };
                    var uri = "api/DepartmentTitleMenuAPI/GetMenuByDepartmentAndTitle";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    HttpResponseMessage result = client.PostAsync(uri, content).Result;
                    var rs = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //Console.WriteLine(rs);
                    var model = JsonConvert.DeserializeObject<ResponseModelList<V3DepartmentTitleMenuDTO>>(rs);
                    return model.Data;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TreeRollMenuV3Model>> GetTreeRollMenuV3API()
        {
            try
            {
                // Lay ra tat ca bang v3OrgMenu theo cau truc cay cha con.
                List<TreeRollMenuV3Model> result = new List<TreeRollMenuV3Model>();
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
                        
                        RequestUri = new Uri("api/DepartmentTitleMenuAPI/GetAllTreeRollMenuV3", UriKind.Relative)
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<TreeRollMenuV3Model>>>(jsonResponse).Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateMenuRoleDepartmentTitle(CRUDDepartmentTitleMenuV3 item)
        {
            try
            {
                // Luu lai tat ca phan quyen menu theo chuc danh cho user vua duoc thay doi.
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = item, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/DepartmentTitleMenuAPI/UpdateMenuRoleDepartmentTitle";
                    var response = client.PutAsync(uri, content);
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

        public List<v3OrgMenuDTO> GetBusinessModuleByUserID (int userID)
        {
            try 
            {
                // Lay ra cac menu Module duoc phan quyen theo chuc danh cua user hien tai.
                List<v3OrgMenuDTO> modules = new List<v3OrgMenuDTO>();
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    RequestModelBusinessModuleV3OrgMenu paramRequestModel = new RequestModelBusinessModuleV3OrgMenu() { userID = userID };
                    object param = new { data = paramRequestModel, signature = base64String };
                    var uri = "api/MenuAPI/GetBusinessModule";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    HttpResponseMessage result = client.PostAsync(uri, content).Result;
                    var rs = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //Console.WriteLine(rs);
                    var model = JsonConvert.DeserializeObject<ResponseModelList<v3OrgMenuDTO>>(rs);
                    modules = model.Data;
                }
                return modules;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<v3OrgMenuDTO> GetBusinessFunctionByUserIdAndModuleCode(int userID, string moduleCode)
        {
            try
            {
                // Lay ra cac menu Function duoc phan quyen theo chuc danh cua user hien tai.
                List<v3OrgMenuDTO> functions = new List<v3OrgMenuDTO>();
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    RequestModelBusinessFunctionV3OrgMenu paramRequestModel = new RequestModelBusinessFunctionV3OrgMenu() { userID = userID, ModuleCode = moduleCode };
                    object param = new { data = paramRequestModel, signature = base64String };
                    var uri = "api/MenuAPI/GetBusinessFunction";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    HttpResponseMessage result = client.PostAsync(uri, content).Result;
                    var rs = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //Console.WriteLine(rs);
                    var model = JsonConvert.DeserializeObject<ResponseModelList<v3OrgMenuDTO>>(rs);
                    functions = model.Data;
                }
                return functions;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string CopyMenuRoleDepartmentTitle(CopyMenuTitleRoleV3 item)
        {
            try
            {
                // Copy quyen trong phan quyen menu theo chuc danh.
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = item, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/DepartmentTitleMenuAPI/CopyMenuTitleRoleV3";
                    var response = client.PutAsync(uri, content);
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

    }
}