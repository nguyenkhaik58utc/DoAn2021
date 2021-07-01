using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System.IO;
using System.Xml.Linq;
using System.Data;

namespace iDAS.Services
{
    public class TemplateExportFieldSV
    {
        TemplateExportFieldDA templExportFDA = new TemplateExportFieldDA();
        public List<TemplateExportFieldItem> GetAllByTemID(int TempID)
        {
            var lst = templExportFDA.GetQuery(i => i.TemplateExportID == TempID).Select(i => new TemplateExportFieldItem()
            {
                ID = i.ID,
                TemplateExportID = i.TemplateExportID,
                Name = i.Name,
                Field = i.Field,
                Value =i.Value,
                Postition = i.Postition
            }).ToList();
            return lst;
        }
        
        public int Insert(TemplateExportFieldItem data, int userID)
        {
            var obj = new TemplateExportField();
            obj.TemplateExportID = data.TemplateExportID;
            obj.Name = data.Name;
            obj.Field = data.Field;
            obj.Postition = data.Postition;
            obj.Value = data.Value;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userID;
            templExportFDA.Insert(obj);
            templExportFDA.Save();
            return obj.ID;
        }
        public bool Update(TemplateExportFieldItem data, int userID)
        {
            try
            {
                var obj = templExportFDA.GetById(data.ID);
                if (obj != null)
                {
                    obj.Name = data.Name;
                    obj.Field = data.Field;
                    obj.Postition = data.Postition;
                    obj.Value = data.Value;
                    obj.UpdateAt = DateTime.Now;
                    obj.UpdateBy = userID;
                    templExportFDA.Save();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(int ID)
        {
            var dbo = templExportFDA.Repository;
            var obj = dbo.TemplateExportFields.FirstOrDefault(i => i.ID == ID);
            dbo.TemplateExportFields.Remove(obj);
            dbo.SaveChanges();
            return true;
        }
        public bool DeletebyTemID(int TempID)
        {
            var dbo = templExportFDA.Repository;
            var obj = dbo.TemplateExportFields.Where(i => i.TemplateExportID == TempID);
            dbo.TemplateExportFields.RemoveRange(obj);
            int rs= dbo.SaveChanges();
            if (rs > 0)
                return true;
            else
                return false;
        }
    }
}
