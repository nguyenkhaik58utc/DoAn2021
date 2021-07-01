using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class ComboboxSV //where TEntity : class
    {
        public List<ComboboxItem> Combobox(IQueryable l)
        {
            List<ComboboxItem> lst = new List<ComboboxItem>();
            ComboboxItem item; 
            foreach (var e in l)
            {
                item = new ComboboxItem();
                foreach (var p in e.GetType().GetProperties())
                {
                    if (p.Name.ToUpper() == "ID")
                    {
                        item.ID = Convert.ToInt32(p.GetValue(e, null));
                    }
                    if (p.Name.ToUpper() == "NAME" || p.Name.ToUpper() == "FULLNAME")
                    {
                        item.Name = Convert.ToString(p.GetValue(e, null));
                        item.FullName = Convert.ToString(p.GetValue(e, null));

                    }
                    if (p.Name.ToUpper() == "DESCRIPTION")
                    {
                        item.Description = Convert.ToString(p.GetValue(e, null));
                    }

                    if (p.Name.ToUpper() == "ADDRESS")
                    {
                        item.Address = Convert.ToString(p.GetValue(e, null));
                    }
                    if (p.Name.ToUpper() == "NUMBERREQUEST")
                    {
                        item.Name = Convert.ToString(p.GetValue(e, null));
                    }
                } lst.Add(item);
            }
            return lst;
        }
    }
}
