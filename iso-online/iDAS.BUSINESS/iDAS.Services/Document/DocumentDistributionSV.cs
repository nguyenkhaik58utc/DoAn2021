using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.DA;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{
    public class DocumentDistributionSV
    {
        DocumentDistributionDA distributionDA = new DocumentDistributionDA();
        DocumentSV docSV = new DocumentSV();
        HumanUserSV userSV = new HumanUserSV();
        HumanEmployeeSV employeeSV = new HumanEmployeeSV();

        //Lấy thông tin phan phối tà liệu theo từng phòng ban
        public DocumentDistributionItem GetByDepIDDocID(int Id, int docID, bool ckObs = false)
        {
            try
            {
                var item = distributionDA.GetQuery(p => p.DepartmentID == Id && p.DocumentID == docID)
                            .Select(i => new DocumentDistributionItem()
                            {
                                ID = i.ID,
                                DocumentDistributionID= i.ID,                          
                                Name = i.Document.Name,                              
                                DepartmentID = Id,
                                DocumentID = i.DocumentID,
                                Number = i.Number,
                                IssuedDate = i.Document.PublicDate,//Ngay ban hành
                                DistributionDate = i.Date,//Ngày phân phối
                                FormH = i.FormH,
                                FormS = i.FormS,
                                NoteIssues = i.Note,
                                ObsoleteDate= i.DateObsolote,
                                NumberObsolete = i.NumberObsolete != null ? (int)i.NumberObsolete : 0,
                                NoteObs = i.Note,
                                IssuedBy= i.IssuedBy,
                                ObsoleteBy= i.ObsoleteBy,
                                FormHO =i.FormHO,
                                FormSO = i.FormSO
                            })
                            .FirstOrDefault();

                if (item!= null && item.IssuedBy != null && item.IssuedBy > 0)
                {                       
                        item.EmployeeInfo = employeeSV.GetEmployeeView(item.IssuedBy); 
                        if (item.EmployeeInfo != null)
                        {
                            item.IssuedName = item.EmployeeInfo.Name;
                            item.Position = getEmployeeInfo(item.EmployeeInfo);
                            item.Avatar = item.EmployeeInfo.Avatar;
                        }
                }
                if (ckObs && item != null && item.ObsoleteBy != null && item.ObsoleteBy > 0)
                {
                 
                    item.EmployeeInfo = employeeSV.GetEmployeeView(item.ObsoleteBy);
                    if (item.EmployeeInfo != null)
                    {
                        item.ObsoleteName = item.EmployeeInfo.Name;
                        item.Position = getEmployeeInfo(item.EmployeeInfo);
                        item.Avatar = item.EmployeeInfo.Avatar;
                    }
                  
                }
                return item;
            }
            catch (Exception)
            {   
                throw;
            }
        }
        public List<DocumentDistributionItem> GetDepDistribuByDocID(int docID)
        {
            var dbo=distributionDA.Repository;
            try
            {
                var item = distributionDA.GetQuery(p => p.DocumentID == docID)
                            .Select(i => new DocumentDistributionItem()
                            {
                                ID = i.ID,
                                DocumentDistributionID = i.ID,
                                Name = i.Document.Name,
                                DepartmentID = i.DepartmentID,
                                DocumentDeptName = dbo.HumanDepartments.FirstOrDefault(x => x.ID == i.DepartmentID).Name,
                                DocumentID = i.DocumentID,
                                Number = i.Number,
                                IssuedDate = i.Document.PublicDate,//Ngay ban hành
                                DistributionDate = i.Date,//Ngày phân phối
                                FormH = i.FormH,
                                FormS = i.FormS,
                                NoteIssues = i.Note,
                                ObsoleteDate = i.DateObsolote,
                                NumberObsolete = i.NumberObsolete != null ? (int)i.NumberObsolete : 0,
                                NoteObs = i.NoteObsolete,
                                IssuedBy = i.IssuedBy,
                                ObsoleteBy = i.ObsoleteBy,
                                FormHO = i.FormHO,
                                FormSO = i.FormSO
                            }).ToList();
                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<DocumentDistributionItem> GetAllDocDistributionByDeptID(int page, int pageSize, out int totalCount,int deptID)
        {
            try
            {
                var dbo = distributionDA.Repository;
              //  var lstDeptID = deptSV.GetListDepartmentByParentID(deptID).ToList().Select(m => m.ID).ToList();
                var rs = distributionDA.GetQuery(o=>o.DepartmentID==deptID)
                            .Select(i => new DocumentDistributionItem()
                            {
                                ID = i.ID,
                                DocumentDistributionID = i.ID,
                                Name = i.Document.Name,
                                DepartmentID = i.DepartmentID,
                                DocumentID = i.DocumentID,
                                Number = i.Number,
                                IssuedDate = i.Document.PublicDate,//Ngay ban hành
                                DistributionDate = i.Date,//Ngày phân phối
                                FormH = i.FormH,
                                FormS = i.FormS,
                                NoteIssues = i.Note,
                                ObsoleteDate = i.DateObsolote,
                                NumberObsolete = i.NumberObsolete != null ? (int)i.NumberObsolete : 0,
                                NoteObs = i.NoteObsolete,
                                IssuedBy = i.IssuedBy,
                                ObsoleteBy = i.ObsoleteBy,
                                FormHO = i.FormHO,
                                FormSO = i.FormSO,
                                Department = dbo.HumanDepartments.FirstOrDefault(o=>o.ID == i.DepartmentID)!=null?dbo.HumanDepartments.FirstOrDefault(o=>o.ID == i.DepartmentID).Name : "",
                                DocumentDeptName = dbo.HumanDepartments.FirstOrDefault(o => o.ID == i.Document.DepartmentID) != null ? dbo.HumanDepartments.FirstOrDefault(o => o.ID == i.Document.DepartmentID).Name : "",
                                IssuedName =i.IssuedBy.HasValue? dbo.HumanEmployees.FirstOrDefault(o=>o.ID == i.IssuedBy).Name : "",
                                ObsoleteName = i.ObsoleteBy.HasValue ? dbo.HumanEmployees.FirstOrDefault(o => o.ID == i.ObsoleteBy).Name : "",
                                AttachmentFileIDs = dbo.DocumentAttachmentFiles.Where(t => t.DocumentID == i.DocumentID).Select(x => x.AttachmentFileID).ToList()
                            }).OrderBy(p => p.DistributionDate)
                            .Page(page, pageSize, out totalCount)
                            .ToList();
                if (rs != null && rs.Count() > 0)
                {
                    foreach (var itm in rs)
                    {
                        itm.IssuedTypeDisp = getForm(itm.FormH, itm.FormS);
                        itm.ObsoleteTypeDisp = (itm.FormHO != null || itm.FormSO != null) ? getForm(itm.FormHO, itm.FormSO) : "";
                    }
                }
                return rs;
            }catch(Exception)
            {
                throw;
            }
        }
        public List<DocumentItem> GetByDocID(int page, int pageSize, out int totalCount,int docID)
        {
            try
            {
                var dbo = distributionDA.Repository;
                var lst = distributionDA.GetQuery(p =>  p.DocumentID == docID)
                            .Select(i => new DocumentItem()
                            {
                                ID = i.ID,
                             
                                Name = i.Document.Name,
                                Department = new HumanDepartmentViewItem()
                                {
                                    ID = i.DepartmentID == null ? 0 : (int)i.DepartmentID,
                                    Name = dbo.HumanDepartments.Where(d => d.ID == i.DepartmentID).Select(d => d.Name).FirstOrDefault()
                                },
                               // Department = i.HumanDepartment.Name,
                                ParentID = i.DocumentID,
                                Number = i.Number,

                                EffectiveDate = i.Date,
                                FormH = i.FormH,
                                FormS = i.FormS,
                                
                                NoteIssues = i.Note,
                                ObsoleteDate = i.DateObsolote,
                                NumberObsolete = i.NumberObsolete != null ? (int)i.NumberObsolete : i.Number,
                                NoteObs = i.NoteObsolete,
                                IssuedBy = i.IssuedBy,
                                ObsoleteBy = i.ObsoleteBy,
                                FormHO = i.FormHO,
                                FormSO = i.FormSO,
                            })
                            .OrderBy(p=> p.Department)
                            .Page(page, pageSize, out totalCount)
                            .ToList();
                
                if (lst != null && lst.Count()>0 )
                {
                    foreach(var itm in lst)
                    {
                        itm.IssueForm = getForm(itm.FormH, itm.FormS);
                        itm.ObsoleteForm = (itm.FormHO != null && itm.FormSO != null)? getForm((bool)itm.FormHO, (bool)itm.FormSO):"";
                        if(itm.IssuedBy != null && itm.IssuedBy > 0)
                        {
                            //itm.IssuedName = employeeSV.GetNameByID((int)itm.IssuedBy);

                        }
                          

                        //if (itm.ObsoleteBy != null && itm.ObsoleteBy > 0)
                            //itm.ObsoleteName = employeeSV.GetNameByID((int)itm.ObsoleteBy);
                    }
                }
               
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DocumentItem> GetDetailByDocID(int page, int pageSize, out int totalCount, int docID)
        {
            try
            {
                var dbo = distributionDA.Repository;
                var lst = distributionDA.GetQuery(p => p.DocumentID == docID)
                            .Select(i => new DocumentItem()
                            {
                                ID = i.ID,
                                Name = i.Document.Name,
                                Department = new HumanDepartmentViewItem()
                                {
                                    ID = i.DepartmentID == null ? 0 : (int)i.DepartmentID,
                                    Name = dbo.HumanDepartments.Where(d => d.ID == i.DepartmentID).Select(d => d.Name).FirstOrDefault()
                                },
                                ParentID = i.DocumentID,
                                Number = i.Number,
                                DistributionDate = i.Date,
                                FormH = i.FormH,
                                FormS = i.FormS,
                                NoteIssues = i.Note,
                                ObsoleteDate = i.DateObsolote,
                                NumberObsolete = i.NumberObsolete != null ? (int)i.NumberObsolete : 0,
                                NoteObs = i.NoteObsolete,
                                IssuedDate= i.Document.PublicDate,
                                IssuedBy = i.IssuedBy,
                                ObsoleteBy = i.ObsoleteBy,
                                FormHO = i.FormHO,
                                FormSO = i.FormSO,
                            })
                            .OrderBy(p => p.Department)
                            .Page(page, pageSize, out totalCount)
                            .ToList();

                if (lst != null && lst.Count() > 0)
                {
                    foreach (var itm in lst)
                    {
                        itm.IssueForm = getForm(itm.FormH, itm.FormS);
                        itm.ObsoleteForm = (itm.FormHO != null || itm.FormSO != null) ? getForm(itm.FormHO, itm.FormSO) : "";
                        if (itm.IssuedBy != null && itm.IssuedBy > 0)
                            itm.IssuedName = employeeSV.GetNameByID((int)itm.IssuedBy);

                        if (itm.ObsoleteBy != null && itm.ObsoleteBy > 0)
                            itm.ObsoleteName = employeeSV.GetNameByID((int)itm.ObsoleteBy);
                    }
                }

                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(DocumentDistributionItem obj)
        {
            try
            {
                var itm = new DocumentDistribution
                  {
                      DepartmentID =(int) obj.DepartmentID,
                      DocumentID =(int)obj.DocumentID,
                      Number = obj.Number,
                      Note = obj.NoteObs,
                      FormH =obj.FormH,
                      FormS = obj.FormS,
                      Date = (DateTime)obj.DistributionDate,
                      CreateAt = DateTime.Now,
                      CreateBy = obj.CreateBy,
                      IssuedBy= obj.EmployeeInfo.ID

                  };
                distributionDA.Insert(itm);
                distributionDA.Save();
                return itm.ID;
            }
            catch (Exception)
            {                
                throw;
            }
        }


        //Cập nhật thông tin Phân phối TL
        public void UpdateDistribution(DocumentDistributionItem obj)
        {
            try
            {
                var itm = distributionDA.GetById(obj.DocumentDistributionID);
                itm.Number = obj.Number;
                itm.Note = obj.NoteObs;
                itm.FormH = obj.FormH;
                itm.FormS = obj.FormS;
                itm.Date = (DateTime)obj.DistributionDate;

                itm.IssuedBy = obj.EmployeeInfo.ID;
             
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                distributionDA.Update(itm);
                distributionDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Cập nhật thông tin Thu hồi Tl
        public void UpdateObsolete(DocumentDistributionItem obj)
        {
            try
            {
                var itm = distributionDA.GetById(obj.DocumentDistributionID);
             
                itm.NumberObsolete = obj.NumberObsolete;
                itm.NoteObsolete = obj.NoteObs;
                itm.ObsoleteBy = obj.EmployeeInfo.ID;            
                itm.DateObsolote = obj.ObsoleteDate;
                itm.FormHO = obj.FormHO;
                itm.FormSO = obj.FormSO;
                itm.UpdateAt = DateTime.Now;
                itm.UpdateBy = obj.UpdateBy;
                distributionDA.Update(itm);
                distributionDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string getForm(bool? isHard, bool? isSoft)
        {
            if (isHard != null && isSoft != null && isHard == true&& isSoft== true)
                return "1. Bản cứng <br>2. Bản mềm";
            else if (isHard != null && isHard== true)
                return "1. Bản cứng";
            else if (isSoft != null && isSoft== true)
                return "1. Bản mềm";
            return "";

        }

        private string getEmployeeInfo(HumanEmployeeViewItem obj)
        {
            string role = "N/A", dept = "N/A";
            if (obj.Role != null && !obj.Role.Trim().Equals(""))
                role = obj.Role;
            if (obj.Department != null && !obj.Department.Trim().Equals(""))
                dept = obj.Department;

            return "Phòng ban:" + role + "<br> Chức danh:" + dept;

        }

        public DocumentDistributionItem GetByID(int id)
        {
            var i = distributionDA.GetById(id);
            
            if (i == null)
                return null;
            else
            {
                DocumentDistributionItem item = new DocumentDistributionItem()
                {
                    ID = i.ID,
                    DocumentDistributionID = i.ID,
                    Name = i.Document.Name,
                    DepartmentID = i.DepartmentID,
                    DocumentID = i.DocumentID,
                    Number = i.Number,
                    IssuedDate = i.Document.PublicDate,//Ngay ban hành
                    DistributionDate = i.Date,//Ngày phân phối
                    FormH = i.FormH,
                    FormS = i.FormS,
                    NoteIssues = i.Note,
                    ObsoleteDate = i.DateObsolote,
                    NumberObsolete = i.NumberObsolete != null ? (int)i.NumberObsolete : 0,
                    NoteObs = i.Note,
                    IssuedBy = i.IssuedBy,
                    ObsoleteBy = i.ObsoleteBy,
                    FormHO = i.FormHO,
                    FormSO = i.FormSO
                };
                if (i != null && i.IssuedBy != null && i.IssuedBy > 0)
                {
                    item.EmployeeInfo = employeeSV.GetEmployeeView(item.IssuedBy);
                    if (item.EmployeeInfo != null)
                    {
                        item.IssuedName = item.EmployeeInfo.Name;
                        item.Position = getEmployeeInfo(item.EmployeeInfo);
                        item.Avatar = item.EmployeeInfo.Avatar;
                    }
                }
                return item;
            }
                
        }

        public void Delete(int id)
        {
            var item = distributionDA.GetById(id);
            distributionDA.Delete(item);
            distributionDA.Save();
        }
    }
}
