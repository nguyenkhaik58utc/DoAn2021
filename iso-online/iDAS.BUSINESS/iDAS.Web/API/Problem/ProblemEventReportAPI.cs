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
    public class ProblemEventReportAPI
    {
        private string baseUrl = Ultilities.APIServer;

        public ResponseModel<List<ProblemEventReportDTO>> GetData(int problemEventID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                ProblemEventReportSearchModel req = new ProblemEventReportSearchModel();
                req.PageNumber = 1;
                req.PageSize = int.MaxValue;
                req.ProblemEventID = problemEventID;

                ResponseModel<List<ProblemEventReportDTO>> result = new ResponseModel<List<ProblemEventReportDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/ProblemEventReportAPI/GetPaging";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<ProblemEventReportDTO>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProblemEventReportInsModel> GetByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ProblemEventReportInsModel result = new ProblemEventReportInsModel();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/ProblemEventReportAPI/GetByID?id={0}", ID), UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<ProblemEventReportInsModel>>(jsonResponse);
                    result = respMode.Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string InsertUpdate(ProblemEventReportInsModel problemEventReport, int userId)
        {
            try
            {
                if (problemEventReport.ID > 0)
                {
                    problemEventReport.Reporter = userId;
                    problemEventReport.UpdatedBy = userId;
                    problemEventReport.UpdatedAt = DateTime.Now;
                }
                else
                {
                    problemEventReport.Reporter = userId;
                    problemEventReport.CreatedBy = userId;
                    problemEventReport.CreatedAt = DateTime.Now;
                }

                string base64String = Ultilities.GetTokenString();
                ResponseModel<object> objResponse = new ResponseModel<object>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = problemEventReport, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/ProblemEventReportAPI/InsertUpdate";
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

        public string Delete(int ID, int userID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                ResponseModel<object> objResponse = new ResponseModel<object>();
                DeleteRequestModel req = new DeleteRequestModel();
                req.ID = ID;
                req.UpdatedAt = DateTime.Now;
                req.UpdatedBy = userID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var uri = "api/ProblemEventReportAPI/Delete";
                    var response = client.PostAsync(uri, content).Result;
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
    }
}