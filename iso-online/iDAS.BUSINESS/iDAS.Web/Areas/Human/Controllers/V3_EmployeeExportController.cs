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
using iDAS.Web.Areas.Human.Model;
using System.Text;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Xuất file danh sách nhân sự", IsActive = true, IsShow = false, Parent = "Human")]
    public class V3_EmployeeExportController : BaseController
    {

        private V3_HumanProfileDiplomaAPI humanProfileDiplomaAPI = new V3_HumanProfileDiplomaAPI();
        private V3_HumanProfileRewardAPI humanProfileRewardAPI = new V3_HumanProfileRewardAPI();
        private V3_HumanProfileCuriculmViateAPI humanProfileCuriculmViateAPI = new V3_HumanProfileCuriculmViateAPI();
        private V3_HumanProfileCertificateAPI humanProfileCertificateAPI = new V3_HumanProfileCertificateAPI();
        private V3_HumanProfileTrainingAPI humanProfileTrainingAPI = new V3_HumanProfileTrainingAPI();
        private V3_HumanProfileWorkExperienceAPI humanProfileWorkExperienceAPI = new V3_HumanProfileWorkExperienceAPI();
        private V3_HumanEmployeeAPI v3_HumanEmployeeAPI = new V3_HumanEmployeeAPI();

        // GET: Human/V3_EmployeeExport
        public ActionResult Index()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "FormProfileFileExcel" };
        }

        /// <summary>
        /// 1. hien thi form check thong tin can export
        /// </summary>
        /// <returns></returns>
        public ActionResult FormProfileFieldExcel(string Name = null, string BirthDayFrom = null, string BirthDayTo = null, string Native = "", int Religion = 0, int Ethnic = 0,
                                    string PlaceOfWork = "", string Position = "", string DepartmentName = "",
                                    string EduName = "", int EducationType = 0, int EducationResult = 0,
                                    string DiplomaName = "", int Major = 0, int DiplomaEducationType = 0, int DiplomaEducationOrg = 0, int DiplomaEducationLevel = 0, int DiplomaCertificateLevel = 0,
                                    string NameOfCertificate = "", int CertificateLevel1 = 0, int CertificateType = 0,
                                    string Number = "", string reason = "", string Reward = "")
        {
            ViewData["Name"] = Name;
            ViewData["BirthDayFrom"] = BirthDayFrom;
            ViewData["BirthDayTo"] = BirthDayTo;
            ViewData["Native"] = Native;
            ViewData["Religion"] = Religion;
            ViewData["Ethnic"] = Ethnic;
            ViewData["PlaceOfWork"] = PlaceOfWork;
            ViewData["Position"] = Position;
            ViewData["DepartmentName"] = DepartmentName;
            ViewData["EduName"] = EduName;
            ViewData["EducationType"] = EducationType;
            ViewData["EducationResult"] = EducationResult;
            ViewData["DiplomaName"] = DiplomaName;
            ViewData["Major"] = Major;
            ViewData["DiplomaEducationType"] = DiplomaEducationType;
            ViewData["DiplomaEducationOrg"] = DiplomaEducationOrg;
            ViewData["DiplomaEducationLevel"] = DiplomaEducationLevel;
            ViewData["DiplomaCertificateLevel"] = DiplomaCertificateLevel;
            ViewData["NameOfCertificate"] = NameOfCertificate;
            ViewData["CertificateLevel1"] = CertificateLevel1;
            ViewData["CertificateType"] = CertificateType;
            ViewData["Number"] = Number;
            ViewData["reason"] = reason;
            ViewData["Reward"] = Reward;
            // Thông tin cơ bản
            var cv = new string[] { "Code","Mã", "Name","Tên", "Sex", "Giới tính", "DateOfBirth","Ngày sinh", "BornAddress", "Nơi sinh", "HomeAddress", "Quê quán",
                "People", "Dân tộc", "PermanentResidence", "Hộ khẩu", "CurrentResidence", "Địa chỉ hiện tại", "OfficePhone","Điện thoại", "NumberOfIdentityCard","Số CMT",
                "DateIssueOfIdentityCard","Ngày cấp CMT", "PlaceIssueOfIdentityCard","Nơi cấp CMT", "TaxCode","Mã số thuế", "NumberOfBankAccounts","Số tài khoản",
                "Bank","Ngân hàng", "NumberOfPassport","Số hộ chiếu", "PlaceOfPassport","Nơi cấp hộ chiếu", "DateOfIssuePassport","Ngày cấp hộ chiếu",
                "PassportExpirationDate","Ngày hết hạn hộ chiếu", "Religion","Tôn giáo"};

            // Chính trị
            var politic = new string[] { "DateOnGroup","Ngày vào đoàn", "PositionGroup","Chức vụ đoàn", "PlaceOfLoadedGroup","Nơi kết nạp đoàn",
                "DateJoinRevolution", "Ngày tham gia cách mạng", "DateOfJoinParty", "Ngày vào đảng chính thức", "PlaceOfLoadedParty", "Nơi kết nạp đảng",
                "PosititonParty", "Chức vụ đảng" , "NumberOfPartyCard", "Số thẻ đảng" , "DateOnArmy", "Ngày nhập ngũ", "PositionArmy", "Chức vụ quân đội",
                "ArmyRank", "Cập bậc quân đội", "PoliticalTheory", "Trình độ lý luận chính trị", "GovermentManagement", "Trình độ quản lý nhà nước"};

            // quá trình công tác
            var workExp = new string[] { "StartDate","Bắt đầu", "EndDate","Kết thúc", "PlaceOfWork","Nơi công tác",
                "Position", "Chức vụ đảm nhiệm", "Department", "Phòng ban", "JobDescription", "Công việc"};

            // quá trình đào tạo
            var training = new string[] { "Name","Tên khóa học", "Form","Hình thức đào tạo", "Content","Nội dung đào tạo",
                "Certificate", "Chứng chỉ đào tạo", "Result", "Kết quả đào tạo"};

            // văn bằng
            var diploma = new string[] { "Name", "Tên văn bằng", "Faculty", "Khoa", "Major", "Chuyên ngành", "Place", "Nơi đào tạo", "Level", "Trình độ" };
            // chứng chỉ
            var certificate = new string[] { "Name", "Tên chứng chỉ", "Level", "Xếp loại", "CertificateType", "Loại chứng chỉ", "PlaceOfTraining", "Nơi cấp",
                "DateIssuance", "Ngày cấp", "DateExpiration", "Ngày hết hạn"};
            // khen thưởng
            var reward = new string[] { "NumberOfDecision", "Số quyết định", "DateOfDecision", "Ngày quyết định", "Reason", "Lý do", "Form", "Hình thức khen thưởng" };

            var result = new GroupCheckBoxBO();
            // Tạo danh sách checkbox
            // hồ sơ lý lịch
            for (int i = 0; i < cv.Length - 1; i += 2)
                result.groupCV.Add(new CheckBoxBO(cv[i], false, cv[i + 1]));

            // chính trị
            for (int i = 0; i < politic.Length - 1; i += 2)
                result.groupPolitic.Add(new CheckBoxBO(politic[i], false, politic[i + 1]));

            // quá trình công tác
            for (int i = 0; i < workExp.Length - 1; i += 2)
                result.groupWorkExp.Add(new CheckBoxBO(workExp[i], false, workExp[i + 1]));

            // Quá trình đào tạo
            for (int i = 0; i < training.Length - 1; i += 2)
                result.groupTraining.Add(new CheckBoxBO(training[i], false, training[i + 1]));

            // Văn bằng
            for (int i = 0; i < diploma.Length - 1; i += 2)
                result.groupDiploma.Add(new CheckBoxBO(diploma[i], false, diploma[i + 1]));

            // Chứng chỉ
            for (int i = 0; i < certificate.Length - 1; i += 2)
                result.groupCertificate.Add(new CheckBoxBO(certificate[i], false, certificate[i + 1]));

            // Khen thưởng
            for (int i = 0; i < reward.Length - 1; i += 2)
                result.groupReward.Add(new CheckBoxBO(reward[i], false, reward[i + 1]));

            return new Ext.Net.MVC.PartialViewResult { ViewName = "FormProfileFileExcel", Model = result, ViewData = ViewData };
            //return new Ext.Net.MVC.PartialViewResult { ViewName = "FormProfileFieldExcel"};
        }

        /// <summary>
        /// 2. xuat file excel
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult ExportExcelProfile(GroupCheckBoxBO data = null, string Name = null, string BirthDayFrom = null, string BirthDayTo = null, string Native = "", int Religion = 0, int Ethnic = 0,
                                    string PlaceOfWork = "", string Position = "", string DepartmentName = "",
                                    string EduName = "", int EducationType = 0, int EducationResult = 0,
                                    string DiplomaName = "", int Major = 0, int DiplomaEducationType = 0, int DiplomaEducationOrg = 0, int DiplomaEducationLevel = 0, int DiplomaCertificateLevel = 0,
                                    string NameOfCertificate = "", int CertificateLevel1 = 0, int CertificateType = 0,
                                    string Number = "", string reason = "", int Reward = 0)
        {
            HumanEmployeeSearchRequest req = new HumanEmployeeSearchRequest();
            req.pageNumber = 1;
            req.pageSize = Int32.MaxValue;
            req.name = Name;
            if (BirthDayFrom.Equals("null"))
            {
                DateTime? From = null;
                req.birthDayFrom = From;
            }
            else
                req.birthDayFrom = DateTime.Parse(BirthDayFrom);
            if (BirthDayTo.Equals("null"))
            {
                DateTime? To = null;
                req.birthDayTo = To;
            }
            else
                req.birthDayTo = DateTime.Parse(BirthDayTo);
            req.native = Native;
            req.religion = Religion;
            req.ethnic = Ethnic;

            req.placeOfWork = PlaceOfWork;
            req.position = Position;
            req.departmentName = DepartmentName;

            req.eduName = EduName;
            req.educationType = EducationType;
            req.educationResult = EducationResult;

            req.diplomaName = DiplomaName;
            req.major = Major;
            req.diplomaEucationType = DiplomaEducationType;
            req.diplomaEducationOrg = DiplomaEducationOrg;
            req.educationLevel = DiplomaEducationLevel;
            req.certificateLevel = DiplomaCertificateLevel;

            req.certificateName = NameOfCertificate;
            req.level = CertificateLevel1;
            req.certificatetype = CertificateType;

            req.numberOfDecision = Number;
            req.reason = reason;
            req.form = Reward;
            TempData["DownloadExcel_Profile"] = getDataExportExcel(data, req);
            return Json(new { success = true, responseText = "get data successfuly" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 3. lay du lieu de export
        /// </summary>
        /// <param name="grpCheck"></param>
        /// <returns></returns>
        private string getDataExportExcel(GroupCheckBoxBO grpCheck, HumanEmployeeSearchRequest req)
        {
            try
            {
                if (grpCheck == null)
                    return null;
                var emps = v3_HumanEmployeeAPI.GetListByDepartment(req).Data;
                if (emps == null)
                    return null;
                /*TempData.Keep("EmployeeList");
                var emps = TempData["EmployeeList"];*/
                //int departmentID = (int)TempData["departmentID"];
                //var result = v3_HumanEmployeeAPI.GetListByDepartment(departmentID, 1000, 1).Result;
                //emps = result.Data;
                if (emps.Count() == 0)
                    return null;
                List<int> lstEmpID = new List<int>();
                foreach (var item in emps)
                {
                    lstEmpID.Add(item.ID);
                }

                // khởi tạo ds danh mục hồ sơ
                List<HumanCommonInformationDTO> lstCommon = new List<HumanCommonInformationDTO>();
                //IEnumerable<HumanPoliticInformationDTO> lstPolitic = null;
                List<HumanPoliticInformationDTO> lstPolitic = new List<HumanPoliticInformationDTO>();
                IEnumerable<V3_HumanProfileWorkExperienceDTO> lstWorkExp = null;
                IEnumerable<HumanProfileEducationTrainingExcel> lstTraining = null;
                IEnumerable<HumanProfileDiplomaExcel> lstDiploma = null;
                IEnumerable<HumanProfileCertificateExcel> lstCertificate = null;
                IEnumerable<HumanProfileRewardExcel> lstReward = null;

                // luôn check họ tên
                grpCheck.groupCV[1].FieldValue = true;
                List<CheckBoxBO> grpCV = grpCheck.groupCV.FindAll(i => i.FieldValue == true);
                List<CheckBoxBO> grpPolitic = grpCheck.groupPolitic.FindAll(i => i.FieldValue == true);
                List<CheckBoxBO> grpWorkExp = grpCheck.groupWorkExp.FindAll(i => i.FieldValue == true);
                List<CheckBoxBO> grpTraining = grpCheck.groupTraining.FindAll(i => i.FieldValue == true);
                List<CheckBoxBO> grpDiploma = grpCheck.groupDiploma.FindAll(i => i.FieldValue == true);
                List<CheckBoxBO> grpCerfificate = grpCheck.groupCertificate.FindAll(i => i.FieldValue == true);
                List<CheckBoxBO> grpReward = grpCheck.groupReward.FindAll(i => i.FieldValue == true);

                // Chỉ lấy dữ liệu về khi người dùng chọn
                // Lấy danh sách thông tin liên quan theo danh sách nhân viên(list ID)
                lstCommon = humanProfileCuriculmViateAPI.GetCommonInformationByListEmployeeID(lstEmpID).Result.Data;
                if (grpPolitic.Count > 0)
                    // nếu chọn dữ liệu lí lịch chính trị thì ghép bảng
                    lstPolitic = humanProfileCuriculmViateAPI.GetPoliticInformationByListEmployeeID(lstEmpID).Result.Data;
                var allEmps = from a in emps
                              join b in lstCommon on a.ID equals b.HumanEmployeeID into tmp
                              from b in tmp.DefaultIfEmpty()
                              join c in lstPolitic on a.ID equals c.HumanEmployeeID into tmp2
                              from c in tmp2.DefaultIfEmpty()
                              select new { a, b, c };

                if (grpWorkExp.Count > 0)
                    lstWorkExp = humanProfileWorkExperienceAPI.GetByListEmployeeID(lstEmpID).Result.Data;
                if (grpTraining.Count > 0)
                    lstTraining = humanProfileTrainingAPI.GetByListEmployeeID(lstEmpID).Result.Data;
                if (grpDiploma.Count > 0)
                    lstDiploma = humanProfileDiplomaAPI.GetByListEmployeeID(lstEmpID).Result.Data;
                if (grpCerfificate.Count > 0)
                    lstCertificate = humanProfileCertificateAPI.GetByListEmployeeID(lstEmpID).Result.Data;
                if (grpReward.Count > 0)
                    lstReward = humanProfileRewardAPI.GetByListEmployeeID(lstEmpID).Result.Data;

                var rowCount = (emps.Count() + 2).ToString();
                var colCount = (1 + grpCV.Count + grpPolitic.Count + grpWorkExp.Count + grpTraining.Count + grpDiploma.Count + grpCerfificate.Count + grpReward.Count).ToString();

                StringBuilder stringBuilder = new StringBuilder(@"<?xml version=""1.0""?>
               <?mso-application progid=""Excel.Sheet""?>
               <Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""
                xmlns:o=""urn:schemas-microsoft-com:office:office""
                xmlns:x=""urn:schemas-microsoft-com:office:excel""
                xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""
                xmlns:html=""http://www.w3.org/TR/REC-html40"">
                <DocumentProperties xmlns=""urn:schemas-microsoft-com:office:office"">
                 <Author>vncert</Author>
                 <LastAuthor>vncert</LastAuthor>
                 <Created>2019-10-25T04:14:19Z</Created>
                 <LastSaved>2019-10-25T06:45:54Z</LastSaved>
                 <Version>16.00</Version>
                </DocumentProperties>
                <OfficeDocumentSettings xmlns=""urn:schemas-microsoft-com:office:office"">
                 <AllowPNG/>
                </OfficeDocumentSettings>
                <ExcelWorkbook xmlns=""urn:schemas-microsoft-com:office:excel"">
                 <WindowHeight>7320</WindowHeight>
                 <WindowWidth>20490</WindowWidth>
                 <WindowTopX>0</WindowTopX>
                 <WindowTopY>0</WindowTopY>
                 <ProtectStructure>False</ProtectStructure>
                 <ProtectWindows>False</ProtectWindows>
                </ExcelWorkbook>
                <Styles>
                 <Style ss:ID=""Default"" ss:Name=""Normal"">
                  <Alignment ss:Vertical=""Bottom""/>
                  <Borders/>
                  <Font ss:FontName=""Calibri"" x:Family=""Swiss"" ss:Size=""11"" ss:Color=""#000000""/>
                  <Interior/>
                  <NumberFormat/>
                  <Protection/>
                 </Style>
                 <Style ss:ID=""s62"">
                  <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom""/>
                  <Font ss:FontName=""Times New Roman"" x:Family=""Roman"" ss:Size=""11"" ss:Color=""#000000"" ss:Bold=""1""/>
                 </Style>
                 <Style ss:ID=""s64"">
                  <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom""/>
                  <Font ss:FontName=""Calibri"" x:Family=""Swiss"" ss:Size=""12"" ss:Color=""#000000"" ss:Bold=""1""/>
                 </Style>
                 <Style ss:ID=""s65"">
                  <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""/>
                  <Font ss:FontName=""Times New Roman"" x:Family=""Roman"" ss:Size=""11"" ss:Color=""#000000"" ss:Bold=""1""/>
                 </Style>
                 <Style ss:ID=""s66"">
                  <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""/>
                 </Style>
                 <Style ss:ID=""s67"">
                  <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""/>
                  <Font ss:FontName=""Times New Roman"" x:Family=""Roman"" ss:Size=""11"" ss:Color=""#000000""/>
                 </Style>
                 <Style ss:ID=""s68"">
                  <Font ss:FontName=""Times New Roman"" x:Family=""Roman"" ss:Size=""11"" ss:Color=""#000000""/>
                 </Style>
                 <Style ss:ID=""s69"">
                  <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom""/>
                  <Font ss:FontName=""Times New Roman"" x:Family=""Roman"" ss:Size=""12"" ss:Color=""#000000"" ss:Bold=""1""/>
                 </Style>
                 <Style ss:ID=""s70"">
                  <Alignment ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                  <Font ss:FontName=""Times New Roman"" x:Family=""Roman"" ss:Size=""11"" ss:Color=""#000000""/>
                 </Style >
                </Styles>
                <Worksheet ss:Name=""Sheet1"">
                 <Table ss:ExpandedColumnCount=""" + colCount + @""" ss:ExpandedRowCount=""" + rowCount + @""" x:FullColumns=""1"" x:FullRows=""1"" ss:DefaultRowHeight=""15"">
                  <Row ss:Height=""15.75"" ss:StyleID=""s64"">");
                if (grpCV.Count > 0)
                    stringBuilder.Append(@"<Cell ss:MergeAcross=""" + (grpCV.Count).ToString() + @""" ss:StyleID=""s69""><Data ss:Type=""String"">Thông tin chung</Data></Cell>");
                if (grpPolitic.Count > 0)
                    stringBuilder.Append(@"<Cell ss:MergeAcross=""" + (grpPolitic.Count - 1).ToString() + @""" ss:StyleID=""s69""><Data ss:Type=""String"">Lý lịch chính trị</Data></Cell>");
                if (grpWorkExp.Count > 0)
                    stringBuilder.Append(@"<Cell ss:MergeAcross=""" + (grpWorkExp.Count - 1).ToString() + @""" ss:StyleID=""s69""><Data ss:Type=""String"">Quá trình công tác</Data></Cell>");
                if (grpTraining.Count > 0)
                    stringBuilder.Append(@"<Cell ss:MergeAcross=""" + (grpTraining.Count - 1).ToString() + @""" ss:StyleID=""s69""><Data ss:Type=""String"">Quá trình đào tạo</Data></Cell>");
                if (grpDiploma.Count > 0)
                    stringBuilder.Append(@"<Cell ss:MergeAcross=""" + (grpDiploma.Count - 1).ToString() + @""" ss:StyleID=""s69""><Data ss:Type=""String"">Văn bằng</Data></Cell>");
                if (grpCerfificate.Count > 0)
                    stringBuilder.Append(@"<Cell ss:MergeAcross=""" + (grpCerfificate.Count - 1).ToString() + @""" ss:StyleID=""s69""><Data ss:Type=""String"">Chứng chỉ</Data></Cell>");
                if (grpReward.Count > 0)
                    stringBuilder.Append(@"<Cell ss:MergeAcross=""" + (grpReward.Count - 1).ToString() + @""" ss:StyleID=""s69""><Data ss:Type=""String"">Khen thưởng</Data></Cell>");
                stringBuilder.Append(@"</Row>");
                stringBuilder.Append(@"<Row ss:Height=""14.25"" ss:StyleID=""s62"">");
                stringBuilder.Append(@"<Cell ss:StyleID=""s65""><Data ss:Type=""String"">STT</Data></Cell>");

                //Tiêu đề cột
                foreach (var item in grpCV)
                    stringBuilder.Append(@"<Cell><Data ss:Type=""String"">" + item.Title + "</Data></Cell>");

                foreach (var item in grpPolitic)
                    stringBuilder.Append(@"<Cell><Data ss:Type=""String"">" + item.Title + "</Data></Cell>");

                foreach (var item in grpWorkExp)
                    stringBuilder.Append(@"<Cell><Data ss:Type=""String"">" + item.Title + "</Data></Cell>");

                foreach (var item in grpTraining)
                    stringBuilder.Append(@"<Cell><Data ss:Type=""String"">" + item.Title + "</Data></Cell>");

                foreach (var item in grpDiploma)
                    stringBuilder.Append(@"<Cell><Data ss:Type=""String"">" + item.Title + "</Data></Cell>");
                foreach (var item in grpCerfificate)
                    stringBuilder.Append(@"<Cell><Data ss:Type=""String"">" + item.Title + "</Data></Cell>");

                foreach (var item in grpReward)
                    stringBuilder.Append(@"<Cell><Data ss:Type=""String"">" + item.Title + "</Data></Cell>");

                stringBuilder.Append(@"</Row>");
                int count = 0;
                // danh sách row
                foreach (var emp in allEmps)
                {
                    count += 1;
                    stringBuilder.Append(@"<Row ss:AutoFitHeight=""0"" ss:StyleID=""s68"">");
                    stringBuilder.Append(@"<Cell ss:StyleID=""s67""><Data ss:Type=""Number"">" + count.ToString() + @"</Data></Cell>");

                    foreach (var item in grpCV)
                        stringBuilder.Append(@"<Cell><Data ss:Type=""String"">" + getValueCell_CommonInformation(emp.a, item.FieldLabel, emp.b) + @"</Data></Cell>");

                    foreach (var item in grpPolitic)
                        stringBuilder.Append(@"<Cell><Data ss:Type=""String"">" + getValueCell_PoliticInformation(emp.c, item.FieldLabel) + @"</Data></Cell>");

                    foreach (var item in grpWorkExp)
                        stringBuilder.Append(@"<Cell><Data ss:Type=""String"">" + getValueCell_WorksExp(emp.a, item.FieldLabel, lstWorkExp) + @"</Data></Cell>");

                    foreach (var item in grpTraining)
                        stringBuilder.Append(@"<Cell ss:StyleID=""s70""><Data ss:Type=""String"">" + getValueCell_Training(emp.a, item.FieldLabel, lstTraining) + @"</Data></Cell>");

                    foreach (var item in grpDiploma)
                        stringBuilder.Append(@"<Cell ss:StyleID=""s70""><Data ss:Type=""String"">" + getValueCell_Diploma(emp.a, item.FieldLabel, lstDiploma) + @"</Data></Cell>");

                    foreach (var item in grpCerfificate)
                        stringBuilder.Append(@"<Cell ss:StyleID=""s70""><Data ss:Type=""String"">" + getValueCell_Certificate(emp.a, item.FieldLabel, lstCertificate) + @"</Data></Cell>");

                    foreach (var item in grpReward)
                        stringBuilder.Append(@"<Cell ss:StyleID=""s70""><Data ss:Type=""String"">" + getValueCell_Reward(emp.a, item.FieldLabel, lstReward) + @"</Data></Cell>");

                    stringBuilder.Append(@"</Row>");
                }


                stringBuilder.Append(@"</Table>
                 <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                  <PageSetup>
                   <Header x:Margin=""0.3""/>
                   <Footer x:Margin=""0.3""/>
                   <PageMargins x:Bottom=""0.75"" x:Left=""0.7"" x:Right=""0.7"" x:Top=""0.75""/>
                  </PageSetup>
                  <Selected/>
                  <Panes>
                   <Pane>
                    <Number>3</Number>
                    <ActiveRow>8</ActiveRow>
                    <ActiveCol>6</ActiveCol>
                   </Pane>
                  </Panes>
                  <ProtectObjects>False</ProtectObjects>
                  <ProtectScenarios>False</ProtectScenarios>
                 </WorksheetOptions>
                </Worksheet>
               </Workbook>
               ");

                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 4. tai file du lieu complete
        /// </summary>
        public void DownloadProfile()
        {
            var data = TempData["DownloadExcel_Profile"];
            TempData.Keep("DownloadExcel_Profile");
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=DanhSachNhanSu.xls");
            Response.Write(data);
            Response.End();
        }
        /// <summary>
        /// Chuyen doi dia chi
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="districtId"></param>
        /// <param name="CommuneId"></param>
        /// <returns></returns>
        private string ConvertAddress(string cityName, string districtName, string CommuneName)
        {
            string[] arr = { CommuneName, districtName, cityName };
            return String.Join(" - ", arr);
        }

        /// <summary>
        /// Lấy hô sơ kinh nghiệm làm việc
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="fieldName"></param>
        /// <param name="lstWorkExp"></param>
        /// <returns></returns>
        private string getValueCell_WorksExp(V3_HumanEmployeeResponse emp, string fieldName, IEnumerable<V3_HumanProfileWorkExperienceDTO> lstWorkExp)
        {
            if (lstWorkExp == null || lstWorkExp.Count() == 0)
                return string.Empty;

            var objs = lstWorkExp.Where(i => i.HumanEmployeeId == emp.ID);
            if (objs == null || objs.Count() == 0)
                return string.Empty;

            switch (fieldName)
            {
                case "StartDate":
                    var StartDate = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(StartDate))
                                StartDate += ((DateTime)o.StartDate).ToString("dd/MM/yyyy");
                            else
                                StartDate += "&#10;" + ((DateTime)o.StartDate).ToString("dd/MM/yyyy");
                        }
                    }
                    catch
                    {
                        return StartDate;
                    }
                    return StartDate;
                case "EndDate":
                    var EndDate = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(EndDate))
                                EndDate += ((DateTime)o.EndDate).ToString("dd/MM/yyyy");
                            else
                                EndDate += "&#10;" + ((DateTime)o.EndDate).ToString("dd/MM/yyyy");
                        }
                    }
                    catch
                    {
                        return EndDate;
                    }
                    return EndDate;
                case "PlaceOfWork":
                    var PlaceOfWork = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(PlaceOfWork))
                                PlaceOfWork += o.PlaceOfWork;
                            else
                                PlaceOfWork += "&#10;" + o.PlaceOfWork;
                        }
                    }
                    catch
                    {
                        return PlaceOfWork;
                    }
                    return PlaceOfWork;
                case "Position":
                    var Position = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(Position))
                                Position += o.Position;
                            else
                                Position += "&#10;" + o.Position;
                        }
                    }
                    catch
                    {
                        return Position;
                    }
                    return Position;
                case "Department":
                    var Department = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(Department))
                                Department += o.Department;
                            else
                                Department += "&#10;" + o.Department;
                        }
                    }
                    catch
                    {
                        return Department;
                    }
                    return Department;
                case "JobDescription":
                    var JobDescription = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(JobDescription))
                                JobDescription += o.JobDescription;
                            else
                                JobDescription += "&#10;" + o.JobDescription;
                        }
                    }
                    catch
                    {
                        return JobDescription;
                    }
                    return JobDescription;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Lấy hồ sơ đào tạo
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="fieldName"></param>
        /// <param name="lstTraining"></param>
        /// <returns></returns>
        private string getValueCell_Training(V3_HumanEmployeeResponse emp, string fieldName, IEnumerable<HumanProfileEducationTrainingExcel> lstTraining)
        {
            if (lstTraining == null || lstTraining.Count() == 0)
                return string.Empty;

            var objs = lstTraining.Where(i => i.HumanEmployeeID == emp.ID);
            if (objs == null || objs.Count() == 0)
                return string.Empty;

            switch (fieldName)
            {
                case "Name":
                    var NameEducationTraining = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(NameEducationTraining))
                                NameEducationTraining += o.NameEducationTraining;
                            else
                                NameEducationTraining += "&#10;" + o.NameEducationTraining;
                        }
                    }
                    catch
                    {
                        return NameEducationTraining;
                    }
                    return NameEducationTraining;
                case "Form":
                    var NameEducationType = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(NameEducationType))
                                NameEducationType += o.NameEducationType;
                            else
                                NameEducationType += "&#10;" + o.NameEducationType;
                        }
                    }
                    catch
                    {
                        return NameEducationType;
                    }
                    return NameEducationType;
                case "Content":
                    var Content = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(Content))
                                Content += o.Content;
                            else
                                Content += "&#10;" + o.Content;
                        }
                    }
                    catch
                    {
                        return Content;
                    }
                    return Content;
                case "Certificate":
                    var Certificate = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(Certificate))
                                Certificate += o.Certificate;
                            else
                                Certificate += "&#10;" + o.Certificate;
                        }
                    }
                    catch
                    {
                        return Certificate;
                    }
                    return Certificate;
                case "Result":
                    var NameEducationResult = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(NameEducationResult))
                                NameEducationResult += o.NameEducationResult;
                            else
                                NameEducationResult += "&#10;" + o.NameEducationResult;
                        }
                    }
                    catch
                    {
                        return NameEducationResult;
                    }
                    return NameEducationResult;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Lấy hồ sơ văn bằng
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="fieldName"></param>
        /// <param name="lstDiploma"></param>
        /// <returns></returns>
        private string getValueCell_Diploma(V3_HumanEmployeeResponse emp, string fieldName, IEnumerable<HumanProfileDiplomaExcel> lstDiploma)
        {
            if (lstDiploma == null || lstDiploma.Count() == 0)
                return string.Empty;

            var objs = lstDiploma.Where(i => i.HumanEmployeeID == emp.ID);
            if (objs == null || objs.Count() == 0)
                return string.Empty;
            switch (fieldName)
            {
                // tên
                case "Name":
                    var NameDiploma = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(NameDiploma))
                                NameDiploma += o.Faculty;
                            else
                                NameDiploma += "&#10;" + o.NameDiploma;

                        }
                    }
                    catch
                    {
                        return NameDiploma;
                    }
                    return NameDiploma;
                // khoa
                case "Faculty":
                    var Faculty = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(Faculty))
                                Faculty += o.Faculty;
                            else
                                Faculty += "&#10;" + o.Faculty;

                        }
                    }
                    catch
                    {
                        return Faculty;
                    }
                    return Faculty;
                // chuyên ngành
                case "Major":
                    var NameEducationField = "";
                    try
                    {
                        foreach (var o in objs)
                        {

                            // tách dòng
                            if (string.IsNullOrWhiteSpace(NameEducationField))
                                NameEducationField += o.NameEducationField;
                            else
                                NameEducationField += "&#10;" + o.NameEducationField;

                        }
                    }
                    catch
                    {
                        return NameEducationField;
                    }
                    return NameEducationField;
                case "Place":
                    var NameEducationOrg = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(NameEducationOrg))
                                NameEducationOrg += o.NameEducationOrg;
                            else
                                NameEducationOrg += "&#10;" + o.NameEducationOrg;
                        }
                    }
                    catch
                    {
                        return NameEducationOrg;
                    }
                    return NameEducationOrg;
                case "Level":
                    var NameEducationLevel = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(NameEducationLevel))
                                NameEducationLevel += o.NameEducationLevel;
                            else
                                NameEducationLevel += "&#10;" + o.NameEducationLevel;
                        }
                    }
                    catch
                    {
                        return NameEducationLevel;
                    }
                    return NameEducationLevel;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Lấy hồ sơ chứng chỉ
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="fieldName"></param>
        /// <param name="lstCertificate"></param>
        /// <returns></returns>
        private string getValueCell_Certificate(V3_HumanEmployeeResponse emp, string fieldName, IEnumerable<HumanProfileCertificateExcel> lstCertificate)
        {
            if (lstCertificate == null || lstCertificate.Count() == 0)
                return string.Empty;

            var objs = lstCertificate.Where(i => i.HumanEmployeeID == emp.ID);
            if (objs == null || objs.Count() == 0)
                return string.Empty;
            switch (fieldName)
            {
                case "Name":
                    var Name = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(Name))
                                Name += o.NameCertificate;
                            else
                                Name += "&#10;" + o.NameCertificate;
                        }
                    }
                    catch
                    {
                        return Name;
                    }
                    return Name;
                case "Level":
                    var Level = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(Level))
                                Level += o.NameCertificateLevel;
                            else
                                Level += "&#10;" + o.NameCertificateLevel;
                        }
                    }
                    catch
                    {
                        return Level;
                    }
                    return Level;
                case "CertificateType":
                    var CertificateType = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(CertificateType))
                                CertificateType += o.NameCertificateType;
                            else
                                CertificateType += "&#10;" + o.NameCertificateType;
                        }
                    }
                    catch
                    {
                        return CertificateType;
                    }
                    return CertificateType;
                case "PlaceOfTraining":
                    var PlaceOfTraining = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(PlaceOfTraining))
                                PlaceOfTraining += o.PlaceOfTraining;
                            else
                                PlaceOfTraining += "&#10;" + o.PlaceOfTraining;
                        }
                    }
                    catch
                    {
                        return PlaceOfTraining;
                    }
                    return PlaceOfTraining;
                case "DateIssuance":
                    var DateIssuance = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(DateIssuance))
                                DateIssuance += ((DateTime)o.DateIssuance).ToString("dd/MM/yyyy");
                            else
                                DateIssuance += "&#10;" + ((DateTime)o.DateIssuance).ToString("dd/MM/yyyy");
                        }
                    }
                    catch
                    {
                        return DateIssuance;
                    }
                    return DateIssuance;
                case "DateExpiration":
                    var DateExpiration = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(DateExpiration))
                                DateExpiration += ((DateTime)o.DateExpiration).ToString("dd/MM/yyyy");
                            else
                                DateExpiration += "&#10;" + ((DateTime)o.DateExpiration).ToString("dd/MM/yyyy");
                        }
                    }
                    catch
                    {
                        return DateExpiration;
                    }
                    return DateExpiration;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Lấy hồ sơ khen thưởng
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="fieldName"></param>
        /// <param name="lstReward"></param>
        /// <returns></returns>
        private string getValueCell_Reward(V3_HumanEmployeeResponse emp, string fieldName, IEnumerable<HumanProfileRewardExcel> lstReward)
        {
            if (lstReward == null || lstReward.Count() == 0)
                return string.Empty;

            var objs = lstReward.Where(i => i.HumanEmployeeID == emp.ID);
            if (objs == null || objs.Count() == 0)
                return string.Empty;

            switch (fieldName)
            {
                case "NumberOfDecision":
                    var NumberOfDecision = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(NumberOfDecision))
                                NumberOfDecision += o.NumberOfDecision;
                            else
                                NumberOfDecision += "&#10;" + o.NumberOfDecision;
                        }
                    }
                    catch
                    {
                        return NumberOfDecision;
                    }
                    return NumberOfDecision;
                case "DateOfDecision":
                    var DateOfDecision = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(DateOfDecision))
                                DateOfDecision += ((DateTime)o.DateOfDecision).ToString("dd/MM/yyyy");
                            else
                                DateOfDecision += "&#10;" + ((DateTime)o.DateOfDecision).ToString("dd/MM/yyyy");
                        }
                    }
                    catch
                    {
                        return DateOfDecision;
                    }
                    return DateOfDecision;
                case "Reason":
                    var Reason = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(Reason))
                                Reason += o.Reason;
                            else
                                Reason += "&#10;" + o.Reason;
                        }
                    }
                    catch
                    {
                        return Reason;
                    }
                    return Reason;
                case "Form":
                    var Form = "";
                    try
                    {
                        foreach (var o in objs)
                        {
                            // tách dòng
                            if (string.IsNullOrWhiteSpace(Form))
                                Form += o.Name;
                            else
                                Form += "&#10;" + o.Name;
                        }
                    }
                    catch
                    {
                        return Form;
                    }
                    return Form;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Lấy thông tin chung hồ sơ lý lịch
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="fieldName"></param>
        /// <param name="lstCV"></param>
        /// <returns></returns>
        private string getValueCell_CommonInformation(V3_HumanEmployeeResponse emp, string fieldName, HumanCommonInformationDTO item)
        {
            if (fieldName.Equals("Code"))
                return emp.Code;
            if (fieldName.Equals("Name"))
                return emp.Name;

            switch (fieldName)
            {
                case "OfficePhone":
                    return emp.Phone;

                case "Sex":
                    if (emp.Gender == true)
                        return "Nam";
                    else
                        return "Nữ";
                case "DateOfBirth":
                    if (emp.BirthDay == null)
                        return string.Empty;
                    else
                        return ((DateTime)emp.BirthDay).ToString("dd/MM/yyyy");

                default:
                    break;
            }

            if (item == null)
                return string.Empty;
            switch (fieldName)
            {
                case "PlaceIssueOfIdentityCard":
                    return item.PlaceIssueOfIdentityCard;
                case "HomeAddress":
                    return ConvertAddress(item.CityNameHomeTown, item.DistrictNameHomeTown, item.CommuneNameHomeTown);
                case "PermanentResidence":
                    return item.residence;
                case "CurrentResidence":
                    return item.currentAddress;
                case "TaxCode":
                    return item.NumberOfPartyCard;
                case "NumberOfBankAccounts":
                    return item.NumberOfBankAccounts;
                case "Bank":
                    return item.Bank;
                case "NumberOfPassport":
                    return item.NumberOfPassport;
                case "BornAddress":
                    return ConvertAddress(item.CityName, item.DistrictName, item.CommuneName);
                case "PlaceOfPassport":
                    return item.PlaceOfPassport;
                case "DateOfIssuePassport":
                    if (item.DateOfIssuePassport == null)
                        return string.Empty;
                    else
                        return ((DateTime)item.DateOfIssuePassport).ToString("dd/MM/yyyy");
                case "PassportExpirationDate":
                    if (item.PassportExpirationDate == null)
                        return string.Empty;
                    else
                        return ((DateTime)item.PassportExpirationDate).ToString("dd/MM/yyyy");
                case "Religion":
                    return item.ReligionName;
                case "People":
                    return item.EthnicName;
                case "NumberOfIdentityCard":
                    return item.NumberOfIdentityCard;
                case "DateIssueOfIdentityCard":
                    if (item.PassportExpirationDate == null)
                        return string.Empty;
                    else
                        return ((DateTime)item.DateIssueOfIdentityCard).ToString("dd/MM/yyyy");
                default:
                    return string.Empty;
            }
        }
        /// <summary>
        /// Lấy thông tin lý lịch chính trị
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="fieldName"></param>
        /// <param name="lstCV"></param>
        /// <returns></returns>
        private string getValueCell_PoliticInformation(HumanPoliticInformationDTO emp, string fieldName)
        { 
            if (emp == null)
                return string.Empty;
            switch (fieldName)
            {
                //đoàn
                case "DateOnGroup":
                    if (emp.DateOnGroup == null)
                        return string.Empty;
                    else
                        return ((DateTime)emp.DateOnGroup).ToString("dd/MM/yyyy");
                case "PositionGroup":
                    return emp.YouthPositionName;
                case "PlaceOfLoadedGroup":
                    return emp.PlaceOfLoadedGroup;
                //đảng
                case "DateJoinRevolution":
                    if (emp.DateJoinRevolution == null)
                        return string.Empty;
                    else
                        return ((DateTime)emp.DateJoinRevolution).ToString("dd/MM/yyyy");
                case "DateOfJoinParty":
                    if (emp.DateOfJoinParty == null)
                        return string.Empty;
                    else
                        return ((DateTime)emp.DateOfJoinParty).ToString("dd/MM/yyyy");
                case "PlaceOfLoadedParty":
                    return emp.PlaceOfLoadedParty;
                case "PosititonParty":
                    return emp.PartyPositionName;
                case "NumberOfPartyCard":
                    return emp.NumberOfPartyCard;
                //quân đội
                case "DateOnArmy":
                    if (emp.DateOnArmy == null)
                        return string.Empty;
                    else
                        return ((DateTime)emp.DateOnArmy).ToString("dd/MM/yyyy");
                case "PositionArmy":
                    return emp.PositionArmy;
                case "ArmyRank":
                    return emp.MilitaryPositionLevelName;
              case "PoliticalTheory":
                    return emp.PoliticalTheoryName;
              case "GovermentManagement":
                    return emp.GovermentManagementName;
                default:
                    return string.Empty;
            }
        }
    }
}