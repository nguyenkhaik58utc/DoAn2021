using iDAS.Web.Areas.Department.Models;
using iDAS.Web.Areas.Document.Models;
using iDAS.Web.Areas.Human.Models;
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
    public class DocumentDepartmentAPI
    {
        private string baseUrl = Ultilities.APIServer;

        /// <summary>
        /// Lấy danh sách phòng ban có quyền truy document
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<DocumentDepartmentDTO>>> GetListByDocumentID(int documentID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<List<DocumentDepartmentDTO>> result = new ResponseModel<List<DocumentDepartmentDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/DocumentDepartmentAPI/GetListByDocumentID?documentID=" + documentID), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<DocumentDepartmentDTO>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lưu quyên truy cập theo phong
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SavePermission(DocumentDepartmentSaveRequest entity)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = entity, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/DocumentDepartmentAPI/SavePermission";
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