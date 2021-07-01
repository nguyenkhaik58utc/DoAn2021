using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class HumanTrainingPractionersSV
    {

        private HumanTrainingPractionerDA practionersDA = new HumanTrainingPractionerDA();
        public bool CheckInvalidEmployees(int employeesId, int detailId)
        {
            var obj = practionersDA.GetQuery(t => t.HumanEmployeeID == employeesId 
                && t.HumanTrainingPlanDetailID == detailId
                ).FirstOrDefault();
            if (obj != null)
                return true;
            return false;
        }
        public bool CheckOverNumber(int detailId, int number)
        {
            var obj = practionersDA.GetQuery(
                t => t.HumanTrainingPlanDetailID == detailId
                ).ToList();
            if (obj.Count >= number)
                return true;
            return false;
        }
        public void Insert(HumanTrainingPractionersItem objNew, int userId, int employeesId)
        {

            var obj = new HumanTrainingPractioner();
            obj.HumanTrainingPlanDetailID = objNew.DetailID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.HumanEmployeeID = objNew.EmployeesRegister.ID;
            obj.IsAcceptCommit = objNew.IsAcceptCommit;
            obj.IsRegister = true;
            obj.TimeRegister = DateTime.Now;
            practionersDA.Insert(obj);
            practionersDA.Save();
        }
        public HumanTrainingPractionersItem GetByID(int id)
        {
            var data = practionersDA.GetById(id);
            var employeeSV = new HumanEmployeeSV();
            var obj = new HumanTrainingPractionersItem();
            if (data != null)
                obj.ID = data.ID;
            obj.NumberAbsence = data.NumberAbsence;
            obj.NumberPresence = data.NumberPresence;
            obj.RankID = data.Rank;
            obj.FormName = data.HumanTrainingPlanDetail.Type == true ? "Bên ngoài" : "Nội bộ";
            obj.Certificate = data.HumanTrainingPlanDetail.Certificate;
            obj.Content = data.HumanTrainingPlanDetail.Content;
            obj.EmployeesID = data.HumanEmployeeID;
            obj.StartDate = data.HumanTrainingPlanDetail.FromDate;
            obj.EndDate = data.HumanTrainingPlanDetail.ToDate;
            obj.IsJoin = data.IsJoin;
            obj.ResonUnJoin = data.ResonUnJoin;
            obj.TotalPoint = data.TotalPoint;
            obj.CommentOfTeacher = data.CommentOfTeacher;
            obj.EmployeesRegister = employeeSV.GetEmployeeView(data.HumanEmployeeID);
            return obj;
        }
        public void Update(HumanTrainingPractionersItem objEdit, int userId)
        {
            var data = practionersDA.GetById(objEdit.ID);
            data.NumberPresence = objEdit.NumberPresence;
            data.NumberAbsence = objEdit.NumberAbsence;
            data.Rank = objEdit.RankID;
            data.TotalPoint = objEdit.TotalPoint;
            data.CommentOfTeacher = objEdit.CommentOfTeacher;
            data.UpdateAt = DateTime.Now;
            data.UpdateBy = userId;
            practionersDA.Update(data);
            practionersDA.Save();
        }
        public void UpdateIsInProile(int id)
        {
            var data = practionersDA.GetById(id);
            data.IsInProfile = true;
            practionersDA.Update(data);
            practionersDA.Save();
        }
        public void UpdateJoin(HumanTrainingPractionersItem objEdit, int userId)
        {
            var data = practionersDA.GetById(objEdit.ID);
            data.IsJoin = objEdit.IsJoin;
            data.ResonUnJoin = objEdit.ResonUnJoin;
            data.UpdateAt = DateTime.Now;
            data.UpdateBy = userId;
            practionersDA.Update(data);
            practionersDA.Save();
        }
        public void InsertObject(string stringId, int userId, int detailId)
        {
            var dbo = practionersDA.Repository;
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            var idsExit = dbo.HumanTrainingPractioners
                .Where(i => i.HumanTrainingPlanDetailID == detailId)
                .Select(item => item.HumanEmployeeID).ToArray();
            List<HumanTrainingPractioner> objectPractioners = new List<HumanTrainingPractioner>();
            foreach (var id in ids)
            {
                if (!idsExit.Contains((int)id))
                {
                    objectPractioners.Add(new HumanTrainingPractioner
                    {
                        HumanEmployeeID = (int)id,
                        HumanTrainingPlanDetailID = detailId,
                        TimeRegister = DateTime.Now,
                        IsRegister = false,
                        CreateAt = DateTime.Now,
                        CreateBy = userId
                    });
                };
            }
            dbo.HumanTrainingPractioners.AddRange(objectPractioners);
            dbo.SaveChanges();
        }
        public List<HumanTrainingPractionersItem> GetAll(int page, int pageSize, out int total, int detailId)
        {
            List<HumanTrainingPractionersItem> lst = new List<HumanTrainingPractionersItem>();
            var dbo = practionersDA.Repository;
            var data = practionersDA.GetQuery(
                t => t.HumanTrainingPlanDetailID == detailId
                )
                .OrderByDescending(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new HumanTrainingPractionersItem
                    {
                        ID = item.ID,
                        EmployeesID = item.HumanEmployeeID,
                        EmployeesName = dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.HumanEmployeeID) != null ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.HumanEmployeeID).Name : string.Empty,
                        IsRegister = item.IsRegister,
                        Birthday = dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.HumanEmployeeID) != null ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.HumanEmployeeID).Birthday : null,
                        Address = dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.HumanEmployeeID) != null ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.HumanEmployeeID).Address : string.Empty,
                        sex = dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.HumanEmployeeID) != null ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.HumanEmployeeID).Gender == true ? "Nam" : "Nữ" : string.Empty,
                        TimeRegister = item.TimeRegister,
                        IsJoin = item.IsJoin,
                        IsInProfile = item.IsInProfile,
                        StatusInProfile = item.IsInProfile ? "Đã lưu hồ sơ" : "Chưa lưu hồ sơ",
                        TotalPoint = item.TotalPoint,
                        NumberAbsence = item.NumberAbsence,
                        NumberPresence = item.NumberPresence,
                        RankName = item.Rank == 1 ? "Giỏi" : item.Rank == 2 ? "Khá" : item.Rank == 3 ? "Trung bình" : item.Rank == 4 ? "Yếu" : item.Rank == 5 ? "Kém" : string.Empty,
                        RankID = item.Rank,
                        CommentOfTeacher = item.CommentOfTeacher
                    });
                }
            }
            return lst;
        }

        public List<HumanTrainingPlanItem> GetPlanIsAccept(int page, int pageSize, out int total)
        {
            return new HumanTrainingPlanSV().GetPlanIsAccept(page, pageSize, out total);
        }

        public List<HumanTrainingPlanDetailItem> GetPlanDetail(int page, int pageSize, out int total, int planId)
        {
            return new HumanTrainingPlanDetailSV().GetAll(page, pageSize, out total,planId);
        }

        public List<HumanTrainingPlanDetailItem> GetAllForResultTraining(int page, int pageSize, out int total,  int planId)
        {
            return new HumanTrainingPlanDetailSV().GetAllForResultTraining(page, pageSize, out total,planId);
        }

        public HumanTrainingPlanDetailItem GetPlanDetail(int detailId)
        {
            return new HumanTrainingPlanDetailSV().GetByID(detailId);
        }

        public void Delete(int id)
        {
            practionersDA.Delete(id);
            practionersDA.Save();
        }

        public List<HumanTrainingPlanItem> GetPlanIsAcceptByDate(int page, int pageSize, out int total, DateTime searchFromDate, DateTime searchToDate)
        {
            var dbo = practionersDA.Repository;
            var data = dbo.HumanTrainingPlans.Join(dbo.QualityPlans.Where(s => s.IsAccept && s.IsApproval)
                    .Where(s => (s.EndTime <= searchToDate && s.StartTime >= searchFromDate) || (s.EndTime <= searchToDate && s.EndTime >= searchFromDate) || (s.StartTime <= searchToDate && s.StartTime >= searchFromDate)), t => t.QualityPlanID, p => p.ID, (t, p) => new HumanTrainingPlanItem()
            {
                ID = t.ID,
                Name = p.Name,
                CreateAt = p.CreateAt
            })
                            .OrderByDescending(item => item.CreateAt)
                            .Page(page, pageSize, out total)
                            .ToList();
            return data;
        }
    }
}
