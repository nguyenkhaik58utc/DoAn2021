using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.DAL.MnCustomer;
using iDAS.Items.MnCustomer;
using iDAS.Core;
namespace iDAS.Services.MnCustomer
{
    public class DemandService
    {
        DemandDA demandDA = new DemandDA();

        public List<DemandItem> GetAll(int customerId)
        {
            var surveys = demandDA.GetQuery(filter: s => s.CustomerID == customerId)
                        .Select(s => new DemandItem() {
                            ID = s.ID,
                            CustomerID = s.CustomerID,
                            ProductID = s.ProductID,
                            UpdateAt = s.UpdateAt,
                            UpdateBy = s.UpdateBy,
                        }).ToList();
            return surveys;
        }

        public List<DemandItem> GetAll(int customerId, int categoryId, int page, int pageSize, out int totalCount)
        {
            var dbo = demandDA.SystemContext;
            var demands = dbo.MnCustomerProducts.Where(item => item.CategoryID == categoryId)
                            .Select(item =>
                                 new DemandItem()
                                    {
                                        ID = item.MnCustomerDemands.FirstOrDefault(i => i.CustomerID == customerId).ID,
                                        CustomerID = customerId,
                                        ProductID = item.ID,
                                        ProductName = item.Name,
                                        Time = item.MnCustomerDemands.FirstOrDefault(i => i.CustomerID == customerId).Time,
                                        Price = item.MnCustomerDemands.FirstOrDefault(i => i.CustomerID == customerId).Price,
                                        Quantity = item.MnCustomerDemands.FirstOrDefault(i => i.CustomerID == customerId).Quantity,
                                        IsSelected = item.MnCustomerDemands.FirstOrDefault(i => i.CustomerID == customerId) != null,
                                        UpdateAt = item.MnCustomerDemands.FirstOrDefault(i => i.CustomerID == customerId).UpdateAt,
                                        UpdateBy = item.MnCustomerDemands.FirstOrDefault(i => i.CustomerID == customerId).UpdateBy,
                                        UpdateByName = dbo.SystemUsers.FirstOrDefault(i => i.ID == item.MnCustomerDemands.FirstOrDefault(d => d.CustomerID == customerId).UpdateBy).FullName,
                                    }
                            )
                            .OrderByDescending(item => item.UpdateAt)
                            .Page(page, pageSize, out totalCount)
                            .ToList();

            return demands;
        }

        public List<DemandItem> GetAll(int customerId, int page, int pageSize, out int totalCount)
        {
            var dbo = demandDA.SystemContext;
            var demands = dbo.MnCustomerDemands.Where(item=>item.CustomerID == customerId)
                            .Select(item =>
                                 new DemandItem()
                                 {
                                     ID = item.ID,
                                     CustomerID = customerId,
                                     ProductID = item.ID,
                                     ProductName = dbo.MnCustomerProducts.FirstOrDefault(i=>i.ID == item.ProductID).Name,
                                     Time = item.Time,
                                     TimeBack = item.TimeBack,
                                     Price = item.Price,
                                     IsSelected = true,
                                     Quantity = item.Quantity,
                                     IsDisabled = item.IsDisabled,
                                 }
                            )
                            .OrderByDescending(item => item.Time)
                            .Page(page, pageSize, out totalCount)
                            .ToList();

            return demands;
        }

        public DemandItem Get(int customerId, int productId)
        {
            var survey = demandDA.GetQuery(filter: s => s.CustomerID == customerId && s.ProductID == productId).FirstOrDefault();
            if (survey != null)
            {
                return new DemandItem()
                {
                    ID = survey.ID,
                    CustomerID = survey.CustomerID,
                    ProductID = survey.ProductID,
                    UpdateAt = survey.UpdateAt,
                    UpdateBy = survey.UpdateBy,
                };
            }
            return null;
        }

        public void Insert(DemandItem item)
        {
            var demand = new MnCustomerDemand()
            {
                CustomerID = item.CustomerID,
                ProductID = item.ProductID,
                Time = item.Time,
                Price = item.Price,
                Quantity = item.Quantity,
                UpdateAt = DateTime.Now,
                UpdateBy = item.UpdateBy,
            };
            demandDA.Insert(demand);
            demandDA.Save();
        }
        public void Update(DemandItem item)
        {
            var demand = demandDA.GetById(item.ID);
            demand.Time = item.Time;
            demand.Price = item.Price;
            demand.Quantity = item.Quantity;
            demand.UpdateAt  = DateTime.Now;
            demand.UpdateBy = item.UpdateBy;
            demandDA.Save();
        }
        public void Delete(int id)
        {
            demandDA.Delete(id);
            demandDA.Save();
        }
    }
}
