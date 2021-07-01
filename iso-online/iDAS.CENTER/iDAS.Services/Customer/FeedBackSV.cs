using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using iDAS.Items;
using iDAS.Base;
using iDAS.DA;
using iDAS.Core;

namespace iDAS.Services
{
   public class FeedBackSV
    {
        FeedBackDA feedBackDA = new FeedBackDA();
        public List<FeedbackItem> GetAll(int page, int pageSize, out int total)
        {
            try
            {
                List<FeedbackItem> lst;
                 lst = feedBackDA.GetQuery().Select(
                   p=> new FeedbackItem
                    {
                        ID= p.ID,
                        Name= p.FullName,
                        Email= p.Email,
                        Phone= p.Telephone,
                        Content= p.Content,
                       
                    }).OrderBy(p=>p.Name).Page(page, pageSize, out total).ToList();
                
                 return lst;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public FeedbackItem GetByID(int id)
        {
            try
            {
                
               var p = feedBackDA.GetById(id);
                var obj = new FeedbackItem
                  {
                      Name = p.FullName,
                      Email = p.Email,
                      Phone = p.Telephone,
                      Content = p.Content,

                  };

                return obj;
            }
            catch (Exception)
            {

                throw;
            }

        }
       
       //Insert
        public void Insert(FeedbackItem obj)
        {
            try
            {
                var item = new WebFeedBack
                {
                    FullName = obj.Name,
                    Email = obj.Email,
                    Telephone = obj.Phone,
                    Content = obj.Content
                };

                feedBackDA.Insert(item);
                feedBackDA.Save();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void Update(FeedbackItem obj)
        {
            try
            {
                var item = feedBackDA.GetById(obj.ID);
                //item.Form = obj.Form;
                item.UpdateAt = DateTime.Now;
                //item.UpdateBy

                feedBackDA.Update(item);
                feedBackDA.Save();
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
