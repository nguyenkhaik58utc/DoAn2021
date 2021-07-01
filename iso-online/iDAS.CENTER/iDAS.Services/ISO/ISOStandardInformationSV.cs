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
    public class ISOStandardInformationSV
    {
        private ISOStandardInformationDA ISOStandardInformationDA = new ISOStandardInformationDA();
        ISOStandardDA iSOStandardDA = new ISOStandardDA();

        public bool CheckNameExits(string name, int isoId)
        {
            try
            {
                var rs = ISOStandardInformationDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper() && t.ISOStandardID == isoId
                    ).ToList();
                if (rs.Count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Người tạo: ThanhNV; ngày tạo: 29/11/2014
        /// Người update : CuongPC; ngày sửa 1/12/2014
        /// Lấy tất cả các sản phẩm
        /// </summary>
        /// <returns></returns>
        public List<ISOStandardInformationItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var Service = ISOStandardInformationDA.GetQuery()
                    .Select(i => new ISOStandardInformationItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        ISOStandardID = i.ISOStandardID,
                        IsShow = i.IsShow,
                        Order = i.Order,
                        Description = i.Description,
                        CreateAt = i.CreateAt,
                        CreateBy = i.CreateBy,
                        UpdateAt = i.UpdateAt,
                        UpdateBy = i.UpdateBy
                    })
                    .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            return Service;
        }
        public List<ISOStandardInformationItem> GetByISO(int isoStandardId)
        {
            var dbo = ISOStandardInformationDA.Repository;
            var result = ISOStandardInformationDA.GetQuery(i=>i.ISOStandardID==isoStandardId)
                    .Select(item => new ISOStandardInformationItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        SystemName =dbo.CenterSystems.Where(i=>i.ID == item.SystemID).Select(i=>i.Name).FirstOrDefault(),
                        IsShow = item.IsShow,
                        Order = item.Order,
                        Description = item.Description,
                    })
                    .OrderByDescending(item => item.Name)
                    .ToList();
            return result;
        }
        /// <summary>
        /// Người tạo: ThanhNV; ngày tạo: 29/11/2014
        /// Lấy tất cả các sản phẩm hiển thị
        /// </summary>
        /// <returns></returns>
        public List<ISOStandardInformationItem> GetAllShow(int page, int pageSize, out int totalCount)
        {
            var Service = ISOStandardInformationDA.GetQuery(i => i.IsShow == true)
                    .Select(i => new ISOStandardInformationItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        ISOStandardID = i.ISOStandardID,
                        IsShow = i.IsShow,
                        Order = i.Order,
                        Description = i.Description,
                        CreateAt = i.CreateAt,
                        CreateBy = i.CreateBy,
                        UpdateAt = i.UpdateAt,
                        UpdateBy = i.UpdateBy
                    })
                    .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            return Service;
        }
        /// <summary>
        /// Người tạo: ThanhNV; ngày tạo: 29/11/2014
        /// Lấy tất cả các sản phẩm theo Id khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ISOStandardInformationItem> GetServicebyCustomerId(int page, int pageSize, out int totalCount, int id)
        {
            //var registerServiceDa = new DA.CustomerRegisterServiceDA();
            //  int[] ids = registerServiceDa.GetQuery(i => i.CustomerID == id).Select(i => i.ServiceID).ToArray();
            var Service = new List<ISOStandardInformationItem>();
            //if (ids != null && ids.Count() > 0)
            //{
            Service = ISOStandardInformationDA.GetQuery()
                //.GetQuery(i => ids.Contains(i.ID))
                .Select(i => new ISOStandardInformationItem()
                {
                    ID = i.ID,
                    Name = i.Name,
                    ISOStandardID = i.ISOStandardID,
                    IsShow = i.IsShow,
                    Order = i.Order,
                    Description = i.Description,
                    CreateAt = i.CreateAt,
                    CreateBy = i.CreateBy,
                    UpdateAt = i.UpdateAt,
                    UpdateBy = i.UpdateBy
                }).OrderByDescending(item => item.Name).Page(page, pageSize, out totalCount).ToList();
            return Service;
            //}
            return Service.Page(page, pageSize, out totalCount).ToList();
        }
        public ISOStandardInformationItem GetById(int Id, int customerId = 0)
        {
            try
            {
                var dbo = ISOStandardInformationDA.Repository;
                var Service = ISOStandardInformationDA.GetQuery(i => i.ID == Id)
                    .Select(i => new ISOStandardInformationItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        ISOStandardID = i.ISOStandardID,
                        Content = i.Content,
                        Order = i.Order,
                        IsShow = i.IsShow,
                        SystemID = i.SystemID.HasValue?i.SystemID.Value:0,
                        Description = i.Description,
                        CreateAt = i.CreateAt,
                        CreateBy = i.CreateBy,
                        UpdateAt = i.UpdateAt,
                        UpdateBy = i.UpdateBy,

                    }).First();
                return Service;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Người tạo: ThanhNV; ngày tạo: 29/11/2014
        /// Thêm mới sản phẩm
        /// </summary>
        /// <param name="ServiceItems">sản phẩm thêm mới</param>
        /// <param name="createBy">Id người tạo</param>
        /// <returns></returns>
        ///  /// HuongLL sửa - 12/12/2014
        public bool Insert(ISOStandardInformationItem objNew)
        {
            try
            {
                ISOStandardInformation obj = new ISOStandardInformation();
                obj.Content = objNew.Content;
                obj.ISOStandardID = objNew.ISOStandardID;
                obj.Order = objNew.Order;
                obj.SystemID = objNew.SystemID;
                obj.Description = objNew.Description;
                obj.IsShow = objNew.IsShow;
                obj.Name = objNew.Name;
                obj.CreateAt = DateTime.Now;
                obj.CreateBy = objNew.CreateBy;
                ISOStandardInformationDA.Insert(obj);
                ISOStandardInformationDA.Save();
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// Người tạo: ThanhNV; ngày tạo: 29/11/2014
        /// Cập nhật sản phẩm
        /// </summary>
        /// <param name="ServiceItems">sản phẩm cập nhật</param>
        /// <param name="updateBy">Id người update</param>
        /// <returns></returns>
        ///  /// HuongLL sửa - 12/12/2014
        public bool Update(ISOStandardInformationItem objEdit)
        {
            try
            {
                var item = ISOStandardInformationDA.GetById(objEdit.ID);
                item.Content = objEdit.Content;
                item.Description = objEdit.Description;
                item.ISOStandardID = objEdit.ISOStandardID;
                item.Order = objEdit.Order;
                item.SystemID = objEdit.SystemID;
                item.IsShow = objEdit.IsShow;
                item.Name = objEdit.Name;
                item.UpdateAt = DateTime.Now;
                item.UpdateBy = objEdit.UpdateBy;
                ISOStandardInformationDA.Update(item);
                ISOStandardInformationDA.Save();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdatePrice(int id)
        {
            try
            {
                var item = ISOStandardInformationDA.GetById(id);
                ISOStandardInformationDA.Update(item);
                ISOStandardInformationDA.Save();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Người tạo: ThanhNV; ngày tạo: 29/11/2014
        /// Xóa sản phẩm theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int Id)
        {
            try
            {
                ISOStandardInformationDA.Delete(Id);
                ISOStandardInformationDA.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Người tạo: ThanhNV; ngày tạo: 29/11/2014
        /// Xóa sản phẩm theo danh sách Id
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteByListId(string stringId)
        {
            try
            {
                var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
                ISOStandardInformationDA.DeleteRange(ids);
                ISOStandardInformationDA.Save();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
