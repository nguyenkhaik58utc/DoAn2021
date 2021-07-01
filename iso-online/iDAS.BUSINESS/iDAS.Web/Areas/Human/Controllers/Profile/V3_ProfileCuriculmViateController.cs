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
using iDAS.Web.API.Human.V3_Category;
using System.Threading.Tasks;
using iDAS.Web.API.Human;
using iDAS.Web.Areas.Human.Models;
using iDAS.Web.Areas.Human.Models.V3_Category;

namespace iDAS.Web.Areas.Human.Controllers.Profile
{

    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Hồ sơ lý lịch", IsActive = true, IsShow = false, Parent = "ProfileEmployee")]
    public class V3_ProfileCuriculmViateController : BaseController
    {
        private V3_NationalityAPI v3_NationalityAPI = new V3_NationalityAPI();
        private V3_CityAPI v3_CityAPI = new V3_CityAPI();
        private V3_DistrictAPI v3_DistrictAPI = new V3_DistrictAPI();
        private V3_CommuneAPI v3_CommuneAPI = new V3_CommuneAPI();
        private V3_EthnicAPI v3_EthnicAPI = new V3_EthnicAPI();
        private V3_ReligionAPI v3_ReligionAPI = new V3_ReligionAPI();
        private V3_YouthPositionAPI v3_YouthAPI = new V3_YouthPositionAPI();
        private V3_PartyPositionAPI v3_PartyPositionAPI = new V3_PartyPositionAPI();
        private V3_MilitaryPositionAPI v3_MilitaryPositionAPI = new V3_MilitaryPositionAPI();
        private V3_HumanProfileCuriculmViateAPI v3_HumanProfileViateAPI = new V3_HumanProfileCuriculmViateAPI();
        private HumanProfileCuriculmViateSV profile = new HumanProfileCuriculmViateSV();
        private V3_EducationTypeAPI v3_EducationTypeAPI = new V3_EducationTypeAPI();
        private V3_EducationResultAPI v3_EducationResultAPI = new V3_EducationResultAPI();
        private V3_EducationFieldAPI v3_EducationFieldAPI = new V3_EducationFieldAPI();
        private V3_EducationOrgAPI v3_EducationOrgAPI = new V3_EducationOrgAPI();
        private V3_EducationLevelAPI v3_EducationLevelAPI = new V3_EducationLevelAPI();
        private V3_CertificateLevelAPI v3_CertificateLevelAPI = new V3_CertificateLevelAPI();
        private V3_CertificateTypeAPI v3_CertificateTypeAPI = new V3_CertificateTypeAPI();
        private V3_AwardTypeAPI v3_AwardTypeAPI = new V3_AwardTypeAPI();
        private V3_GovermentManagementAPI v3_GovermentManagementAPI = new V3_GovermentManagementAPI();
        private V3_PoliticalTheoryAPI v3_PoliticalTheoryAPI = new V3_PoliticalTheoryAPI();

