using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;

namespace iDAS.Web.Areas.Generic.Controllers
{
    public class FileController : BaseController
    {
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
        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.Client, Duration = 120 * 60)]
        //public ActionResult LoadAvatarFile(int employeeId = 0)
        //{
        //    var bytedata = fileSV.GetAvatarFile(employeeId);
        //    return this.File(bytedata, "image/jpeg");
        //}
        //public ActionResult LoadLogoFile(string code)
        //{
        //    var bytedata = fileSV.GetLogoFile(code);
        //    return this.File(bytedata, "image/jpeg");
        //}
        public ActionResult ViewFile(string fileId)
        {
            var data = fileSV.ReadFile(fileId);
            return new Ext.Net.MVC.PartialViewResult
            {
                Model = data,
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

        public ActionResult BrowseImage()
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult UploadImg(ImageItem item)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var fileId = fileSV.Upload(item.BrowseImage).FirstOrDefault();
                    result = Url.Action("LoadImage", "File", new { area = "Generic", fileId = fileId });
                }
                else
                {
                    Ultilities.ShowMessageRequest(RequestStatus.ValidError);
                }
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct(result);
        }
    }
}
