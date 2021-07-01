using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace iDAS.Services
{
    public class ReportTemplateSV : BaseService
    {
        private ReportTemplateDA reportTemplateDA = new ReportTemplateDA();
        public List<ReportTemplateItem> GetReportTemplates(string moduleCode, string functionCode) {
            var lst = reportTemplateDA.GetQuery().Where(i => i.IsDelete == false)
                        .Where(i => i.ModuleCode == moduleCode)
                        .Where(i => i.FunctionCode == functionCode)
                        .Select(i => new ReportTemplateItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Path = i.PathUpload,
                            File = i.File,
                            Size = i.Size,
                            Type = i.Type,
                            IsActive = i.IsActive,
                            IsMap = i.IsMap,
                            UpdateAt = i.UpdateAt,
                            CreateAt = i.CreateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .ToList();
            return lst;
        }
        public void Insert(ReportTemplateItem item)
        {
            var file = item.FileUpload.FileAttachments[0];
            if (file != null)
            {
                var pathUpload = "~/FileData/" + User.Code.ToUpper() + "/ReportTemplate/" + item.ModuleCode + "/" + item.FunctionCode + "/";
                var pathReport = "~/FileData/" + User.Code.ToUpper() + "/Report/" + item.ModuleCode + "/" + item.FunctionCode + "/";
                var pathSave = HttpContext.Current.Server.MapPath(pathUpload);
                if (!System.IO.Directory.Exists(pathSave))
                {
                    System.IO.Directory.CreateDirectory(pathSave);
                }
                file.SaveAs(pathSave + file.FileName);
                var reportTemplate = new ReportTemplate()
                {
                    ModuleCode = item.ModuleCode,
                    FunctionCode = item.FunctionCode,
                    Name = item.Name,
                    File = file.FileName,
                    PathUpload = pathUpload,
                    PathReport = pathReport,
                    Size = file.ContentLength,
                    Type = file.ContentType,
                    IsMap = false,
                    IsActive = item.IsActive,
                    IsDelete = false,
                    CreateAt = DateTime.Now,
                    CreateBy = User.ID,
                };
                reportTemplateDA.Insert(reportTemplate);
                reportTemplateDA.Save();
            }
        }
        public string Export(object data, int reportTemplateID){
            var temp = reportTemplateDA.GetById(reportTemplateID);
            ReportInfo info = new ReportInfo();
            
            var reports = temp.Reports.Where(i => !string.IsNullOrEmpty(i.ValueMap));
            foreach (var f in reports) {
                if (f.Type == (int)ReportType.Field) {
                    info.Fields.Add(new FieldReport() { 
                        Name = f.Name,
                        Code = f.Code,
                        ValueMap = f.ValueMap,
                    });
                }
                if (f.Type == (int)ReportType.Table) {
                    var columns = reports.Where(i => i.ParentID == f.ID);
                    var tab = new TableReport() {
                        Name = f.Name,
                        Code = f.Code,
                        ValueMap = f.ValueMap,
                    };
                    foreach (var c in columns) {
                        tab.Columns.Add(new FieldReport()
                        {
                            Name = c.Name,
                            Code = c.Code,
                            ValueMap = c.ValueMap,
                        });
                    }
                    info.Tables.Add(tab);
                }
            }
            var pathsave = HttpContext.Current.Server.MapPath(temp.PathReport);
            var filesave = "[" + DateTime.Now.ToString("dd-MM-yyyy hh.mm") + "]" + temp.Name.Replace(" ", "_") + ".pdf";
            info.PathOpen = HttpContext.Current.Server.MapPath(temp.PathUpload + temp.File);
            info.PathSave = pathsave + filesave; //+ temp.File.Split('.').LastOrDefault();
            
            if (!System.IO.Directory.Exists(pathsave))
            {
                System.IO.Directory.CreateDirectory(pathsave);
            }
            new iDAS.Core.Report().Export(data, info);
            return (HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "") + temp.PathReport + filesave).Replace("~","");
        }
        public bool CheckNameExist(string name)
        {
            var check = reportTemplateDA.GetQuery()
                        .Where(i => i.Name.ToUpper().Equals(name.Trim().ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
