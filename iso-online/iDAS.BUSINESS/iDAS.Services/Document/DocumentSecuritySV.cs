using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Base;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{ 
    public class DocumentSecuritySV
    {
        DocumentSecurityDA docSercurityDA = new DocumentSecurityDA();
        HumanUserSV userSV = new HumanUserSV();

        public List<DocumentSecurityItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var services = docSercurityDA.GetQuery()
                    .Select(i => new DocumentSecurityItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                       Color= i.Color,
                       Note= i.Note,
                       IsActive= i.IsActive
                    })
                    .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
           
            return services;
        }

      
        public List<DocumentSecurityItem> GetAll()
        {
            var services = docSercurityDA.GetQuery(p=> p.IsActive)
                    .Select(i => new DocumentSecurityItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        Color = i.Color,
                    })
                    .OrderByDescending(item => item.Name)                    
                    .ToList();

            return services;
        }
        public void Insert(DocumentSecurityItem obj)
        {
            try
            {
                var itm = new DocumentSecurity
                {
                    Name = obj.Name,
                    Color = obj.Color,
                    Note = obj.Note,
                    IsActive=obj.IsActive,
                    IsDelete = false,
                   
                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy
                };


                docSercurityDA.Insert(itm);
                docSercurityDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(DocumentSecurityItem obj)
        {
            var itm = docSercurityDA.GetById(obj.ID);

            itm.Name = obj.Name;
            itm.Color = obj.Color;
            itm.Note = obj.Note;
            itm.IsActive = obj.IsActive;
           

            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;

            docSercurityDA.Update(itm);
            docSercurityDA.Save();
        }
        public void Delete(int id)
        {
            try
            {
                var item = docSercurityDA.GetById(id);
                docSercurityDA.Delete(item);
                docSercurityDA.Save();
            }
            catch (Exception)
            {

                throw;
            }

        }



        public DocumentSecurityItem GetByName(string name)
        {
            name = name.Trim().ToLower();
            var obj = docSercurityDA.GetQuery(p=> p.Name.Trim().ToLower().Equals(name))                                   
                  .FirstOrDefault();
            if (obj != null)
                return convertToDocumentSecurityItem(obj);

            return null;
        }

        public DocumentSecurityItem GetByID(int id)
        {
            var obj = docSercurityDA.GetById(id);
            if(obj != null) 
                return convertToDocumentSecurityItem(obj);
                
            return null;
        }


        public bool CheckNotIsUse(int id)
        {
            DocumentDA doc = new DocumentDA();
            var obj = doc.GetQuery(p => p.DocumentSecurityID==id);
            if(obj.Count()>0)
                return false;

            ProfileDA profileDa = new ProfileDA();
            var obj1 = profileDa.GetQuery(p => p.ProfileSecurityID == id);
            if (obj1.Count() > 0)
                return false;

            return true;
        }

        public void DeleteRange(List<object> ids)
        {
            throw new NotImplementedException();
        }

        private DocumentSecurityItem convertToDocumentSecurityItem(DocumentSecurity i)
        {
          var obj=  new DocumentSecurityItem()
                   {
                       ID = i.ID,
                       Name = i.Name,
                       Color = i.Color,
                       Note = i.Note,
                       IsActive = i.IsActive,
                       CreateAt= i.CreateAt,
                       CreateBy= i.CreateBy,
                       UpdateAt= i.UpdateAt,
                       UpdateBy= i.UpdateBy
                   };
            if(obj != null)
            {
                obj.CreateName = userSV.GetNameByUserID(obj.CreateBy);
                obj.UpdateName = userSV.GetNameByUserID(obj.UpdateBy);
            }
          return obj;
        }
    }
}