        [BaseSystem(Name = "Xem hồ sơ cá nhân")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ProfileForm(int id, int check)
        {
            var result = new HumanEmployeeItem { ID = id };

            ViewData["CheckPermission"] = check;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Profile", Model = result, ViewData = ViewData };
        }
        public ActionResult Index(int Id = 0, int check = 0)
        {
            ViewData["CheckPermission"] = check;
            var data = v3_HumanProfileViateAPI.GetByID(Id).Result;

            //danh sách tôn giáo
            ViewData["lstReligion"] = v3_ReligionAPI.GetAll().Result.Data;

            //danh sách dân tộc
            ViewData["lstEthnic"] = v3_EthnicAPI.GetAll().Result.Data;

            //danh sách quốc gia
            ViewData["lstNationality"] = v3_NationalityAPI.GetAll().Result.Data;

            //danh sách tỉnh thành phố
            ViewData["lstCity"] = v3_CityAPI.GetAll().Result.Data;

            //danh sách chức vụ quân đội
            ViewData["lstMilitaryPosition"] = v3_MilitaryPositionAPI.GetAll().Result.Data;

            //danh sách chức vụ đoàn
            ViewData["lstYouthPosition"] = v3_YouthAPI.GetAll().Result.Data;

            //danh sách chức vụ đảng
            ViewData["lstPartyPosition"] = v3_PartyPositionAPI.GetAll().Result.Data;
            //danh sách trình độ lý luận trính trị
            ViewData["lstPoliticalTheory"] = v3_PoliticalTheoryAPI.GetAll().Result.Data;
            //danh sách trình độ quản lý nhà nước
            ViewData["lstGovermentManagement"] = v3_GovermentManagementAPI.GetAll().Result.Data;

            // var data = profile.GetByEmployeeId(Id);
            if (data != null)
                return View(data);
            else return View(new V3_HumanProfileCuriculmViateResponse { HumanEmployeeID = Id });
            // else return View(new HumanProfileCuriculmViateItem { EmployeeID = Id });
        }
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update1(HumanProfileCuriculmViateItem updateData)
        {
            try
            {
                if (updateData.ID == 0)
                {
                    profile.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    profile.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            return this.Direct();
        }

        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(V3_HumanProfileCuriculmViateResponse updateData)
        {
            try
            {
                v3_HumanProfileViateAPI.SaveHumanProfileCuricilmViate(updateData);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);

            }
            return this.Direct();
        }

        //public async Task<ActionResult> GetNationalityCb()
        //{//ok
        //    var result = await v3_NationalityAPI.GetAll();
        //    return this.Store(result.Data);
        //}

        // dân tộc
        public async Task<ActionResult> GetEthnicCb()
        {
            var result = await v3_EthnicAPI.GetAll();
            return this.Store(result.Data);
        }

        // tôn giáo
        public async Task<ActionResult> GetReligionCb()
        {
            var result = await v3_ReligionAPI.GetAll();
            return this.Store(result.Data);
        }
        // hình thức đào tạo
        public async Task<ActionResult> GetEducationTypeCb()
        {
            var result = await v3_EducationTypeAPI.GetAll();
            return this.Store(result.Data);
        }
        // kết quả đào tạo
        public async Task<ActionResult> GetEducationResultCb()
        {
            var result = await v3_EducationResultAPI.GetAll();
            return this.Store(result.Data);
        }
        // chuyên ngành
        public async Task<ActionResult> GetEducationFieldCb()
        {
            var result = await v3_EducationFieldAPI.GetAll();
            return this.Store(result.Data);
        }
        // nơi đào tạo
        public async Task<ActionResult> GetEducationOrgCb()
        {
            var result = await v3_EducationOrgAPI.GetAll();
            return this.Store(result.Data);
        }
        // trình độ
        public async Task<ActionResult> GetEducationLevelCb()
        {
            var result = await v3_EducationLevelAPI.GetAll();
            return this.Store(result.Data);
        }
        // xếp loại
        public async Task<ActionResult> GetCertificateLevelCb()
        {
            var result = await v3_CertificateLevelAPI.GetAll();
            return this.Store(result.Data);
        }
        // loại chứng chỉ
        public async Task<ActionResult> GetCertificateTypeCb()
        {
            var result = await v3_CertificateTypeAPI.GetAll();
            return this.Store(result.Data);
        }
        // hình thức khen thưởng
        public async Task<ActionResult> GetAwardTypeCb()
        {
            var result = await v3_AwardTypeAPI.GetAll();
            return this.Store(result.Data);
        }

        //public async Task<ActionResult> GetCityCb()
        //{//ok
        //    var result = await v3_CityAPI.GetAll();
        //    return this.Store(result.Data);
        //}

        public async Task<ActionResult> GetDistrictCb(int CityID = 0)
        {//ok
            if (CityID == 0)
            {
                var result = await v3_DistrictAPI.GetAll();
                return this.Store(result.Data);
            }
            else
            {
                var result = await v3_DistrictAPI.GetByCity(CityID);
                return this.Store(result.Data);
            }
        }

        public async Task<ActionResult> GetCommuneCb(int DistrictID = 0)
        {
            if (DistrictID == 0)
            {
                var result = await v3_CommuneAPI.GetAll();
                return this.Store(result.Data);
            }
            else
            {
                var result = await v3_CommuneAPI.GetByDistrict(DistrictID);
                return this.Store(result.Data);
            }

        }

        public ActionResult FormPreviewProfileEmployee(int Id)
        {
            ViewData["Id"] = Id;
            return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
        }

        public ActionResult FormSummaryProfile(int Id)
        {
            var data = v3_HumanProfileViateAPI.GetFullByID(Id).Result;

            if (data != null)
            {
                //data.DepartmentId = DepartmentService.GetDepartmentsByEmployeeID(Id).FirstOrDefault().Id;
                //var nationality = v3_NationalityAPI.GetByID( data.NationalityID).Result.Name;
                //var ethnic = v3_EthnicAPI.GetByID(data.EthnicID).Result.Name;
                //var religion = v3_ReligionAPI.GetByID(data.ReligionID).Result.Name;
                //var positionGroup = v3_YouthAPI.GetByID(data.YouthPositionID).Result.Name;
                //var positionParty = v3_PartyPositionAPI.GetByID(data.PartyPosititonID).Result.Name;
                //var rankArmy = v3_NationalityAPI.GetByID(data.MilitaryPositionLevelID).Result.Name;

                //data.ID = data.ID;
                //data.NationalityName = nationality;
                //data.EthnicName = ethnic;
                //data.ReligionName = religion;
                //data.YouthPositionName = positionGroup;
                //data.PartyPosititonName = positionParty;
                //data.MilitaryPositionLevelName = rankArmy;
                //data.CityIDOfBirthName = "";
                //data.CityIDOfBirthName = ConvertAddress(data)[0];
                return View(data);
            }
            else return View(new V3_HumanProfileCuriculmViateResponse
            {
                HumanEmployeeID = Id,
                //DepartmentId = DepartmentService.GetDepartmentsByEmployeeID(Id).FirstOrDefault().Id
                //DepartmentId = DepartmentService.GetDepartmentsByEmployeeID(Id).FirstOrDefault().Id
            });
        }


        public ActionResult PrintCuriculmViate(Guid employeeId)
        {
            return new Ext.Net.MVC.PartialViewResult { };
        }

        //public async Task<ActionResult> GetYouthPositionCb()
        //{//ok
        //    var result = await v3_YouthAPI.GetAll();
        //    return this.Store(result.Data);
        //}

        //public async Task<ActionResult> GetPartyPositionCb()
        //{
        //    var result = await v3_PartPositionAPI.GetAll();
        //    return this.Store(result.Data);
        //}

        //public async Task<ActionResult> GetMilitaryPositionCb()
        //{//ok
        //    var result = await v3_MilitaryPositionAPI.GetAll();
        //    return this.Store(result.Data);
        //}

        //public ActionResult SetName(string id)
        //{
        //    var name = "";
        //    if (Guid.TryParse(id, out Guid eid))
        //    {
        //        var service = new EmployeeService();
        //        name = service.GetById(eid).Name;
        //    }
        //    return this.Direct(name);
        //}

        //public List<string> ConvertAddress(VPHumanProfileCuriculmViateBO data)
        //{
        //    var bornCityId = data.PlaceOfBirthCity;
        //    var bornDistrictId = data.PlaceOfBirthDistrict;
        //    var bornCommuneId = data.PlaceOfBirthCommune;
        //    var homeCityId = data.HomeTownCity;
        //    var homeDistrictId = data.HomeTownDistrict;
        //    var homeCommuneId = data.HomeTownCommune;
        //    var masterService = new ProfileMasterService();
        //    var bornCity = "";
        //    var bornDistrict = "";
        //    var bornCommune = "";
        //    var homeCity = "";
        //    var homeDistrict = "";
        //    var homeCommune = "";
        //    if (bornCityId != null && bornCityId != "" && Guid.TryParse(bornCityId, out Guid gid)) bornCity = masterService.GetSingleMasterData("city", bornCityId).Name;
        //    if (homeCityId != null && homeCityId != "" && Guid.TryParse(homeCityId, out Guid gid1)) homeCity = masterService.GetSingleMasterData("city", homeCityId).Name;
        //    if (bornDistrictId != null && bornDistrictId != "" && Guid.TryParse(bornDistrictId, out Guid gid2)) bornDistrict = masterService.GetSingleMasterData("district", bornDistrictId).Name;
        //    if (homeDistrictId != null && homeDistrictId != "" && Guid.TryParse(homeDistrictId, out Guid gid3)) homeDistrict = masterService.GetSingleMasterData("district", homeDistrictId).Name;
        //    if (bornCommuneId != null && bornCommuneId != "" && Guid.TryParse(bornCommuneId, out Guid gid4)) bornCommune = masterService.GetSingleMasterData("commune", bornCommuneId).Name;
        //    if (homeCommuneId != null && homeCommuneId != "" && Guid.TryParse(homeCommuneId, out Guid gid5)) homeCommune = masterService.GetSingleMasterData("commune", homeCommuneId).Name;

        //    var bornAddress = string.Join(" - ", new[] { bornCommune, bornDistrict, bornCity }.Where(s => !string.IsNullOrEmpty(s)));
        //    var homeAddress = string.Join(" - ", new[] { homeCommune, homeDistrict, homeCity }.Where(s => !string.IsNullOrEmpty(s)));
        //    return new List<string> { bornAddress, homeAddress };
        //}

        #region Export Profile Employee
        //public ActionResult InformationProfile(Guid employeeId)
        //{
        //    var data = VPHumanProfileCuriculmViateService.GetByEmployeeId(employeeId);
        //    var experience = VPHumanProfileWorkExperienceService.GetByEmployeeIdNotPaging(employeeId).ToList();
        //    ViewBag.experience = experience;
        //    return PartialView(data);
        //}

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Export(string data)
        {
            //using (var stream = new io.MemoryStream())
            //{
            //    var sr = new io.StringReader(Encoding.UTF8.GetBytes(data).ToString());
            //    //var htmlStream = new io.MemoryStream(Encoding.UTF8.GetBytes(data));
            //    var pdfDoc = new iTextSharp.text.Document();
            //    var writer = PdfWriter.GetInstance(pdfDoc, stream);
            //    var htmlWorker = new HTMLWorker(pdfDoc);
            //    pdfDoc.Open();

            //    BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIAL.TTF", BaseFont.IDENTITY_H, true);
            //    Font NormalFont = new Font(bf, 12, Font.NORMAL, BaseColor.BLACK);
            //    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);

            //    htmlWorker.StartDocument();
            //    htmlWorker.Parse(sr);
            //    htmlWorker.EndDocument();
            //    htmlWorker.Close();
            //    pdfDoc.Close();
            //    var fileName = AppDomain.CurrentDomain.BaseDirectory + "1.pdf";
            //    io.File.WriteAllBytes(fileName, stream.ToArray());
            //    //File(stream.ToArray(), "application/pdf", "1.pdf");
            //    ////stream.write

            //    //var file = new FileBO()
            //    //    {
            //    //        Name = DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf",
            //    //        Stream = stream
            //    //    };
            //    //FileUploadService.Upload(file);
            //}
            ////var fileUpload = new FileUploadBO()
            ////{
            ////     FileAttachments = new List<HttpPostedFileBase>()
            ////     {
            ////         new HttpPostedFileBase()
            ////     }
            ////};
            ////var file = new HttpPostedFile
            ////FileService.Upload()

            return this.Direct();
        }
        #endregion
    }
}
