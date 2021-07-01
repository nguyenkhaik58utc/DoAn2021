using iDAS.Web.Areas.Human.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace iDAS.Web.API.Human
{
    public class HumanDashBoardAPI
    {
        string base64String = Ultilities.GetTokenString();
        [HttpPost]
        public async Task<List<ChartDTO>> GetAmountEmployeeByMothOfYear(int year)
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ChartDTO> chartDTO = new List<ChartDTO>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanDashboardAPI/GetAmountEmployeeByMothOfYear?year=" + year, UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    chartDTO = JsonConvert.DeserializeObject<List<ChartDTO>>(data.data.ToString());
                    return chartDTO;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<List<ChartDTO>> GetStatisticalByEducationLevel()
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ChartDTO> chartDTO = new List<ChartDTO>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanDashboardAPI/GetStatisticalByEducationLevel", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    chartDTO = JsonConvert.DeserializeObject<List<ChartDTO>>(data.data.ToString());
                    return chartDTO;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<List<ChartDTO>> GetStatisticalByAge()
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ChartDTO> chartDTO = new List<ChartDTO>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanDashboardAPI/GetStatisticalByAge", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    chartDTO = JsonConvert.DeserializeObject<List<ChartDTO>>(data.data.ToString());
                    return chartDTO;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<List<ChartDTO>> GetStatisticalByGender()
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ChartDTO> chartDTO = new List<ChartDTO>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanDashboardAPI/GetStatisticalByGender", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    chartDTO = JsonConvert.DeserializeObject<List<ChartDTO>>(data.data.ToString());
                    return chartDTO;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<List<ChartDTO>> GetStatisticalByContractType()
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<ChartDTO> chartDTO = new List<ChartDTO>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanDashboardAPI/GetStatisticalByContractType", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    chartDTO = JsonConvert.DeserializeObject<List<ChartDTO>>(data.data.ToString());
                    return chartDTO;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<List<RewardDTO>> GetDataAward()
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<RewardDTO> rewardDTO = new List<RewardDTO>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanDashboardAPI/GetStatisticalByReward", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    rewardDTO = JsonConvert.DeserializeObject<List<RewardDTO>>(data.data.ToString());
                    return rewardDTO;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<List<NewEmployeeDTO>> GetDataNewEmployee()
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<NewEmployeeDTO> newEmployeeDTO = new List<NewEmployeeDTO>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanDashboardAPI/GetStatisticalByNewEmployee", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    newEmployeeDTO = JsonConvert.DeserializeObject<List<NewEmployeeDTO>>(data.data.ToString());
                    return newEmployeeDTO;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpPost]
        public async Task<List<AlmostExpireEmployeeDTO>> GetDataExpireDate()
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<AlmostExpireEmployeeDTO> ExpireDateDTO = new List<AlmostExpireEmployeeDTO>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanDashboardAPI/GetStatisticalByExpireDate", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    ExpireDateDTO = JsonConvert.DeserializeObject<List<AlmostExpireEmployeeDTO>>(data.data.ToString());
                    return ExpireDateDTO;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<List<RecruitmentDTO>> GetAllCruitment()
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<RecruitmentDTO> recruitmentDTOs = new List<RecruitmentDTO>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanDashboardAPI/GetStatisticalByCruitment", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    recruitmentDTOs = JsonConvert.DeserializeObject<List<RecruitmentDTO>>(data.data.ToString());
                    return recruitmentDTOs;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<List<TrainingDTO>> GetAllTraining()
        {
            try
            {
                string Baseurl = Ultilities.APIServer;
                List<TrainingDTO> trainingDTOs = new List<TrainingDTO>();
                var tokenString = Ultilities.GenerateJSONWebToken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("api/HumanDashboardAPI/GetStatisticalByTraining", UriKind.Relative)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(jsonResponse);
                    trainingDTOs = JsonConvert.DeserializeObject<List<TrainingDTO>>(data.data.ToString());
                    return trainingDTOs;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
