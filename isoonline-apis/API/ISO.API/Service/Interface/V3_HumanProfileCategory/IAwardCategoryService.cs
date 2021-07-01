using ISO.API.Models.AwardCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service danh mục hình thức khen thưởng
    /// </summary>
    public interface IAwardCategoryService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        AwardCategoryDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        List<AwardCategoryDTO> GetAll();

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(AwardCategoryDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(AwardCategoryDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}
