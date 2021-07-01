using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using iDAS.Web.Areas.Problem.Models.RelativePeople;
using iDAS.Web.Models;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using ISO.API.Models;

namespace iDAS.Web.API.Problem
{
    public class ProblemRelativePeopleAPI
    {
        private string baseUrl;

        public ProblemRelativePeopleAPI()
        {
            baseUrl = Ultilities.APIServer;
        }

        public async Task<List<ProblemRelativePeopleDTO>> GetListProblemRelativePeople(int problemEventId, string rdbList)
        {
            try
            {
                // Lay ra tat ca cac nhan su lien quan den su co.
                List<ProblemRelativePeopleDTO> result = new List<ProblemRelativePeopleDTO>();
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
                        // test tam thoi. Debug
                        //RequestUri = new Uri(string.Format("api/ProblemEventUserAPI/GetByProblemEvent?problemEventID={0}", problemEventId), UriKind.Relative)
                        // end.
                        RequestUri = new Uri(string.Format("api/ProblemEventUserAPI/GetByProblemEvent?problemEventID={0}&listType={1}", problemEventId, rdbList), UriKind.Relative)
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemRelativePeopleDTO>>>(jsonResponse).Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public int InsertListProblemRelativePeople(ProblemRelativePeopleInsUpdListDTO lstProblemRelativePeople)
        public string InsertListProblemRelativePeople(int problemEventID, string lstSelectedRole, int deptId, string lstHumanEmployeeId)
        {

            try
            {
                // Them cac nhan su lien quan den su co.
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    ProblemRelativePeopleInsReqModelDTO paramRequestModel = new ProblemRelativePeopleInsReqModelDTO()
                    {
                        problemEventID = problemEventID,
                        lstSelectedRole = lstSelectedRole,
                        deptId = deptId,
                        lstHumanEmployeeId = lstHumanEmployeeId
                    };
                    object param = new { data = paramRequestModel, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemEventUserAPI/InsertUpdate";
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

        public string DeleteListProblemRelativePeople(string lstDeletedRelatePeople)
        {
            try
            {
                // Xoa nhan su lien quan den su co.
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = lstDeletedRelatePeople, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemEventUserAPI/Delete";
                    var response = client.PutAsync(uri, content);
                    //var jsonResponse =  response.Content.ReadAsStringAsync();
                    var jsonResponse = response.Result.Content.ReadAsStringAsync();
                    return jsonResponse.Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<HumanRoleHumanDepartmentHumanEmployeeDTO> GetListRoleDepartmentFromListHumanEmployeeID(string lstHumanEmployeeIDs)
        {
            try
            {
                // Lay ra tat ca cac chuc danh, phong ban tu danh sach HumanEmployeeID
                List<HumanRoleHumanDepartmentHumanEmployeeDTO> rsl = new List<HumanRoleHumanDepartmentHumanEmployeeDTO>();
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    RequestModelBusinesslstHumanEmployeeIDs paramRequestModel = new RequestModelBusinesslstHumanEmployeeIDs() { lstHumanEmployeeIDs = lstHumanEmployeeIDs };
                    object param = new { data = paramRequestModel, signature = base64String };
                    var uri = "api/ProblemEventUserAPI/GetListRoleDepartmentFromListHumanEmployeeID";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    HttpResponseMessage result = client.PostAsync(uri, content).Result;
                    var rs = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //Console.WriteLine(rs);
                    var model = JsonConvert.DeserializeObject<ResponseModelList<HumanRoleHumanDepartmentHumanEmployeeDTO>>(rs);
                    rsl = model.Data;
                }
                return rsl;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}