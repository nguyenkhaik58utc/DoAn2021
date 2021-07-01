using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.API;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Generic.Controllers
{
    public class FileController : BaseController
    {
        private DocumentAPI api = new DocumentAPI();
        //
        // GET: /Generic/File/
        FileSV fileSV = new FileSV();
        /// <summary>
        /// Load ImageFile
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Client, Duration = 120 * 60, VaryByParam = "fileName")]
        public ActionResult LoadImage(Guid? fileId, string fileName)
        {
            if (fileId != null)
            {
                var file = fileSV.GetById((Guid)fileId);
                if (file != null)
                {
                    var byedata = file.Data;
                    if (byedata != null)
                    {
                        return this.File(byedata, "image/jpeg");
                    }
                }
            }
            return this.File("/Content/images/underfind.jpg", "image/jpeg");
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Client, Duration = 120 * 60)]
        public ActionResult LoadAvatarFile(int employeeId = 0)
        {
            var bytedata = fileSV.GetAvatarFile(employeeId);
            return this.File(bytedata, "image/jpeg");
        }
        public ActionResult LoadLogoFile(string code)
        {
            var bytedata = fileSV.GetLogoFile(code);
            return this.File(bytedata, "image/jpeg");
        }
        public ActionResult DownloadFile(string fileId)
        {
            var file = fileSV.GetFile(fileId);
            return this.File(file.Data,file.Type,file.Name);
        }
        // download nhanh tài liệu
        // f: fileId
        public ActionResult D(string f)
        {
            try
            {
                bool checkDownload = api.CheckQuickDownloadLink(new Guid(f)).Result;
                if (checkDownload == false)
                    return null;

                var file = fileSV.GetFile(f);
                return this.File(file.Data, file.Type, file.Name);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult ViewFile(string fileId)
        {
            var data = fileSV.ReadFile(fileId);
            return new Ext.Net.MVC.PartialViewResult
            {
                Model = data,
            };
        }
        public ActionResult ViewFilePdf(string file)
        {
            return new Ext.Net.MVC.PartialViewResult
            {
                Model = file,
            };
        }
        /// <summary>
        /// Load Url Image in base64;
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public ActionResult LoadImageUrl(Guid? fileId)
        {
            string result = "";
            if (fileId != null)
            {
                var byedata = fileSV.GetById((Guid)fileId).Data;
                var data = System.Convert.ToBase64String(byedata);
                if (byedata == null || System.Convert.ToBase64String(byedata) == "")
                {
                    result = "";
                }
                else
                {
                    result = string.Format("data:image;base64,{0}", data);
                }
            }
            return this.Json(result);
        }
        public ActionResult AttachmentFileView(string files, string key)
        {
            var result = new List<FileItem>();
            var lstFile = fileSV.GetByIds(iDAS.Utilities.Convert.ToListGuid(files));
            foreach (var file in lstFile)
            {
                file.TypeIcon = Ultility.GetIconFile(file.Extension);
                result.Add(file);
            }
            ViewData["Key"] = key;
            return new Ext.Net.MVC.PartialViewResult() { Model = result, ViewData = ViewData };
        }
        public ActionResult ViewListFile(string files)
        {
            var result = new List<FileItem>();
            var lstFile = fileSV.GetByIds(iDAS.Utilities.Convert.ToListGuid(files));
            foreach (var file in lstFile)
            {
                file.TypeIcon = Ultility.GetIconFile(file.Extension);
                result.Add(file);
            }
            return new Ext.Net.MVC.PartialViewResult() { Model = result, ViewData = ViewData };
        }
    }
}
