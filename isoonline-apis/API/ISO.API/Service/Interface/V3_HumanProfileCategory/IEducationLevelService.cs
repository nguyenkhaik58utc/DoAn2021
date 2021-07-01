using ISO.API.Models.EducationLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
   /// <summary>
   /// Service danh mục trình độ đào tạo
   /// </summary>
    public interface IEducationLevelService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        EducationLevelDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        List<EducationLevelDTO> GetAll();

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(EducationLevelDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(EducationLevelDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);
    }
}