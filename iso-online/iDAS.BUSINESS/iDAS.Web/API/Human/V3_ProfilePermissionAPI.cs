using iDAS.Web.Areas.Department.Models;
using iDAS.Web.Areas.Human.Model;
using ISO.API.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using iDAS.Web.Areas.Human.Model;
using System.Collections.Generic;
using System.Text;

namespace iDAS.Web.API.Human
{
    public class V3_ProfilePermissionAPI
    {
        private string baseUrl = Ultilities.APIServer;

        /// <summary>
        /// Lấy danh sách quyền theo tác hồ sơ lý lịch theo EmployeeID
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public ResponseModel<List<V3_CheckProfileHumanPermissionResponse>> GetListPermissionByEmployeeID(int HumanEmployeeID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                V3_ProfilePermissionByEmployeeIDRequest req = new V3_ProfilePermissionByEmployeeIDRequest();
                req.HumanEmployeeID = HumanEmployeeID;

                ResponseModel<List<V3_CheckProfileHumanPermissionResponse>> result = new ResponseModel<List<V3_CheckProfileHumanPermissionResponse>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProfileHumanPermissionAPI/CheckProfileHumanPermission";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_CheckProfileHumanPermissionResponse>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách quyền thao tác hồ sơ lý lịch của user
        /// </summary>
        /// <param name="DepartmentTitleFromID"></param>
        /// <returns></returns>
        public ResponseModel<List<V3_ProfilePermissionResponse>> GetAllPermission(int DepartmentTitleFromID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                V3_ProfilePermissionRequest req = new V3_ProfilePermissionRequest();
                req.DepartmentTitleFromID = DepartmentTitleFromID;

                ResponseModel<List<V3_ProfilePermissionResponse>> result = new ResponseModel<List<V3_ProfilePermissionResponse>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProfileHumanPermissionAPI/GetAllByDepartmentTitle";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<V3_ProfilePermissionResponse>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lưu lại quyền thao tác hồ sơ lý lịch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool SaveProfilePermission(V3_ProfilePermissionSaveRequest request)
        {
            ///////////////////
            try
            {
                string base64String = Ultilities.GetTokenString();

                ResponseModel<List<V3_ProfilePermissionResponse>> result = new ResponseModel<List<V3_ProfilePermissionResponse>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = request, signature = base64String };
                    var uri = "api/ProfileHumanPermissionAPI/SaveProfilePermission";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //result = JsonConvert.DeserializeObject<ResponseModel<List<ProfilePermissionResponse>>>(jsonResponse);
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