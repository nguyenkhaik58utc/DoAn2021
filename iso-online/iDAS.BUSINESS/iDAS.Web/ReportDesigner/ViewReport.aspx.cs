using iDAS.Core;
using iDAS.Services;
using iDAS.Web.API.Human;
using iDAS.Web.Areas.Generic.Controllers;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iDAS.Web.ReportDesigner
{
    public partial class ViewReport : System.Web.UI.Page
    {
        const string defaultAvatar = "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";
        private V3_HumanProfileDiplomaAPI humanProfileDiplomaAPI = new V3_HumanProfileDiplomaAPI();
        private V3_HumanProfileRewardAPI humanProfileRewardAPI = new V3_HumanProfileRewardAPI();
        private V3_HumanProfileCuriculmViateAPI humanProfileCuriculmViateAPI = new V3_HumanProfileCuriculmViateAPI();
        private V3_HumanProfileCertificateAPI humanProfileCertificateAPI = new V3_HumanProfileCertificateAPI();
        private V3_HumanProfileTrainingAPI humanProfileTrainingAPI = new V3_HumanProfileTrainingAPI();
        private V3_HumanProfileWorkExperienceAPI humanProfileWorkExperienceAPI = new V3_HumanProfileWorkExperienceAPI();
        private V3_HumanProfileDisciplineAPI humanProfileDisciplineAPI = new V3_HumanProfileDisciplineAPI();
        private V3_HumanProfileAssessAPI humanProfileAssessAPI = new V3_HumanProfileAssessAPI();
        private V3_HumanProfileAttachAPI humanProfileAttachAPI = new V3_HumanProfileAttachAPI();
        private V3_HumanProfileContractAPI humanProfileContractAPI = new V3_HumanProfileContractAPI();
        private V3_HumanProfileRelationshipAPI humanProfileRelationshipAPI = new V3_HumanProfileRelationshipAPI();
        private V3_HumanProfileInsuranceAPI humanProfileInsuranceAPI = new V3_HumanProfileInsuranceAPI();
        private V3_HumanProfileSalaryAPI humanProfileSalaryAPI = new V3_HumanProfileSalaryAPI();
        private V3_HumanEmployeeAPI humanEmployeeAPI = new V3_HumanEmployeeAPI();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var employeeId = Request.QueryString["emp"];
                if (!string.IsNullOrEmpty(employeeId))
                    SetLocalReport(int.Parse(employeeId));
            }
        }

        private void SetLocalReport(int employeeId)
        {
            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.Width = Unit.Percentage(100);
            ReportViewer1.Height = Unit.Percentage(100);

            //var ms = new ProfileMasterService();

            var profileWorkExperience = humanProfileWorkExperienceAPI.GetByEmployeeID(employeeId).Result.Data;
            profileWorkExperience = profileWorkExperience.OrderByDescending(o => o.StartDate).ToList();
            
            var work = (new[] { new { StartDate = DateTime.Now, EndDate = DateTime.Now, Content = "" } }).ToList();
            foreach (var v in profileWorkExperience)
            {
                if (v.StartDate.HasValue)
                {
                    var endDate = v.EndDate ?? DateTime.Now;
                    var content = string.Join(", ", new[] { v.Position, v.Department, v.PlaceOfWork }.Where(w => !string.IsNullOrEmpty(w)));
                    work.Add(new { StartDate = (DateTime)v.StartDate, EndDate = endDate, Content = content });
                }
            }
            var profileworktraining = humanProfileTrainingAPI.GetByEmployeeID(employeeId).Result.Data;
            foreach (var v in profileworktraining)
            {
                if (v.StartDate.HasValue)
                {
                    var enddate = v.EndDate ?? DateTime.Now;
                    var startdate = v.StartDate ?? DateTime.Now;
                    var form = v.EducationTypeName;
                    var content = string.Join(", ", new[] { v.Name, v.Content, form }.Where(w => !string.IsNullOrEmpty(w)));

                    var datespan = string.Format("dd/mm/yyyy", (DateTime)v.StartDate) + " - " + string.Format("dd/mm/yyyy", enddate);
                    work.Add(new { StartDate = startdate, EndDate = enddate, Content = content });
                }
            }
            work.RemoveAt(0);
            work = work.OrderBy(o => o.StartDate).ToList();

            var profileDiploma = humanProfileDiplomaAPI.GetByEmployeeID(employeeId).Result.Data;
            //Lấy trình độ cao nhất
            //var diplomas = ms.GetMasterDataList("educationLevel");
            //var listDiploma = new List<string[]>();
            //foreach (DataRow dr in diplomas.Rows)
            //{
            //    var o = new[] { dr["Id"].ToString(), dr["Name"].ToString(), dr["Rank"].ToString() };
            //    listDiploma.Add(o);
            //}

            //var listdiploma1 = listDiploma.Except(listDiploma.Where(w => !int.TryParse(w[2], out int z)).ToList()).ToList();

            //var highestLevel = "";
            //foreach (var v in listdiploma1)
            //{
            //    var x = v[0];
            //    var level = profileDiploma.FirstOrDefault(f => f.Level.ToString() == v[0]);
            //    if (level != null)
            //    {
            //        highestLevel = v[1];
            //        break;
            //    }
            //}
            //foreach (var d in profileDiploma)
            //{
            //    var timespan = "";
            //    if (d.StartDate.HasValue && d.EndDate.HasValue)
            //    {
            //        timespan = ((DateTime)d.StartDate).ToString("dd/MM/yyyy") + " - " + ((DateTime)d.EndDate).ToString("dd/MM/yyyy");
            //    }
            //    d.Faculty = timespan;
            //}

            var profileCertificate = humanProfileCertificateAPI.GetByEmployeeID(employeeId).Result.Data;
            var profileReward = humanProfileRewardAPI.GetByEmployeeID(employeeId).Result.Data;
            var profileDiscipline = humanProfileDisciplineAPI.GetByEmployeeID(employeeId).Result.Data;
            var profileAssess = humanProfileAssessAPI.GetByEmployeeID(employeeId).Result.Data;
            var profileContract = humanProfileContractAPI.GetByEmployeeID(employeeId).Result.Data;
            var profileRelationship = humanProfileRelationshipAPI.GetByEmployeeID(employeeId).Result.Data;
            foreach (var v in profileRelationship)
            {
                v.Job = string.Join(", ", new[] { v.Job, v.PlaceOfJob }.Where(w => !string.IsNullOrEmpty(w)));
            }
            var partnerRelationship = profileRelationship.Where(w => w.IsSelf == false).ToList();
            var selfRelationship = profileRelationship.Except(partnerRelationship).ToList();

            var profileSalary = humanProfileSalaryAPI.GetByEmployeeID(employeeId).Result.Data;
           // profileSalary = profileSalary.OrderByDescending(o => o.DateOfApp).ToList();
            var profileInsurance = humanProfileInsuranceAPI.GetByEmployeeID(employeeId).Result.Data;
            var profileAttach = humanProfileAttachAPI.GetByEmployeeID(employeeId).Result.Data;
            //var auditTickDetail = profileWorkTrialService.GetByEmployeeNotPaging(employeeId).ToList();

            ReportViewer1.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"ReportDesigner\EmployeeProfile2C.rdlc";

            // ReportViewer1.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"ReportDesigner\EmployeeProfile_1.rdlc";

            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileWorkExperienceDetail", profileWorkExperience));
            // 28. tóm tắt quá trình công tác (quá trình công tác + quá trình đào tạo)
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileTraining", work));

            // 27. Đào tạo, bồi dưỡng về chuyên môn, nghiệp vụ, lý luận chính trị, ngoại ngữ, tin học(hồ sơ văn bằng)
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileDiplomaDetail", profileDiploma));

            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileCertificateDetail", profileCertificate));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileRewardDetail", profileReward));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileDisciplineDetail", profileDiscipline));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileAssessDetail", profileAssess));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileContractDetail", profileContract));
            //30) Quan hệ gia đình(bản thân và vợ/chồng)
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileSelfRelationshipDetail", selfRelationship));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfilePartnerRelationshipDetail", partnerRelationship));
            //31) Diễn biến quá trình lương của cán bộ, công chức()
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileSalaryDetail", profileSalary));

            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileInsuranceDetail", profileInsurance));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsProfileAttachDetail", profileAttach));

            //ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsAuditTickDetail", auditTickDetail));

            var data = humanProfileCuriculmViateAPI.GetFullByID(employeeId).Result;
            var reportParams = new List<ReportParameter>();
            if (data != null)
            {
                reportParams.Add(new ReportParameter("Aliases", data.Aliases));
                var birthDay = ""; var birthMonth = ""; var birthYear = "";
                if (data.BirthDay.HasValue)
                {
                    var dt = (DateTime)data.BirthDay;
                    birthDay = dt.Day.ToString();
                    birthMonth = dt.Month.ToString();
                    birthYear = dt.Year.ToString();
                }
                reportParams.Add(new ReportParameter("BirthDay", birthDay));
                reportParams.Add(new ReportParameter("BirthMonth", birthMonth));
                reportParams.Add(new ReportParameter("BirthYear", birthYear));
                reportParams.Add(new ReportParameter("IsMale", data.Gender == true ? "Nam" : data.Gender == false ? "Nữ" : "Nam"));
                reportParams.Add(new ReportParameter("BornCommune", data.CommuneIDOfBirthName));
                reportParams.Add(new ReportParameter("BornDistrict", data.DistrictIDOfBirthName));
                reportParams.Add(new ReportParameter("BornCity", data.CityIDOfBirthName));
                reportParams.Add(new ReportParameter("HomeCommune", data.CommuneNameHomeTown));
                reportParams.Add(new ReportParameter("HomeDistrict", data.DistrictNameHomeTown));
                reportParams.Add(new ReportParameter("HomeCity", data.CityNameHomeTown));
                reportParams.Add(new ReportParameter("Ethnic", data.EthnicName));
                reportParams.Add(new ReportParameter("Religion", data.ReligionName));
                reportParams.Add(new ReportParameter("PermanentResidence", data.residence));
                reportParams.Add(new ReportParameter("CurrentResidence", data.currentAddress));
                reportParams.Add(new ReportParameter("PoliticalTheory", data.PoliticalTheoryName));
                reportParams.Add(new ReportParameter("GovermentManagement", data.GovermentManagementName));
                reportParams.Add(new ReportParameter("NumberOfIdentityCard", data.NumberOfIdentityCard));
                reportParams.Add(new ReportParameter("DateIssueOfIdentityCard", data.DateIssueOfIdentityCard.HasValue ? String.Format("{0:dd/MM/yyyy}", data.DateIssueOfIdentityCard) : string.Empty));
                reportParams.Add(new ReportParameter("DateOnGroup", data.DateOnGroup.HasValue ? String.Format("{0:dd/MM/yyyy}", data.DateOnGroup) : string.Empty));
                reportParams.Add(new ReportParameter("PositionGroup", data.YouthPositionName));
                reportParams.Add(new ReportParameter("DateAtParty", data.DateAtParty.HasValue ? String.Format("{0:dd/MM/yyyy}", data.DateAtParty) : string.Empty));
                reportParams.Add(new ReportParameter("DateOfJoinParty", data.DateOfJoinParty.HasValue ? String.Format("{0:dd/MM/yyyy}", data.DateOfJoinParty) : string.Empty));
                reportParams.Add(new ReportParameter("DateOnArmy", data.DateOnMilitary.HasValue ? String.Format("{0:dd/MM/yyyy}", data.DateOnMilitary) : string.Empty));
                reportParams.Add(new ReportParameter("ArmyRank", data.MilitaryPositionLevelName));
                reportParams.Add(new ReportParameter("Forte", data.Forte));
               // reportParams.Add(new ReportParameter("HighestLevel", ""));
                //var disciplines = new VPHumanProfileDisciplineService().GetAllByEmployeeIdNotPaging(employeeId);
                //var disciplineMst = ms.GetMasterDataList("discipline").Rows;
                //var listDiscipline = new List<string[]>();
                //foreach (DataRow dr in disciplineMst)
                //{
                //    var o = new[] { dr["Id"].ToString(), dr["Name"].ToString(), dr["Rank"].ToString() };
                //    listDiscipline.Add(o);
                //}
                //listDiscipline = listDiscipline.OrderBy(o => int.Parse(o[2])).ToList();
                //var disciplineForm = "";
                //var disciplineYear = "";
                //foreach (var v in listDiscipline)
                //{
                //    var highestDiscipline = disciplines.FirstOrDefault(w => w.Form.ToString() == v[0]);
                //    if (highestDiscipline != null)
                //    {
                //        disciplineForm = highestDiscipline.Form.ToString();
                //        disciplineYear = highestDiscipline.DateOfDecision.HasValue ? " (" + ((DateTime)highestDiscipline.DateOfDecision).Year.ToString() + ")" : "";
                //        break;
                //    }
                //}
                //var disciplineName = "";
                //disciplineName = disciplineForm == "" ? "" : ms.GetSingleMasterData("discipline", disciplineForm).Name;
                //reportParams.Add(new ReportParameter("Discipline", disciplineName + disciplineYear));

                //var rewards = humanProfileRewardAPI.GetByEmployeeID(employeeId);
                //var rewardMst = ms.GetMasterDataList("award").Rows;
                //var listReward = new List<string[]>();
                //foreach (DataRow dr in rewardMst)
                //{
                //    var o = new[] { dr["Id"].ToString(), dr["Name"].ToString(), dr["Rank"].ToString() };
                //    listReward.Add(o);
                //}
                //listReward = listReward.OrderBy(o => int.Parse(o[2])).ToList();
                //var rewardForm = "";
                //var rewardYear = "";
                //foreach (var v in listReward)
                //{
                //    var highestReward = rewards.FirstOrDefault(w => w.Form.ToString() == v[0]);
                //    if (highestReward != null)
                //    {
                //        rewardForm = highestReward.Form.ToString();
                //        rewardYear = highestReward.DateOfDecision.HasValue ? " (" + ((DateTime)highestReward.DateOfDecision).Year.ToString() + ")" : "";
                //        break;
                //    }
                //}
                //var rewardName = "";
                //rewardName = rewardForm == "" ? "" : ms.GetSingleMasterData("award", rewardForm).Name;
                //reportParams.Add(new ReportParameter("Reward", rewardName + rewardYear));

                //}
                //ReportViewer1.LocalReport.EnableExternalImages = true;
                reportParams.Add(new ReportParameter("EmployeeName", data.Name == null ? string.Empty : data.Name.ToUpper()));
                var avatarUrl = defaultAvatar;
                if (data.FileID!=null)
                {
                    avatarUrl = GetFile(data.FileID);
                }
                reportParams.Add(new ReportParameter("Avatar", avatarUrl));
                ReportViewer1.LocalReport.SetParameters(reportParams);
            }
        }

        private string GetFile(Guid? fileId)
        {
            string tmp = "";
            FileSV fileSV = new FileSV();
            if (fileId != null)
            {
                var file = fileSV.GetById((Guid)fileId);
                if (file != null)
                {
                    var byedata = file.Data;
                    if (byedata != null)
                    {
                        tmp = Convert.ToBase64String(byedata);
                    }
                }
            }
            return tmp;
        }
    }
}