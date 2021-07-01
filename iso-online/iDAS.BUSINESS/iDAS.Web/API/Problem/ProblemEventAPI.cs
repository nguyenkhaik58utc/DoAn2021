using iDAS.Web.Areas.Problem.Models;
using iDAS.Web.Models;
using ISO.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Web.API
{
    public class ProblemEventAPI
    {
        private string baseUrl = Ultilities.APIServer;

        public ResponseModel<List<ProblemEventListDTO>> GetData(int page, int pageSize, bool isTemplate = false, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int? type = null, int? group = null, int departmentID = 0)
        {
            try
            {
                if (toDate != null)
                    toDate = toDate.Value.AddDays(1).AddTicks(-1);

                string base64String = Ultilities.GetTokenString();

                ProblemEventReqModel req = new ProblemEventReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;
                req.IsProblemOrEvent = true;
                req.IsTemplate = isTemplate;
                if (type > 0)
                    req.ProblemTypeID = type;
                req.OccuredDateFrom = fromDate;
                req.OccuredDateTo = toDate;
                req.DepartmentIdFix = departmentID;

                if (group > 0)
                    req.ProblemGroupID = group;

                ResponseModel<List<ProblemEventListDTO>> result = new ResponseModel<List<ProblemEventListDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProblemEventAPI/GetPaging";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemEventListDTO>>>(jsonResponse);
                }
                if (result == null)
                    result = new ResponseModel<List<ProblemEventListDTO>>();
                if (result.Data == null)
                {
                    result.Data = new List<ProblemEventListDTO>();
                    result.TotalRows = 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseModel<List<ProblemEventListDTO>> GetList()
        {
            try
            {
                int page = 1;
                int pageSize = 20;
                bool isTemplate = false;
                Nullable<DateTime> fromDate = DateTime.Parse("01-01-2020");
                Nullable<DateTime> toDate = DateTime.Now;
                int? type = null;
                int? group = null;
                int departmentID = 0;
                if (toDate != null)
                    toDate = toDate.Value.AddDays(1).AddTicks(-1);

                string base64String = Ultilities.GetTokenString();

                ProblemEventReqModel req = new ProblemEventReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;
                req.IsProblemOrEvent = true;
                req.IsTemplate = isTemplate;
                if (type > 0)
                    req.ProblemTypeID = type;
                req.OccuredDateFrom = fromDate;
                req.OccuredDateTo = toDate;
                req.DepartmentIdFix = departmentID;

                if (group > 0)
                    req.ProblemGroupID = group;

                ResponseModel<List<ProblemEventListDTO>> result = new ResponseModel<List<ProblemEventListDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProblemEventAPI/GetPaging";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemEventListDTO>>>(jsonResponse);
                }
                if (result == null)
                    result = new ResponseModel<List<ProblemEventListDTO>>();
                if (result.Data == null)
                {
                    result.Data = new List<ProblemEventListDTO>();
                    result.TotalRows = 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProblemEventDTO GetByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemEventReqModel req = new ProblemEventReqModel();
                req.ID = ID;

                ProblemEventDTO result = new ProblemEventDTO();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProblemEventAPI/GetByID";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<ProblemEventDTO>>(jsonResponse).Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProblemTypeDTO>> GetTypeCbo()
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                List<ProblemTypeDTO> lstPosition = new List<ProblemTypeDTO>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ProblemTypeAPI/GetAll", UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    lstPosition = JsonConvert.DeserializeObject<ResponseModel<List<ProblemTypeDTO>>>(jsonResponse).Data;
                }
                return lstPosition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProblemGroupDTO>> GetGroupCbo(int typeID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                List<ProblemGroupDTO> lstResult = new List<ProblemGroupDTO>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ProblemGroupAPI/GetByProblemType?problemTypeID=" + typeID, UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    lstResult = JsonConvert.DeserializeObject<ResponseModel<List<ProblemGroupDTO>>>(jsonResponse).Data;
                }
                return lstResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProblemEmergencyDTO>> GetEmergencyCbo()
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                PageRequestModel req = new PageRequestModel();
                req.pageNumber = 1;
                req.pageSize = int.MaxValue;

                List<ProblemEmergencyDTO> result = new List<ProblemEmergencyDTO>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProblemEmergencyAPI/GetPaging";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = await client.PostAsync(uri, content).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemEmergencyDTO>>>(jsonResponse).Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProblemStatusDTO>> GetStatusCbo()
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                PageRequestModel req = new PageRequestModel();
                req.pageNumber = 1;
                req.pageSize = int.MaxValue;

                List<ProblemStatusDTO> result = new List<ProblemStatusDTO>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProblemStatusAPI/GetPaging";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = await client.PostAsync(uri, content).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemStatusDTO>>>(jsonResponse).Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProblemCriticalLevelDTO>> GetCriticalLevelCbo()
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                List<ProblemCriticalLevelDTO> lstPosition = new List<ProblemCriticalLevelDTO>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ProblemCriticalLevelAPI/GetAll", UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    lstPosition = JsonConvert.DeserializeObject<ResponseModel<List<ProblemCriticalLevelDTO>>>(jsonResponse).Data;
                }
                return lstPosition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProblemEventRequestDepDTO>> GetRequestDepCbo()
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                PageRequestModel req = new PageRequestModel();
                req.pageNumber = 1;
                req.pageSize = int.MaxValue;

                List<ProblemEventRequestDepDTO> result = new List<ProblemEventRequestDepDTO>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProblemEventRequestDepAPI/GetPaging";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = await client.PostAsync(uri, content).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemEventRequestDepDTO>>>(jsonResponse).Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProblemResidentAgencyGroupDTO>> GetResidentAgencyGroupCbo()
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                PageRequestModel req = new PageRequestModel();
                req.pageNumber = 1;
                req.pageSize = int.MaxValue;

                List<ProblemResidentAgencyGroupDTO> result = new List<ProblemResidentAgencyGroupDTO>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProblemResidentAgencyGroupAPI/GetPaging";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = await client.PostAsync(uri, content).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemResidentAgencyGroupDTO>>>(jsonResponse).Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProblemResidentAgencyDTO>> GetResidentAgencyCbo(int groupID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                List<ProblemResidentAgencyDTO> lstPosition = new List<ProblemResidentAgencyDTO>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/ProblemResidentAgencyAPI/GetByGroupID?GroupID=" + groupID, UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    lstPosition = JsonConvert.DeserializeObject<ResponseModel<List<ProblemResidentAgencyDTO>>>(jsonResponse).Data;
                }
                return lstPosition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseModel<object> InsertUpdate(ProblemEventDTO problemEvent, int userId, bool isTemplate = false)
        {
            try
            {
                if (problemEvent.ID > 0)
                {
                    problemEvent.UpdatedBy = userId;
                    problemEvent.UpdatedAt = DateTime.Now;
                }
                else
                {
                    problemEvent.CreatedBy = userId;
                    problemEvent.CreatedAt = DateTime.Now;
                }

                problemEvent.IsTemplate = isTemplate;

                // Không cần khi gọi api
                //problemEvent.UserReceiver = null;
                var problemEvent_DTO = new ProblemEventDTO(problemEvent);

                string base64String = Ultilities.GetTokenString();
                ResponseModel<object> objResponse = new ResponseModel<object>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = problemEvent_DTO, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemEventAPI/InsertUpdate";
                    var response = client.PostAsync(uri, content).Result;
                    //var jsonResponse =  response.Content.ReadAsStringAsync();
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    objResponse = JsonConvert.DeserializeObject<ResponseModel<object>>(jsonResponse);
                }
                return objResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public string Delete(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ProblemEventReqModel req = new ProblemEventReqModel();
                req.ID = ID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = string.Format("api/ProblemEventAPI/Delete");
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

        public string InsertProblemDepartment(int departmentID, int problemEventID, int userId)
        {
            try
            {
                ProblemDepartmentDTO obj = new ProblemDepartmentDTO();
                obj.DepartmentID = departmentID;
                obj.ProblemEventID = problemEventID;
                obj.CreatedAt = DateTime.Now;
                obj.CreatedBy = userId;

                string base64String = Ultilities.GetTokenString();
                ResponseModel<object> objResponse = new ResponseModel<object>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = obj, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemEventDepartmentAPI/Insert";
                    var response = client.PostAsync(uri, content).Result;
                    //var jsonResponse =  response.Content.ReadAsStringAsync();
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    objResponse = JsonConvert.DeserializeObject<ResponseModel<object>>(jsonResponse);
                }
                return objResponse.MessageCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public string InsertUpdateProblemDepartment(List<int> departmentIDs, int problemEventID, int userId)
        {
            try
            {
                ProblemDepartmentDTO obj = new ProblemDepartmentDTO();
                obj.DepartmentIDs = departmentIDs;
                obj.ProblemEventID = problemEventID;
                obj.CreatedAt = DateTime.Now;
                obj.CreatedBy = userId;

                string base64String = Ultilities.GetTokenString();
                ResponseModel<object> objResponse = new ResponseModel<object>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = obj, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemEventDepartmentAPI/InsertUpdate";
                    var response = client.PostAsync(uri, content).Result;
                    //var jsonResponse =  response.Content.ReadAsStringAsync();
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    objResponse = JsonConvert.DeserializeObject<ResponseModel<object>>(jsonResponse);
                }
                return objResponse.MessageCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<ProblemDepartmentDTO>> GetDepartmentByProblemEvent(int problemEventID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                List<ProblemDepartmentDTO> lstReturn = new List<ProblemDepartmentDTO>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/ProblemEventDepartmentAPI/GetByProblem?problemID={0}", problemEventID), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    lstReturn = JsonConvert.DeserializeObject<ResponseModel<List<ProblemDepartmentDTO>>>(jsonResponse).Data;
                }
                return lstReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Lấy người trực ca của phòng ban
        public async Task<string> GetShiftDepartment(int departmentID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                string result = "";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/ShiftHistoryAPI/GetByDepartmentID?DepartmentID={0}", departmentID), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<string>>(jsonResponse).Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProblemUserFix>> GetUserFix(int problemEventId)
        {
            try
            {
                List<ProblemUserFix> result = new List<ProblemUserFix>();
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
                        RequestUri = new Uri(string.Format("api/ProblemEventUserAPI/GetUserFix?problemEventID={0}", problemEventId), UriKind.Relative)
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemUserFix>>>(jsonResponse).Data;
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