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
    public class ProblemStatisticAPI
    {
        private string baseUrl = Ultilities.APIServer;

        // Thống kê theo phòng ban - Thống kê theo loại
        public ResponseModel<List<ProblemTypeResModel>> GetStatisticByType(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemTypeResModel>> result = new ResponseModel<List<ProblemTypeResModel>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemTypeReport";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemTypeResModel>>>(jsonResponse);
                }
                if(result == null)
                    result = new ResponseModel<List<ProblemTypeResModel>>();
                if (result.Data == null)
                {
                    result.Data = new List<ProblemTypeResModel>();
                    result.TotalRows = 0;
                }    

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Thống kê theo đơn vị yêu cầu - Thống kê theo loại
        public ResponseModel<List<ProblemTypeEventRequestDepResModel>> GetStatisticByTypeEventRequest(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemTypeEventRequestDepResModel>> result = new ResponseModel<List<ProblemTypeEventRequestDepResModel>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemTypeReportEventRequest";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemTypeEventRequestDepResModel>>>(jsonResponse);
                }
                if (result == null)
                    result = new ResponseModel<List<ProblemTypeEventRequestDepResModel>>();
                if (result.Data == null)
                {
                    result.Data = new List<ProblemTypeEventRequestDepResModel>();
                    result.TotalRows = 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Thống kê theo đơn vị yêu cầu - Thống kê theo độ nghiêm trọng
        public ResponseModel<List<ProblemCriticalEventRequestDepResModel>> GetStatisticByCriticalLevelEventRequest(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemCriticalEventRequestDepResModel>> result = new ResponseModel<List<ProblemCriticalEventRequestDepResModel>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/CriticalTypeReportEventRequest";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemCriticalEventRequestDepResModel>>>(jsonResponse);
                }
                if (result == null)
                    result = new ResponseModel<List<ProblemCriticalEventRequestDepResModel>>();
                if (result.Data == null)
                {
                    result.Data = new List<ProblemCriticalEventRequestDepResModel>();
                    result.TotalRows = 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Thống kê theo cơ quan thường trú - Thống kê theo loại
        public ResponseModel<List<ProblemTypeEventResidentAgencyResModel>> GetStatisticByTypeResidentAgency(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemTypeEventResidentAgencyResModel>> result = new ResponseModel<List<ProblemTypeEventResidentAgencyResModel>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemTypeReportResidentAgency";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemTypeEventResidentAgencyResModel>>>(jsonResponse);
                }
                if (result == null)
                    result = new ResponseModel<List<ProblemTypeEventResidentAgencyResModel>>();
                if (result.Data == null)
                {
                    result.Data = new List<ProblemTypeEventResidentAgencyResModel>();
                    result.TotalRows = 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Thống kê cơ quan thường trú - Thống kê theo phòng ban
        public ResponseModel<List<DepartmentEventResidentAgencyResModel>> GetStatisticByDepartmentResidentAgency(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<DepartmentEventResidentAgencyResModel>> result = new ResponseModel<List<DepartmentEventResidentAgencyResModel>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/DepartmentReportResidentAgency";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<DepartmentEventResidentAgencyResModel>>>(jsonResponse);
                }
                if (result == null)
                    result = new ResponseModel<List<DepartmentEventResidentAgencyResModel>>();
                if (result.Data == null)
                {
                    result.Data = new List<DepartmentEventResidentAgencyResModel>();
                    result.TotalRows = 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // Thống kê theo phòng ban - Thống kê theo độ nghiêm trọng
        public ResponseModel<List<CriticalTypeResModel>> GetStatisticByCriticalLevel(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<CriticalTypeResModel>> result = new ResponseModel<List<CriticalTypeResModel>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/CriticalTypeReport";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<CriticalTypeResModel>>>(jsonResponse);
                }
                if (result == null)
                    result = new ResponseModel<List<CriticalTypeResModel>>();
                if (result.Data == null)
                {
                    result.Data = new List<CriticalTypeResModel>();
                    result.TotalRows = 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Thống kê theo phòng ban - Thống kê theo loại - Chi tiết
        public ResponseModel<List<ProblemEventListDTO>> GetStatisticByTypeDetail(int page, int pageSize, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int type = 0, int departmentID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;
                req.ProblemTypeID = type;
                req.DepartmentID = departmentID;
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemEventListDTO>> result = new ResponseModel<List<ProblemEventListDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemTypeReportDetail";
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

        //Thống kê theo phòng ban - Thống kê theo độ nghiêm trọng - Chi tiết
        public ResponseModel<List<ProblemEventListDTO>> GetStatisticByCriticalDetail(int page, int pageSize, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int criticalLevelID = 0, int departmentID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;
                req.CriticalID = criticalLevelID;
                req.DepartmentID = departmentID;
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemEventListDTO>> result = new ResponseModel<List<ProblemEventListDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/CriticalTypeReportDetail";
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

        // Thống kê theo đơn vị - Thống kê theo loại - Chi tiết
        public ResponseModel<List<ProblemEventListDTO>> GetStatisticByTypeEventRequestDetail(int page, int pageSize, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int type = 0, int EventRequestDepID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticEventRequestReqModel req = new ProblemStatisticEventRequestReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;
                req.ProblemTypeID = type;
                req.EventRequestDepID = EventRequestDepID;
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemEventListDTO>> result = new ResponseModel<List<ProblemEventListDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemTypeReportEventRequestDepDetail";
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
        // Thống kê theo cơ quan thường trú - Thống kê theo loại - Chi tiết
        public ResponseModel<List<ProblemEventListDTO>> GetStatisticByTypeResidentAgencyDetail(int page, int pageSize, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int type = 0, int ResidentAgencyID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticEventRequestReqModel req = new ProblemStatisticEventRequestReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;
                req.ProblemTypeID = type;
                req.ResidentAgencyID = ResidentAgencyID;
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemEventListDTO>> result = new ResponseModel<List<ProblemEventListDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemTypeReportEventResidentAgencyDetail";
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

        //Thống kê theo cơ quan thường trú - Thống kê theo phòng ban - Chi tiết
        public ResponseModel<List<ProblemEventListDTO>> GetStatisticByDepartmentResidentAgencyDetail(int page, int pageSize, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int DepartmentID = 0, int ResidentAgencyID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticEventResidentAgencyModel req = new ProblemStatisticEventResidentAgencyModel();
                req.PageNumber = page;
                req.PageSize = pageSize;
                req.DepartmentID = DepartmentID;
                req.ResidentAgencyID = ResidentAgencyID;
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemEventListDTO>> result = new ResponseModel<List<ProblemEventListDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/DepartmentReportResidentAgencyDetail";
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

        //Thống kê theo đơn vị - Thống kê theo độ nghiêm trọng - Chi tiết
        public ResponseModel<List<ProblemEventListDTO>> GetStatisticByCriticalEventRequestDetail(int page, int pageSize, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int criticalLevelID = 0, int EventRequestDepID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticEventRequestReqModel req = new ProblemStatisticEventRequestReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;
                req.CriticalID = criticalLevelID;
                req.EventRequestDepID = EventRequestDepID;
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemEventListDTO>> result = new ResponseModel<List<ProblemEventListDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/CriticalTypeReportEventRequestDepDetail";
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


        // Thống kê theo cá nhân - Thống kê theo loại - Chi tiết
        public ResponseModel<List<ProblemEventListDTO>> GetStatisticForUserDetail(int page, int pageSize, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int type = 0, int userID = 0, int criticalLevelID = 0, int departmentID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;
                req.ProblemTypeID = type;
                req.CriticalID = criticalLevelID;
                req.UserID = userID;
                req.From = fromDate;
                req.To = toDate;
                req.DepartmentID = departmentID;

                ResponseModel<List<ProblemEventListDTO>> result = new ResponseModel<List<ProblemEventListDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemEvent_ReportByDepartmentDetail";
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

        public async Task<List<HumanEmployeeDTO>> GetUserByDepartment(int departmentID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                List<HumanEmployeeDTO> lstDataReturn = new List<HumanEmployeeDTO>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/HumanEmployeeAPI/GetByDepartment?departmentID={0}", departmentID), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    lstDataReturn = JsonConvert.DeserializeObject<ResponseModel<List<HumanEmployeeDTO>>>(jsonResponse).Data;
                }
                return lstDataReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Thống kê theo cá nhân - Thống kê khi chọn phòng ban
        public ResponseModel<ProblemEventReport_ByDepartment> GetStatisticUser(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int departmentID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.From = fromDate;
                req.To = toDate;
                req.DepartmentID = departmentID;

                ResponseModel<ProblemEventReport_ByDepartment> result = new ResponseModel<ProblemEventReport_ByDepartment>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemEvent_ReportByDepartment";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<ProblemEventReport_ByDepartment>>(jsonResponse);
                }
                if (result == null)
                    result = new ResponseModel<ProblemEventReport_ByDepartment>();

                if (result.Data == null)
                {
                    result.Data = new ProblemEventReport_ByDepartment();
                    result.TotalRows = 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Thống kê theo cá nhân - Thống kê khi chọn user
        public ResponseModel<List<ProblemEventReport_GetByUserDTO>>  GetStatisticOneUser(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int userID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemEventReport_GetByUserReqModel req = new ProblemEventReport_GetByUserReqModel();
                req.From = fromDate.Value;
                req.To = toDate.Value;
                req.UserID = userID;

                ResponseModel<List<ProblemEventReport_GetByUserDTO>> result = new ResponseModel<List<ProblemEventReport_GetByUserDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemEvent_ReportByUser";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemEventReport_GetByUserDTO>>>(jsonResponse);
                }
                if (result == null)
                    result = new ResponseModel<List<ProblemEventReport_GetByUserDTO>>();

                if (result.Data == null)
                {
                    result.Data = new List<ProblemEventReport_GetByUserDTO>();
                    result.TotalRows = 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //ResponseModel<List<ProblemEventDTO>>
        public ResponseModel<List<ProblemEventListDTO>> GetStatisticOneUserDetail(int page, int pageSize, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int userID = 0, int criticalID = 0, int problemTypeID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemEventReport_GetByUserDetailReqModel req = new ProblemEventReport_GetByUserDetailReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;
                req.CriticalID = criticalID;
                req.UserID = userID;
                req.ProblemTypeID = problemTypeID;
                req.From = fromDate.Value;
                req.To = toDate.Value;

                ResponseModel<List<ProblemEventListDTO>> result = new ResponseModel<List<ProblemEventListDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemEvent_ReportByUserDetail";
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

        // Thống kê tiếp nhận
        public ResponseModel<List<ProblemOnDutyResModel>> GetStatisticByOnDuty(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemOnDutyResModel>> result = new ResponseModel<List<ProblemOnDutyResModel>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemOnDutyReport";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemOnDutyResModel>>>(jsonResponse);
                }
                if (result == null)
                    result = new ResponseModel<List<ProblemOnDutyResModel>>();
                if (result.Data == null)
                {
                    result.Data = new List<ProblemOnDutyResModel>();
                    result.TotalRows = 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Thống kê tiếp nhận sự cố
        public ResponseModel<List<ProblemEventListDTO>> GetStatisticByOnDutyDetail(int page, int pageSize, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, bool onDuty = true, int departmentID = 0)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemStatisticReqModel req = new ProblemStatisticReqModel();
                req.PageNumber = page;
                req.PageSize = pageSize;
                req.OnDuty = onDuty;
                req.DepartmentID = departmentID;
                req.From = fromDate;
                req.To = toDate;

                ResponseModel<List<ProblemEventListDTO>> result = new ResponseModel<List<ProblemEventListDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/StatisticsAPI/ProblemOnDutyReportDetail";
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

    }
}