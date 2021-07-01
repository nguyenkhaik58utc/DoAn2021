using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using System.IO;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize=false)]
    [BaseSystem(Name = "Hồ sơ ứng viên", Icon = "User", IsActive = true, IsShow = true, Position = 1, Parent = GroupRecruitmentProfileController.Code)]
    public class RecruitmentProfileController : BaseController
    {
        private HumanRecruitmentProfileSV RecruitmentProfileService = new HumanRecruitmentProfileSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadProfile(StoreRequestParameters parameters)
        {
            int totalCount;
            var result = RecruitmentProfileService.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(result, totalCount);
        }

        #region Cập nhật hồ sơ ứng viên
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int ID = 0)
        {
            var data = new HumanRecruitmentProfileItem();
            if (ID != 0)
            {
                data = RecruitmentProfileService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };

        }

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="recruitmentProfile"></param>
        /// <returns></returns>
        public ActionResult Update(HumanRecruitmentProfileItem recruitmentProfile)
        {
            string pathServer = PathUpload.Root + User.Code + PathUpload.HumanRecruitmentAvatar;
            if (recruitmentProfile.ID == 0)
            {
                try
                {
                    if (this.ModelState.IsValid)
                    {
                        var fileUploadField = X.GetCmp<FileUploadField>("FileUploadFieldId");
                        string path = pathServer;
                        if (fileUploadField.PostedFile != null && fileUploadField.PostedFile.ContentLength > 0)
                            upload(ref path, fileUploadField, FormatFile.Image);
                        if (path != pathServer)
                        {
                            recruitmentProfile.Avatar = path;
                        }

                        RecruitmentProfileService.Insert(recruitmentProfile, User.ID);
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    }
                }
                catch
                {
                    Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                }
                finally
                {

                    X.GetCmp<Window>("winRecruitmentProfile").Close();
                    X.GetCmp<Store>("StoreProfile").Reload();
                }
            }
            else
            {
                try
                {
                    var fileUploadField = X.GetCmp<FileUploadField>("FileUploadFieldId");
                    string path = pathServer;
                    if (fileUploadField.PostedFile != null && fileUploadField.PostedFile.ContentLength > 0)
                        upload(ref path, fileUploadField, FormatFile.Image);
                    if (path != pathServer)
                    {

                        deleteFileAvarta(recruitmentProfile.ID);
                        recruitmentProfile.Avatar = path;
                    }
                    RecruitmentProfileService.Update(recruitmentProfile, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);

                }
                catch
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                }
                finally
                {

                    X.GetCmp<Window>("winRecruitmentProfile").Close();
                    X.GetCmp<Store>("StoreProfile").Reload();
                }
            }
            return this.Direct();
        }
        /// <summary>
        /// Xóa ảnh đại diện
        /// </summary>
        /// <param name="id"></param>
        private void deleteFileAvarta(int id)
        {
            var logo = RecruitmentProfileService.GetAvatarById(id);
            if (!string.IsNullOrEmpty(logo))
            {
                System.IO.File.Delete(Server.MapPath(logo));
            }
        }
        /// <summary>
        /// Upload Ảnh đại diện
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileUploadField"></param>
        /// <param name="format"></param>
        /// <param name="newFileName"></param>
        private void upload(ref string path, FileUploadField fileUploadField, FormatFile format, string newFileName = "")
        {
            //case file upload is correct
            if (fileUploadField.HasFile)
            {
                //get file name
                string fileName = fileUploadField.PostedFile.FileName;

                //get file size (KB)
                decimal fileSize = System.Convert.ToDecimal(fileUploadField.PostedFile.ContentLength) / 1024;

                //case format of file is incorrect
                if (!Ultility.CheckFormatFile(fileName, format))
                {
                    Ultility.ShowMessageBox(message: RequestMessage.NotFileImage, icon: MessageBox.Icon.ERROR);
                    return;
                }

                //check path to save file not or exist
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(Server.MapPath(path));
                }

                //get new file name
                newFileName = string.IsNullOrEmpty(newFileName) ? fileName : newFileName + "_" + DateTime.Now.ToFileTime().ToString() + '.' + fileName.Split('.').Last().ToLower();

                //get full path for file upload
                path = path + newFileName;

                //upload file
                fileUploadField.PostedFile.SaveAs(Server.MapPath(path));
            }

            //case file upload is incorrect
            else
            {
                Ultility.ShowMessageBox(message: RequestMessage.NotFileUpload, icon: MessageBox.Icon.ERROR);
            }
        }
        #endregion

        #region Xóa
        /// <summary>
        /// Thực hiện xóa hồ sơ
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteProfile(int ID)
        {
            try
            {

                RecruitmentProfileService.Delete(ID);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                X.GetCmp<Store>("StoreProfile").Reload();
            }
            catch
            {
                X.Msg.Confirm("Thông báo", "Có ràng buộc dữ liệu, bạn có muốn tiếp tục xóa không?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "reDelete(" + ID + ")",
                        Text = "Tiếp tục",

                    },
                    No = new MessageBoxButtonConfig
                    {
                        Text = "Hủy"
                    }
                }).Show();
            }
            return this.Direct();

        }
        /// <summary>
        /// Xóa trạng thái bản ghi
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult reDeleteProfile(int id)
        {
            try
            {
                RecruitmentProfileService.IsDelete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreProfile").Reload();
            }
            return this.Direct();
        }

        #endregion

        #region Chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int ID)
        {
            var data = RecruitmentProfileService.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion
    }
}