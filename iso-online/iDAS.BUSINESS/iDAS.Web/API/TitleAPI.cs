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
    public class TitleAPI
    {
        private string baseUrl = Ultilities.APIServer;
        public ResponseModel<List<TitleDTO>> GetData(int page, int pageSize)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                TitleReqModel req = new TitleReqModel();
                req.pageIndex = page;
                req.pageSize = pageSize;

                ResponseModel<List<TitleDTO>> result = new ResponseModel<List<TitleDTO>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/TitleAPI/GetData";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<TitleDTO>>>(jsonResponse);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TitleDTO> GetByID(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                TitleDTO result = new TitleDTO();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/TitleAPI/GetByID?ID={0}", ID), UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    //var jsonResponse = response.Content.ReadAsStringAsync();
                    //result = JsonConvert.DeserializeObject<Title>(jsonResponse);
                    //var jsonResponse = response.Result.Content.ReadAsStringAsync().Result;
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<TitleDTO>>(jsonResponse);
                    result = respMode.Data;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PositionCboDTO>> GetPositionCbo()
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                List<PositionCboDTO> lstPosition = new List<PositionCboDTO>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/TitleAPI/GetComboPosition", UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    lstPosition = JsonConvert.DeserializeObject<ResponseModel<List<PositionCboDTO>>>(jsonResponse).Data;
                }
                return lstPosition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(TitleDTO title, int userId)
        {
            try
            {
                title.UserID = userId;
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = title, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/TitleAPI/insert";
                    var response =  client.PostAsync(uri, content);
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

        public string Update(TitleDTO title)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = title, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/TitleAPI/update";
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

        public string Delete(int ID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                TitleReqModel req = new TitleReqModel();
                req.ID = ID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    object param = new { data = req, signature = base64String };
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = string.Format("api/TitleAPI/delete");
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

    }
}