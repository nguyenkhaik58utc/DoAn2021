using iDAS.DA;
using iDAS.Web.Areas.Document.Models;
using ISO.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Web.API
{
    public class DocumentAPI
    {
        private string baseUrl = Ultilities.APIServer;
        private DocumentDA documentDA = new DocumentDA();

        public async Task<List<DocumentFolder>> GetFolder(bool documentOut = false)
        {
            try
            {
                int typeOut = documentOut ? 1 : 0;
                string base64String = Ultilities.GetTokenString();
                List<DocumentFolder> lstResult = new List<DocumentFolder>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/DocumentFolderAPI/GetAll?DocumentOut={0}", typeOut), UriKind.Relative)
                    };
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    lstResult = JsonConvert.DeserializeObject<ResponseModel<List<DocumentFolder>>>(jsonResponse).Data;
                }
                return lstResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseModel<List<DocumentList>> GetData(int page, int pageSize, int folderID, int userID, int employeeID, bool type = false)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();

                DocumentReqModel req = new DocumentReqModel();
                req.pageIndex = page;
                req.pageSize = pageSize;
                req.folderID = folderID;
                req.type = type;
                req.userID = userID;
                req.employeeID = employeeID;

                ResponseModel<List<DocumentList>> result = new ResponseModel<List<DocumentList>>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    object param = new { data = req, signature = base64String };
                    var uri = "api/DocumentAPI/GetData";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);

                    var response = client.PostAsync(uri, content).Result;
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<ResponseModel<List<DocumentList>>>(jsonResponse);
                }

                var dbo = documentDA.Repository;
                for (int i=0; i < result.Data.Count; i++)
                {
                    int documentID = result.Data[i].ID;
                    result.Data[i].AttachmentFileIDs = dbo.DocumentAttachmentFiles.Where(t => t.DocumentID == documentID).Select(x => x.AttachmentFileID).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lưu quyên public
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SavePublicPermission(DocumentPublicSaveRequest entity)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    object param = new { data = entity, signature = base64String };

                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    var uri = "api/DocumentAPI/SavePublicPermission";
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

        public async Task<bool> CheckQuickDownloadLink(Guid fileID)
        {
            try
            {
                string base64String = Ultilities.GetTokenString();
                bool result = false;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Ultilities.ApplicationJson));
                    client.DefaultRequestHeaders.Add("signature", base64String);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Format("api/DocumentAPI/CheckQuickDownload?fileID={0}", fileID), UriKind.Relative),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var respMode = JsonConvert.DeserializeObject<ResponseModel<bool>>(jsonResponse);
                    result = respMode.Data;
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